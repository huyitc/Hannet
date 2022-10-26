import { PaginatorCustomService } from '../../../core/services/paginator-custom.service';
import { MatOptionSelectionChange } from '@angular/material/core';
import { startWith, map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { PageEvent, MatPaginator } from '@angular/material/paginator';
import { MessageConstants } from 'src/app/core/common/message.constants';
import { DataService } from 'src/app/core/services/data.service';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { TranslateService } from '@ngx-translate/core';
import { NotificationService } from 'src/app/core/services/notification.service';
import { FormGroup, FormBuilder, Validators, AbstractControl, FormControl, ValidatorFn } from '@angular/forms';
import { SelectionModel } from '@angular/cdk/collections';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-t-device',
  templateUrl: './t-device.component.html',
  styleUrls: ['./t-device.component.css']
})
export class TDeviceComponent implements OnInit {
  devLaneChecks: any;
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  displayedColumns: string[] = ['select', 'position', 'devName', 'devIp', 'devPort', 'devMacaddress', 'devSerialnumber', 'devPartnumber', 'devTypeName', 'zonName', 'devTimeCheck', 'devStatus', 'action'];
  @ViewChild(MatSort) sort!: MatSort;
  dataSource = new MatTableDataSource<any>();
  preventAbuse = false;
  page = 0;
  keyword: string = '';
  totalRow: number = 0;
  totalPage: number = 0;
  pageSizeOptions: number[] = [10, 25, 50, 100];
  pageSize = this.pageSizeOptions[0];
  selection = new SelectionModel<any>(true, []);
  action!: string;
  tDeviceForm!: FormGroup;
  title!: string;
  aZones: any;
  optionsAZones!: Observable<any[]>;
  aZoneControl = new FormControl('',
    { validators: [this.autocompleteObjectValidator(), Validators.required] });
  tDeviceTypes: any;
  optionsTDeviceTypes!: Observable<any[]>;
  tDeviceTypeControl = new FormControl('',
    { validators: [this.autocompleteObjectValidator(), Validators.required] });
  @ViewChild('dialog') dialogTemplate!: TemplateRef<any>;
  constructor(private spinner: NgxSpinnerService,private pagin: PaginatorCustomService, private notificationService: NotificationService, private translateService: TranslateService,
    private dataService: DataService, private dialog: MatDialog, private formBuilder: FormBuilder) {
    this.tDeviceForm = this.formBuilder.group({
      devId: 0,
      devName: ['', Validators.compose([Validators.required])],
      devIp: ['', Validators.compose([Validators.required])],
      devPort: '',
      devCode: '',
      devSerialnumber: '',
      devPartnumber: '',
      devMacaddress: '',
      devTimeCheck: true,
      devLaneCheck: [0, Validators.compose([Validators.required])],
      devStatus: true,
      zonId: 0,
      devTypeId: 0
    });
  }
  ngOnInit(): void {
    this.loadTDeviceListPaging();
    this.loadAllAZones();
    this.loadAllTDeviceTypes();
    this.translateService.get('messageSystem.loadFail').subscribe(data => MessageConstants.GET_FAILSE_MSG = data)
    let devLaneCheckNotDistinction;
    let devLaneCheckIn;
    let devLaneCheckOut;
    this.translateService.get('tDeviceModule.devLaneCheckNotDistinction').subscribe(data => devLaneCheckNotDistinction = data);
    this.translateService.get('tDeviceModule.devLaneCheckIn').subscribe(data => devLaneCheckIn = data);
    this.translateService.get('tDeviceModule.devLaneCheckOut').subscribe(data => devLaneCheckOut = data);
    this.devLaneChecks = [
      {
        name: devLaneCheckNotDistinction,
        value: 0
      },
      {
        name: devLaneCheckIn,
        value: 1
      },
      {
        name: devLaneCheckOut,
        value: 2
      }];
  }
  ngAfterViewInit() {
    this.paginator._intl.itemsPerPageLabel = this.pagin.setLable;
    this.paginator._intl.firstPageLabel = this.pagin.firstButton;
    this.paginator._intl.nextPageLabel = this.pagin.nextButton;
    this.paginator._intl.lastPageLabel = this.pagin.lastButton;
    this.paginator._intl.previousPageLabel = this.pagin.preButton;
  }
  onchangeAZone(event: MatOptionSelectionChange, item: any) {
    if (event.source.selected) {
      this.tDeviceForm.controls['zonId'].setValue(item.zonId);
    }
  }
  onchangeTDevType(event: MatOptionSelectionChange, item: any) {
    if (event.source.selected) {
      this.tDeviceForm.controls['devTypeId'].setValue(item.devTypeId);
    }
  }
  private _filterAZones(value: any): any[] {
    const filterValue = value?.toLowerCase();
    return this.aZones.filter((option: any) => option?.zonName.toLowerCase().includes(filterValue));
  }
  private _filterTDeviceTypes(value: any): any[] {
    const filterValue = value?.toLowerCase();
    return this.tDeviceTypes.filter((option: any) => option?.devTypeName.toLowerCase().includes(filterValue));
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
  loadTDeviceListPaging() {
    this.preventAbuse = true;
    this.dataService.get('tdevice/getlistpaging?page=' + this.page + "&pageSize=" + this.pageSize + "&keyword=" + this.keyword).subscribe((data: any) => {
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
  applyFilter(event: Event) {
    this.keyword = (event.target as HTMLInputElement).value;
    this.page = 0;
    this.loadTDeviceListPaging();
    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }
  autocompleteObjectValidator(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
      if (typeof control.value === 'string') {
        return { 'invalidAutocompleteObject': { value: control.value } }
      }
      return null  /* valid option selected */
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
  loadAllTDeviceTypes() {
    this.preventAbuse = true;
    this.dataService.get('tdevicetype/getall').subscribe((data: any) => {
      this.tDeviceTypes = data;
      this.preventAbuse = false;
      this.optionsTDeviceTypes = this.tDeviceTypeControl.valueChanges.pipe(
        startWith(''),
        map((value: any) => (value == null ? value : typeof value === 'string' ? value : value.devTypeName)),
        map(name => (name ? this._filterTDeviceTypes(name) : this.tDeviceTypes.slice())),
      );
    }, (err: any) => {
      this.translateService.get('messageSystem.loadFail').subscribe(data => MessageConstants.GET_FAILSE_MSG = data);
      this.notificationService.printErrorMessage(MessageConstants.GET_FAILSE_MSG);
      this.preventAbuse = false;
    });
  }
  displayFnTdevType(devType: any): string {
    return devType && devType.devTypeName ? devType.devTypeName : '';
  }
  onChangePage(pe: PageEvent) {
    this.page = pe.pageIndex;
    this.pageSize = pe.pageSize;
    this.loadTDeviceListPaging();
  }
  removeData() {
    let roleChecked: any[] = [];
    this.selection.selected.forEach((value: any) => {
      let id = value.devId;
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
      this.loadTDeviceListPaging();
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
        this.tDeviceForm.controls['devId'].setValue(item.devId);
        this.tDeviceForm.controls['devTypeId'].setValue(item.devTypeId);
        this.tDeviceForm.controls['zonId'].setValue(item.zonId);
        this.tDeviceForm.controls['devName'].setValue(item.devName);
        this.tDeviceForm.controls['devIp'].setValue(item.devIp);
        this.tDeviceForm.controls['devPort'].setValue(item.devPort);
        this.tDeviceForm.controls['devCode'].setValue(item.devCode);
        this.tDeviceForm.controls['devSerialnumber'].setValue(item.devSerialnumber);
        this.tDeviceForm.controls['devPartnumber'].setValue(item.devPartnumber);
        this.tDeviceForm.controls['devTimeCheck'].setValue(item.devTimeCheck);
        this.tDeviceForm.controls['devMacaddress'].setValue(item.devMacaddress);
        this.tDeviceForm.controls['devStatus'].setValue(item.devStatus);
        this.tDeviceForm.controls['devLaneCheck'].setValue(item.devLaneCheck);
        let aZoneDb = this.aZones.filter((x: any) => x.zonId == item.zonId)[0];
        this.aZoneControl.setValue(aZoneDb);
        let tDevTypeDb = this.tDeviceTypes.filter((x: any) => x.devTypeId == item.devTypeId)[0];
        this.tDeviceTypeControl.setValue(tDevTypeDb);
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
    this.resetForm();
    this.aZoneControl.reset();
    this.tDeviceTypeControl.reset();
  }
  resetForm(){
    this.tDeviceForm = this.formBuilder.group({
      devId: 0,
      devName: ['', Validators.compose([Validators.required])],
      devIp: ['', Validators.compose([Validators.required])],
      devPort: '',
      devCode: '',
      devSerialnumber: '',
      devPartnumber: '',
      devMacaddress: '',
      devTimeCheck: true,
      devLaneCheck: [0, Validators.compose([Validators.required])],
      devStatus: true,
      zonId: 0,
      devTypeId: 0
    });
  }
  get getValidForm(): { [key: string]: AbstractControl } {
    return this.tDeviceForm.controls;
  }
  addData() {
    if (this.tDeviceForm.invalid) {
      return;
    }
    if (this.aZoneControl.invalid) {
      return;
    }
    if (this.tDeviceTypeControl.invalid) {
      return;
    }
    this.preventAbuse = true;
    if (this.action == 'create') {
      this.spinner.show();
      let tDevice = {
        devName: this.tDeviceForm.controls['devName'].value,
        devIp: this.tDeviceForm.controls['devIp'].value,
        devPort: this.tDeviceForm.controls['devPort'].value,
        devCode: this.tDeviceForm.controls['devCode'].value,
        devSerialnumber: this.tDeviceForm.controls['devSerialnumber'].value,
        devPartnumber: this.tDeviceForm.controls['devPartnumber'].value,
        devMacaddress: this.tDeviceForm.controls['devMacaddress'].value,
        devTimeCheck: this.tDeviceForm.controls['devTimeCheck'].value,
        devLaneCheck: this.tDeviceForm.controls['devLaneCheck'].value,
        devStatus: this.tDeviceForm.controls['devStatus'].value,
        zonId: this.tDeviceForm.controls['zonId'].value,
        devTypeId: this.tDeviceForm.controls['devTypeId'].value
      };
      this.dataService.post('tdevice/create', tDevice).subscribe((data: any) => {
        let message!: string;
        this.translateService.get('messageSystem.createSuccess').subscribe(mes => message = mes);
        this.notificationService.printSuccessMessage(message);
        this.loadTDeviceListPaging();
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
      let tDevice = this.tDeviceForm.value;
      this.dataService.put('tdevice/update', tDevice).subscribe((data: any) => {
        let message!: string;
        this.translateService.get('messageSystem.updateSuccess').subscribe(mes => message = mes);
        this.notificationService.printSuccessMessage(message);
        this.loadTDeviceListPaging();
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
