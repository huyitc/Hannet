import { NotificationService } from './../../../core/services/notification.service';
import { DataService } from 'src/app/core/services/data.service';
import { AbstractControl, FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { SelectionModel } from '@angular/cdk/collections';
import { Component, ElementRef, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { TranslateService } from '@ngx-translate/core';
import { PaginatorCustomService } from 'src/app/core/services/paginator-custom.service';
import { COMMA, ENTER } from '@angular/cdk/keycodes';
import { MessageConstants } from 'src/app/core/common/message.constants';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { MatChipInputEvent } from '@angular/material/chips';
import { SystemConstants } from 'src/app/core/common/system.constants';
import { validateBasis } from '@angular/flex-layout';

const user = JSON.parse(localStorage.getItem(SystemConstants.CURRENT_USER)!);

@Component({
  selector: 'app-user',
  templateUrl: './app-user.component.html',
  styleUrls: ['./app-user.component.css']
})
export class AppUserComponent implements OnInit {

  action: string = '';
  title: string = '';
  displayedColumns: string[] = ['select', 'position', 'userName', 'fullName', 'email', 'phoneNumber', 'createdDate', 'createdBy', 'action',];
  dataSource = new MatTableDataSource<any>();
  preventAbuse = false;
  page = 0;
  keyword: string = '';
  totalRow: number = 0;
  totalPage: number = 0;
  roleParents: any;
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  // @ViewChild(MatSort) sort!: MatSort;
  @ViewChild('dialog') dialogTemplate!: TemplateRef<any>;

  pageSizeOptions: number[] = [5, 10, 25, 100];
  pageSize = this.pageSizeOptions[0];
  isAllChecked = false;
  selection = new SelectionModel<any>(true, []);
  userForm!: FormGroup;
  groups: any;
  separatorKeysCodes: number[] = [ENTER, COMMA];
  filePath: string = '';
  listGroups!: any[];
  @ViewChild('groupInput') groupInput!: ElementRef<HTMLInputElement>;
  fruitCtrl = new FormControl();
  filteredFruits: Observable<any[]>;
  employees! : any[];

  constructor(private translateService: TranslateService, private dialog: MatDialog, private pagin: PaginatorCustomService, private formBuilder: FormBuilder, private dataService: DataService, private notificationService: NotificationService) {
    this.userForm = this.formBuilder.group({
      id: '',
      userName: ['', Validators.compose([Validators.required, Validators.maxLength(50)])],
      password: ['', Validators.compose([Validators.maxLength(50), Validators.minLength(6)])],
      fullName: ['', Validators.compose([Validators.required, Validators.maxLength(100)])],
      phoneNumber: '',
      email: ['', Validators.compose([Validators.email, Validators.maxLength(50)])],
      image: '',
      groups: [],
      createdDate: '',
      createdBy: '',
      updatedBy: '',
      updatedDate: ''
      //emId:['', Validators.compose([Validators.required])]
    });
    this.filteredFruits = this.fruitCtrl.valueChanges.pipe(
      startWith(null),
      map((fruit: any | null) => (fruit ? this._filter(fruit) : this.groups.slice())),
    );
    this.listGroups = [];
  }



  ngOnInit(): void {
    this.loadAllUsers();
    this.getAllGroups();
    //this.getEmployees();
  }


  openDialog(action: string, item?: any, config?: MatDialogConfig) {
    this.action = action;
    config = { width: '750px' }
    if (action == 'create') {
      this.translateService.get('appUserModule.createMessage').subscribe(data => this.title = data);
      this.userForm.controls["password"].setValidators([Validators.minLength(6), Validators.maxLength(50), Validators.required]);
    }
    else {
      this.clearPasswordValidators();

      this.translateService.get('appUserModule.editMessage').subscribe(data => this.title = data);
      this.userForm.controls['id'].setValue(item.id);
      let itemFilter = this.dataSource.filteredData.filter((x: any) => x.id == item.id)[0];
      this.userForm.controls['userName'].setValue(itemFilter.userName);
      this.userForm.controls['fullName'].setValue(itemFilter.fullName);
      this.userForm.controls['image'].setValue(itemFilter.image);
      this.userForm.controls['email'].setValue(itemFilter.email);
      this.userForm.controls['phoneNumber'].setValue(itemFilter.phoneNumber);
      this.userForm.controls['createdDate'].setValue(itemFilter.createdDate);
      this.userForm.controls['createdBy'].setValue(itemFilter.createdBy);
      this.userForm.controls['updatedDate'].setValue(itemFilter.updatedDate);
      this.userForm.controls['updatedBy'].setValue(itemFilter.updatedBy);
      this.userForm.controls['groups'].setValue(itemFilter.groups);
      //this.userForm.controls['emId'].setValue(itemFilter.emId);
      this.listGroups = itemFilter.groups;
    }



    const dialogRef = this.dialog.open(this.dialogTemplate, config);
    dialogRef.afterClosed().subscribe(result => {
      this.onReset();
    });
  }

  loadAllUsers() {
    this.preventAbuse = true;
    this.dataService.get('appusers/getlistpaging?page=' + this.page + '&pageSize=' + this.pageSize + '&keyword=' + this.keyword).subscribe((data: any) => {
      this.dataSource = new MatTableDataSource(data.items);
      this.totalRow = data.totalCount;
      this.preventAbuse = false;
    }, err => {
      this.translateService.get('messageSystem.loadFail').subscribe(data => MessageConstants.GET_FAILSE_MSG = data);
      this.notificationService.printErrorMessage(MessageConstants.GET_FAILSE_MSG);
      this.preventAbuse = false;
    });

  }

  getAllGroups() {
    this.dataService.get('appgroups/getall').subscribe((data: any) => {
      this.groups = data;
    }, err => {
    });
  }

  //getall employee
  getEmployees() {
    this.dataService.get('TEmployee/getAllEmployee').subscribe((data: any) => {
      this.employees = data;
    }, err => {

    });
  }

  addData() {
    if (this.userForm.invalid) {
      return;
    }
    this.preventAbuse = true;
    this.userForm.controls['groups'].setValue(this.listGroups);
    if (this.action == 'create') {
      let role = {
        userName: this.userForm.controls['userName'].value,
        fullName: this.userForm.controls['fullName'].value,
        //fullName: this.employees.filter(x => x.emId == this.userForm.controls['emId'].value)[0].emName,
        passwordHash: this.userForm.controls['password'].value,
        phoneNumber: this.userForm.controls['phoneNumber'].value,
        email: this.userForm.controls['email'].value,
        image: this.userForm.controls['image'].value != '' ? this.userForm.controls['image'].value : null,
        groups: this.userForm.controls['groups'].value != [] ? this.userForm.controls['groups'].value : null,
        createdBy: user.id
        //emId : this.userForm.controls['emId'].value
      }
      this.dataService.post('appusers/create', role).subscribe(data => {
        let message!: string;
        this.translateService.get('messageSystem.createSuccess').subscribe(mes => message = mes);
        this.notificationService.printSuccessMessage(message);
        this.preventAbuse = false;
        this.loadAllUsers();
        this.onReset();
      }, err => {
        let error!: string;
        this.translateService.get('messageSystem.createFail').subscribe(mes => error = mes);
        this.notificationService.printErrorMessage(err);
        this.preventAbuse = false;
      });
    }
    else if (this.action == 'edit') {
      let role = {
        userName: this.userForm.controls['userName'].value,
        fullName: this.userForm.controls['fullName'].value,
        passwordHash: this.userForm.controls['password'].value,
        phoneNumber: this.userForm.controls['phoneNumber'].value,
        email: this.userForm.controls['email'].value,
        image: this.userForm.controls['image'].value != '' ? this.userForm.controls['image'].value : null,
        id: this.userForm.controls['id'].value,
        groups: this.userForm.controls['groups'].value != [] ? this.userForm.controls['groups'].value : null,
        updatedBy: user.id,
        createdDate: this.userForm.controls['createdDate'].value,
        createdBy: this.userForm.controls['createdBy'].value,
        emId : this.userForm.controls['emId'].value
      }
      this.dataService.put('appusers/update', role).subscribe(data => {
        let message!: string;
        this.translateService.get('messageSystem.updateSuccess').subscribe(mes => message = mes);
        this.notificationService.printSuccessMessage(message);
        this.preventAbuse = false;
        this.loadAllUsers();
        this.onReset();
      }, err => {
        let error!: string;
        this.translateService.get('messageSystem.updateFail').subscribe(mes => error = mes);
        this.notificationService.printErrorMessage(err);
        this.preventAbuse = false;
      });
    }

  }

  removeData() {
    let roleChecked: any[] = [];
    this.selection.selected.forEach((value: any) => {
      let id = value.id;
      roleChecked.push(id);
    });
    this.translateService.get('messageSystem.confirmDelete').subscribe(data => MessageConstants.CONFIRM_DELETE_MSG = data);
    this.notificationService.printConfirmationDialog(MessageConstants.CONFIRM_DELETE_MSG, () => this.deleteItemConfirm(JSON.stringify(roleChecked)));
  }

  deleteItemConfirm(id: string) {
    this.preventAbuse = true;
    this.dataService.delete('appusers/deletemulti', 'checkedList', id).subscribe(response => {
      this.translateService.get('messageSystem.deleteSuccess').subscribe(data => MessageConstants.DELETED_OK_MSG = data);
      this.notificationService.printSuccessMessage(MessageConstants.DELETED_OK_MSG);
      this.preventAbuse = false;
      this.selection.clear();
      this.loadAllUsers();
    }, err => {
      this.translateService.get('messageSystem.deletefail').subscribe(data => MessageConstants.DELETE_FAILSE_MSG = data);
      this.notificationService.printErrorMessage(MessageConstants.DELETE_FAILSE_MSG);
      this.preventAbuse = false;

    });
  }
  applyFilter(event: Event) {
    //const filterValue = (event.target as HTMLInputElement).value;
    this.keyword = (event.target as HTMLInputElement).value;
    this.page = 0;
    this.loadAllUsers();
    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  /** Whether the number of selected elements matches the total number of rows. */
  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.data.length;
    return numSelected === numRows;
  }

  /** Selects all rows if they are not all selected; otherwise clear selection. */
  masterToggle() {
    if (this.isAllSelected()) {
      this.selection.clear();
      return;
    }

    this.selection.select(...this.dataSource.data);

  }

  /** The label for the checkbox on the passed row */
  checkboxLabel(row?: any): string {
    if (!row) {
      return `${this.isAllSelected() ? 'deselect' : 'select'} all`;
    }
    return `${this.selection.isSelected(row) ? 'deselect' : 'select'} row ${row.position + 1}`;
  }

  ngAfterViewInit() {
    this.paginator._intl.itemsPerPageLabel = this.pagin.setLable;
    this.paginator._intl.firstPageLabel = this.pagin.firstButton;
    this.paginator._intl.nextPageLabel = this.pagin.nextButton;
    this.paginator._intl.lastPageLabel = this.pagin.lastButton;
    this.paginator._intl.previousPageLabel = this.pagin.preButton;
  }

  onChangePage(pe: PageEvent) {
    this.page = pe.pageIndex;
    this.pageSize = pe.pageSize;
    this.loadAllUsers();
  }

  get getValidForm(): { [key: string]: AbstractControl } {
    return this.userForm.controls;
  }

  get password() {
    return this.userForm.get('password');
  }

  clearPasswordValidators() {
    this.password!.clearValidators();
    this.password!.updateValueAndValidity();
  }


  imagePreview(e: any) {
    const file = (e.target as HTMLInputElement).files![0];
    const reader = new FileReader();
    reader.onload = () => {
      let base64String = reader.result as string;
      let img = base64String.split('base64,')[1];
      this.userForm.controls['image'].setValue(img);
    }
    reader.readAsDataURL(file);

  }

  selected(event: MatAutocompleteSelectedEvent): void {
    let option = event.option.value;
    if (!this.listGroups.includes(option))
      this.listGroups.push(option);
    this.groupInput.nativeElement.value = '';
    this.fruitCtrl.setValue(null);
  }

  remove(fruit: string): void {
    const index = this.listGroups.indexOf(fruit);
    if (index >= 0) {
      this.listGroups.splice(index, 1);
      this.groups.push(fruit);
    }

  }

  private _filter(value: any): string[] {
    const filterValue = value.groupCode?.toLowerCase();
    return this.groups.filter((fruit: any) => fruit?.groupCode.toLowerCase().includes(filterValue));
  }

  onReset() {
    this.action == '';
    this.clearPasswordValidators();
    this.listGroups = [];
    this.userForm.reset();
    this.dialog.closeAll();
  }

  clearImage() {
    this.userForm.controls['image'].setValue('');
  }

}
