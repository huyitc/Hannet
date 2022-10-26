import { SelectionModel } from '@angular/cdk/collections';
import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { TranslateService } from '@ngx-translate/core';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { MessageConstants } from 'src/app/core/common/message.constants';
import { DataService } from 'src/app/core/services/data.service';
import { NotificationService } from 'src/app/core/services/notification.service';
import { PaginatorCustomService } from 'src/app/core/services/paginator-custom.service';
import { NgxSpinnerService } from 'ngx-spinner';
@Component({
  selector: 'app-t-regency',
  templateUrl: './t-regency.component.html',
  styleUrls: ['./t-regency.component.css']
})
export class TRegencyComponent implements OnInit {

  displayedColumns: string[] = ['select', 'position', 'regencyName', 'regDescription', 'regStatus', 'action',];
  dataSource = new MatTableDataSource<any>();
  preventAbuse = false;
  page = 0;
  keyword: string = '';
  totalRow: number = 0;
  totalPage: number = 0;
  regParents: any;
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  @ViewChild('dialog') dialogTemplate!: TemplateRef<any>;
  selection = new SelectionModel<any>(true, []);
  regencyForm!: FormGroup;
  action: string = '';
  title: string = '';
  pageSizeOptions: number[] = [10, 25, 50, 100];
  pageSize = this.pageSizeOptions[0];

  isAllChecked = false;
  parentName: any;
  filteredOptions!: Observable<any[]>;
  users: any;
  RegencyParentControl = new FormControl();

  constructor(
    private spinner: NgxSpinnerService,private dataService: DataService, public dialog: MatDialog,
    private notificationService: NotificationService, private pagin: PaginatorCustomService,
    private translateService: TranslateService, private formBuilder: FormBuilder) {
    this.regencyForm = this.formBuilder.group({
      regId: '',
      regName: ['', Validators.compose([Validators.required, Validators.maxLength(100)])],
      regDescription: [''],
      regStatus: '',
    })
    this.filteredOptions = this.RegencyParentControl.valueChanges.pipe(
      startWith(''),
      map(value => (typeof value === 'string' ? value : value)),
      map(name => (name ? this._filter(name) : this.regParents.slice())),
    );
  }


  ngOnInit(): void {
    this.getTRegencieListPaging();
    this.getRegencies();
  }

  //option with regDescription
  private _filter(value: any): any {
    const filterValue = value?.toLowerCase();
    return this.regParents.filter((option: any) => option?.regDescription.toLowerCase().includes(filterValue));
  }

  //open dialog add or edit
  openDialog(action: string, item?: any, config?: MatDialogConfig) {
    this.action = action;
    if (action == 'create') {
      this.translateService.get('tRegencyModule.createMessage').subscribe(data => this.title = data);
      this.regencyForm.controls['regStatus'].setValue(true);
    }
    else {
      this.translateService.get('tRegencyModule.editMessage').subscribe(data => this.title = data);
      this.regencyForm.controls['regId'].setValue(item.regId);
      let itemFilter = this.dataSource.filteredData.filter((x: any) => x.regId == item.regId)[0];
      this.regencyForm.controls['regName'].setValue(itemFilter.regName);
      this.regencyForm.controls['regDescription'].setValue(itemFilter.regDescription);
      this.regencyForm.controls['regStatus'].setValue(itemFilter.regStatus);
      let parent = this.regParents.filter((x: any) => x.regId == item.regParentId)[0];
      this.parentName = parent?.depDescription;
      this.RegencyParentControl.setValue(this.parentName);
    }
    const dialogRef = this.dialog.open(this.dialogTemplate, { width: '650px' });
    dialogRef.disableClose = true;
    dialogRef.afterClosed().subscribe(result => {
      this.onReset();
    })
  }
  onReset() {
    // this.parentName=undefined;
    this.action == '';
    this.regencyForm.reset();

  }

  //getall regencies
  getRegencies() {
    this.dataService.get('TRegency/getall').subscribe((data: any) => {
      this.regParents = data;
    }, err => {

    });
  }

  //getAllRegencies by paging
  getTRegencieListPaging() {
    this.preventAbuse = true;
    this.dataService.get('TRegency/getlistpaging?page=' + this.page + "&pageSize=" + this.pageSize + "&keyword=" + this.keyword).subscribe((data: any) => {
      this.dataSource = new MatTableDataSource(data.items);
      this.dataSource.sort = this.sort;
      this.totalRow = data.totalCount;
    }, err => {
      this.translateService.get('messageSystem.loadFail').subscribe(data => MessageConstants.GET_FAILSE_MSG = data);
      this.notificationService.printErrorMessage(MessageConstants.GET_FAILSE_MSG);
    });
    this.preventAbuse = false;
  }

  //search
  applyFilter(event: Event) {
    //const filterValue = (event.target as HTMLInputElement).value;
    this.keyword = (event.target as HTMLInputElement).value;
    this.page = 0;
    this.getTRegencieListPaging();
    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

   //select all record
   isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.data.length;
    return numSelected === numRows;
  }

  //toggle select
  masterToggle() {
    if (this.isAllSelected()) {
      this.selection.clear();
      return;
    }
    this.selection.select(...this.dataSource.data);
  }

  checkboxLabel(row?: any): string {
    if (!row) {
      return `${this.isAllSelected() ? 'deselect' : 'select'} all`;
    }
    return `${this.selection.isSelected(row) ? 'deselect' : 'select'} row ${row.position + 1}`;
  }

  get getValidForm(): { [key: string]: AbstractControl } {
    return this.regencyForm.controls;
   }

  //get regParentId theo regId
  getParentId(event:any){
    this.regencyForm.controls['regParentId'].setValue(event.regId);
  }

  //Save change data
  addData(){
    if(this.regencyForm.invalid){
      return;
    }
    this.preventAbuse=true;
    if(this.action=='create'){
      this.spinner.show();
      let regency={regName:this.regencyForm.controls['regName'].value,regDescription:this.regencyForm.controls['regDescription'].value,regStatus:this.regencyForm.controls['regStatus'].value};
      this.dataService.post('TRegency/create',regency).subscribe(data=>{
        let message!:string;
        this.translateService.get('messageSystem.createSuccess').subscribe(mes=>message=mes);
        this.notificationService.printSuccessMessage(message);
        this.getTRegencieListPaging();
        this.getRegencies();
        this.onReset();
        this.spinner.hide();
      },err=>{
        let error!:string;
        this.translateService.get('messageSystem.createFail').subscribe(mes=>error=mes);
        this.notificationService.printErrorMessage(err);
        this.spinner.hide();
      });
    }
    else if(this.action=='edit'){
      this.spinner.show();
      let regency={regName:this.regencyForm.controls['regName'].value,regId:this.regencyForm.controls['regId'].value,regDescription:this.regencyForm.controls['regDescription'].value,regStatus:this.regencyForm.controls['regStatus'].value};
      this.dataService.put('TRegency/update' , regency).subscribe(data=>{
        let message!:string;
        this.translateService.get('messageSystem.updateSuccess').subscribe(mes=>message=mes);
        this.notificationService.printSuccessMessage(message);
        this.getTRegencieListPaging();
        this.getRegencies();
        this.dialog.closeAll();
        this.onReset();
        this.spinner.hide();
      },err=>{
        let error!:string;
        this.translateService.get('messageSystem.updateFail').subscribe(mes=>error=mes);
        this.notificationService.printErrorMessage(err);
        this.spinner.hide();
      })
    }
    this.onReset();
    this.preventAbuse=false;
  }

  //remove muilti data
  removeData(){
    let regencyChecked: any[]=[];
    this.selection.selected.forEach((value:any)=>{
      let id= value.regId;
      regencyChecked.push(id);
    });
    this.translateService.get('messageSystem.confirmDelete').subscribe(data=>MessageConstants.CONFIRM_DELETE_MSG=data);
    this.notificationService.printConfirmationDialog(MessageConstants.CONFIRM_DELETE_MSG, () => this.deleteItemConfirm(JSON.stringify(regencyChecked)));
  }

   deleteItemConfirm(regId: string) {
    this.preventAbuse=true;
    this.spinner.show();
    this.dataService.delete('TRegency/deletemulti', 'checkedList', regId).subscribe((response: any )=> {
      this.translateService.get('messageSystem.deleteSuccess').subscribe(data=>MessageConstants.DELETED_OK_MSG=data);
      if(response[0]>0)
      {
        this.notificationService.printSuccessMessage(MessageConstants.DELETED_OK_MSG + response[0])
      }
      if(response[1]>0)
      {
        this.notificationService.printErrorMessage(MessageConstants.DELETE_FAILSE_MSG + response[1])
      }
      this.selection.clear();
      this.getTRegencieListPaging();
      this.getRegencies();
      this.spinner.hide();
    }, err => {
      this.translateService.get('messageSystem.deletefail').subscribe(data=>MessageConstants.DELETE_FAILSE_MSG=data);
      this.notificationService.printErrorMessage(MessageConstants.DELETE_FAILSE_MSG);
      this.spinner.hide();
    });
    this.preventAbuse=false;
  }


   ngAfterViewInit(){
    this.paginator._intl.itemsPerPageLabel= this.pagin.setLable;
    this.paginator._intl.firstPageLabel= this.pagin.firstButton;
    this.paginator._intl.nextPageLabel= this.pagin.nextButton;
    this.paginator._intl.lastPageLabel= this.pagin.lastButton;
    this.paginator._intl.previousPageLabel= this.pagin.preButton;
  }

  //change paging
  onChangePage(pe:PageEvent) {
    this.page=pe.pageIndex;
    this.pageSize=pe.pageSize;
    this.getTRegencieListPaging();
  }
}
