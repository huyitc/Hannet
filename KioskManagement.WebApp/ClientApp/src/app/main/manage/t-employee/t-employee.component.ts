import { NgxSpinnerService } from 'ngx-spinner';
import { DeviceTypeService } from './../../../core/services/device-type.service';
import { Component, OnInit, TemplateRef, ViewChild,} from '@angular/core';
import {AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { MatDialog, MatDialogConfig,} from '@angular/material/dialog';
import { TranslateService } from '@ngx-translate/core';
import { DataService } from 'src/app/core/services/data.service';
import { NotificationService } from 'src/app/core/services/notification.service';
import { PaginatorCustomService } from 'src/app/core/services/paginator-custom.service';
import { DatePipe } from '@angular/common';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { SelectionModel } from '@angular/cdk/collections';
import { Observable } from 'rxjs';
import { startWith, map } from 'rxjs/operators';
import { MessageConstants } from 'src/app/core/common/message.constants';
import { MatOptionSelectionChange } from '@angular/material/core';

@Component({
  selector: 'app-t-employee',
  templateUrl: './t-employee.component.html',
  styleUrls: ['./t-employee.component.css']
})
export class TEmployeeComponent implements OnInit {
  displayedColumns: string[] = ['select', 'emName','emCode','emGender','emBirthday','emIdentity','placeId','zoneName','emType','descripstion','emImage','status'];
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  @ViewChild('dialog') dialogTemplate!: TemplateRef<any>;
  dataSource = new MatTableDataSource<any>();
  preventAbuse = false;
  page = 0;
  keyword: string = '';
  totalRow: number = 0;
  totalPage: number = 0;
  aZones: any;
  optionsAZones!: Observable<any[]>;
  aZoneControl = new FormControl('',
    { validators: [this.autocompleteObjectValidator(), Validators.required] });
  emtype:any;
  pageSizeOptions: number[] = [10, 25, 50, 100];
  pageSize = this.pageSizeOptions[0];
  selection = new SelectionModel<any>(true, []);
  action!: string;
  employeeForm!: FormGroup;
  title!: string;
  constructor(private spinner: NgxSpinnerService, private dataService: DataService, public dialog: MatDialog,
    private notificationService: NotificationService, private pagin: PaginatorCustomService,
    private translateService: TranslateService, private formBuilder: FormBuilder, private datepipe: DatePipe, private _deviceTypeService: DeviceTypeService) {
      this.employeeForm = this.formBuilder.group({
        emId: 0,
        emTypeId: ['', Validators.compose([Validators.required])],
        regId: ['', Validators.compose([Validators.required])],
        depId: ['', Validators.compose([Validators.required])],
        emCode:'',
        emName:'',
        emGender:'',
        emBirthdate:'',
        emIdentityNumber:'',
        description:'',
        emPhone:'',
        emEmail:'',
        emImage:'',
        emStatus:'',
        placeId:['', Validators.compose([Validators.required])],
        zoneId:['', Validators.compose([Validators.required])],
      });
  }

  ngOnInit(): void {
    this.loadEmployeeAllPaging();
    this.loadAllAZones();
    this.loadEmTypeId();
  }
 
  ngAfterViewInit() {
    this.paginator._intl.itemsPerPageLabel = this.pagin.setLable;
    this.paginator._intl.firstPageLabel = this.pagin.firstButton;
    this.paginator._intl.nextPageLabel = this.pagin.nextButton;
    this.paginator._intl.lastPageLabel = this.pagin.lastButton;
    this.paginator._intl.previousPageLabel = this.pagin.preButton;
  }
  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.data.length;
    return numSelected === numRows;
  }
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

  autocompleteObjectValidator(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
      if (typeof control.value === 'string') {
        return { 'invalidAutocompleteObject': { value: control.value } }
      }
      return null  /* valid option selected */
    }
  }

  applyFilter(event: Event) {
    this.keyword = (event.target as HTMLInputElement).value;
    this.page = 0;
    this.loadEmployeeAllPaging();
    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  loadAllAZones() {
    this.preventAbuse = true;
    this.dataService.get('azone/getall').subscribe((data: any) => {
      this.aZones = data;
      this.preventAbuse = false;
      this.optionsAZones = this.aZoneControl.valueChanges.pipe(
        startWith(''),
        map((value: any) => (value == null ? value : typeof value === 'string' ? value : value.zonName)),
        map(name => (name ? this._filterAZones(name) : this.aZones.slice())),
      );
    }, (err: any) => {
      this.translateService.get('messageSystem.loadFail').subscribe(data => MessageConstants.GET_FAILSE_MSG = data);
      this.notificationService.printErrorMessage(MessageConstants.GET_FAILSE_MSG);
      this.preventAbuse = false;
    });
  }

  displayFnAZone(zone: any): string {
    return zone && zone.zonName ? zone.zonName : '';
  }

  private _filterAZones(value: any): any[] {
    const filterValue = value?.toLowerCase();
    return this.aZones.filter((option: any) => option?.zonName.toLowerCase().includes(filterValue));
  }

  onchangeAZone(event: MatOptionSelectionChange, item: any) {
    if (event.source.selected) {
      this.employeeForm.controls['zonId'].setValue(item.zonId);
    }
  }

  loadEmTypeId(){
    this.preventAbuse = true;
    this.dataService.get('TEmployeeType/getall').subscribe((data: any) => {
      this.emtype = data;
    }, (err: any) => {
      this.translateService.get('messageSystem.loadFail').subscribe(data => MessageConstants.GET_FAILSE_MSG = data);
      this.notificationService.printErrorMessage(MessageConstants.GET_FAILSE_MSG);
      this.preventAbuse = false;
    });
  }

  loadEmployeeAllPaging() {
    this.preventAbuse = true;
    this.dataService.get('TEmployee/getAllByPaging?page=' + this.page + "&pageSize=" + this.pageSize + "&keyword=" + this.keyword).subscribe((data: any) => {
      this.dataSource = new MatTableDataSource(data.items);
      this.dataSource.sort = this.sort;
      this.totalRow = data.totalCount;
      this.preventAbuse = false;
    }, (err: any) => {
      this.translateService.get('messageSystem.loadFail').subscribe(data => MessageConstants.GET_FAILSE_MSG = data);
      this.notificationService.printErrorMessage(MessageConstants.GET_FAILSE_MSG);
      this.preventAbuse = false;
    });
  }

  onChangePage(pe: PageEvent) {
    this.page = pe.pageIndex;
    this.pageSize = pe.pageSize;
    this.loadEmployeeAllPaging();
  }
  removeData() {
    let roleChecked: any[] = [];
    this.selection.selected.forEach((value: any) => {
      let id = value.emId;
      roleChecked.push(id);
    });
    this.translateService.get('messageSystem.confirmDelete').subscribe(data => MessageConstants.CONFIRM_DELETE_MSG = data);
    this.notificationService.printConfirmationDialog(MessageConstants.CONFIRM_DELETE_MSG, () => this.deleteItemConfirm(JSON.stringify(roleChecked)));
  }

  deleteItemConfirm(id: string) {
    this.preventAbuse = true;
    this.spinner.show();
    this.dataService.delete('tdevice/deletemulti', 'checkedList', id).subscribe((res: any) => {
      this.translateService.get('messageSystem.deleteSuccess').subscribe(data => MessageConstants.DELETED_OK_MSG = data);
      this.notificationService.printSuccessMessage(MessageConstants.DELETED_OK_MSG);
      this.selection.clear();
      this.loadEmployeeAllPaging();
      this.spinner.hide();
    }, (err: any) => {
      this.preventAbuse = false;
      this.translateService.get('messageSystem.deletefail').subscribe(data => MessageConstants.DELETE_FAILSE_MSG = data);
      this.notificationService.printErrorMessage(MessageConstants.DELETE_FAILSE_MSG);
      this.spinner.hide();
    });
  }

  openDialog(action: string, item?: any, config?: MatDialogConfig) {
    this.action = action;
    if (action == 'create') {
      this.translateService.get('tDeviceModule.createMessage').subscribe(data => this.title = data);
    }
    else {
      this.preventAbuse = true;
      this.dataService.get('tdevice/getbyid/' + item.devId).subscribe((data: any) => {
        let item = data;
        this.employeeForm.controls['emId'].setValue(item.devId);
        this.employeeForm.controls['emTypeId'].setValue(item.devTypeId);
        this.employeeForm.controls['regId'].setValue(item.zonId);
        this.employeeForm.controls['depId'].setValue(item.devName);
        this.employeeForm.controls['emCode'].setValue(item.devIp);
        this.employeeForm.controls['emName'].setValue(item.devPort);
        this.employeeForm.controls['emGender'].setValue(item.devCode);
        this.employeeForm.controls['emBirthdate'].setValue(item.devSerialnumber);
        this.employeeForm.controls['emIdentityNumber'].setValue(item.devPartnumber);
        this.employeeForm.controls['description'].setValue(item.devTimeCheck);
        this.employeeForm.controls['emPhone'].setValue(item.devMacaddress);
        this.employeeForm.controls['emEmail'].setValue(item.devStatus);
        this.employeeForm.controls['emImage'].setValue(item.devLaneCheck);
        this.employeeForm.controls['emStatus'].setValue(item.deviceId);
        this.employeeForm.controls['placeId'].setValue(item.deviceId);
        this.employeeForm.controls['zoneId'].setValue(item.deviceId);
        let aZoneDb = this.aZones.filter((x: any) => x.zonId == item.zonId)[0];
        this.aZoneControl.setValue(aZoneDb);
        this.preventAbuse = false;
      }, (err: any) => {
        this.translateService.get('messageSystem.loadFail').subscribe(data => MessageConstants.GET_FAILSE_MSG = data);
        this.notificationService.printErrorMessage(MessageConstants.GET_FAILSE_MSG);
        this.preventAbuse = false;
      });

    }
    const dialogRef = this.dialog.open(this.dialogTemplate, {
      width: '750px', autoFocus: false
    });
    dialogRef.disableClose = true;
    dialogRef.afterClosed().subscribe(result => {
      this.onReset();
    });
  }

  onReset() {
    this.action == '';
    this.dialog.closeAll();
    this.employeeForm.reset();
    this.aZoneControl.reset();
  }

  get getValidForm(): { [key: string]: AbstractControl } {
    return this.employeeForm.controls;
  }

  addData() {
    if (this.employeeForm.invalid) {
      return;
    }
    if (this.aZoneControl.invalid) {
      return;
    }
    this.preventAbuse = true;
    if (this.action == 'create') {
      this.spinner.show();
      let tEmployee = {
        emId: this.employeeForm.controls['emId'].value,
        emTypeId: this.employeeForm.controls['emTypeId'].value,
        regId: this.employeeForm.controls['regId'].value,
        depId: this.employeeForm.controls['depId'].value,
        emCode: this.employeeForm.controls['emCode'].value,
        emName: this.employeeForm.controls['emName'].value,
        emGender: this.employeeForm.controls['emGender'].value,
        emBirthdate: this.employeeForm.controls['emBirthdate'].value,
        emIdentityNumber: this.employeeForm.controls['emIdentityNumber'].value,
        description: this.employeeForm.controls['description'].value,
        emPhone: this.employeeForm.controls['emPhone'].value,
        emEmail: this.employeeForm.controls['emEmail'].value,
        emImage: this.employeeForm.controls['emImage'].value,
        emStatus: this.employeeForm.controls['emStatus'].value,
        placeId: this.employeeForm.controls['placeId'].value,
        zonId: this.employeeForm.controls['zonId'].value
      };
      this.dataService.post('TEmployee/create', tEmployee).subscribe((data: any) => {
        let message!: string;
        this.translateService.get('messageSystem.createSuccess').subscribe(mes => message = mes);
        this.notificationService.printSuccessMessage(message);
        this.loadAllAZones();
        this.spinner.hide();
      }, (err: any) => {
        this.preventAbuse = false;
        let error!: string;
        this.translateService.get('messageSystem.createFail').subscribe(mes => error = mes);
        this.notificationService.printErrorMessage(error);
        this.spinner.hide();
      });
    } else if (this.action == 'edit') {
      this.spinner.show();
      let tEmployee = this.employeeForm.value;
      this.dataService.put('TEmployee/update', tEmployee).subscribe((data: any) => {
        let message!: string;
        this.translateService.get('messageSystem.updateSuccess').subscribe(mes => message = mes);
        this.notificationService.printSuccessMessage(message);
        this.loadAllAZones();
        this.spinner.hide();
      }, (err: any) => {
        this.preventAbuse = false;
        let error!: string;
        this.translateService.get('messageSystem.updateFail').subscribe(mes => error = mes);
        this.notificationService.printErrorMessage(error);
        this.spinner.hide();
      });
    }

  }
}
