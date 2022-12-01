import { SelectionModel } from '@angular/cdk/collections';
import { DatePipe } from '@angular/common';
import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { TranslateService } from '@ngx-translate/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { MessageConstants } from 'src/app/core/common/message.constants';
import { DataService } from 'src/app/core/services/data.service';
import { DeviceTypeService } from 'src/app/core/services/device-type.service';
import { NotificationService } from 'src/app/core/services/notification.service';
import { PaginatorCustomService } from 'src/app/core/services/paginator-custom.service';

@Component({
  selector: 'app-sync',
  templateUrl: './sync.component.html',
  styleUrls: ['./sync.component.css']
})
export class SyncComponent implements OnInit {
  @ViewChild(MatSort) sort!: MatSort;
  dataSource = new MatTableDataSource<any>();
  dataSourceEmpNotInDevice = new MatTableDataSource<any>();
  dataSourceEmpInDevice = new MatTableDataSource<any>();
  preventAbuse = false;
  selection = new SelectionModel<any>(true, []);
  selection1 = new SelectionModel<any>(true, []);
  selection2 = new SelectionModel<any>(true, []);
  displayedColumns: string[] = ['select', 'position', 'emName', 'emCode', 'depName', 'regName', 'gaName'];
  displayedColumnsEmpNotInDevice: string[] = ['select', 'position', 'EmName', 'EmCode', 'DepName', 'RegName'];
  displayedColumnsEmpInDevice: string[] = ['select', 'position', 'EmName', 'EmCode', 'DepName', 'RegName'];
  keyword: string = '';
  deviceTypeConfig!: string;
  devId: number = 0;
  device: any;
  connectedSingleDev: boolean = false;
  buttonConnectlabel: string = '';
  buttonDisconnectlabel: string = '';
  keyword1:string=''
  keyword2:string=''
  constructor(private spinner: NgxSpinnerService,private dataService: DataService, public dialog: MatDialog,
    private notificationService: NotificationService, private pagin: PaginatorCustomService,
    private translateService: TranslateService, private _deviceTypeService: DeviceTypeService) 
    {

     }
 public datePipe = new DatePipe('en-US');

  ngOnInit(): void {
    this.deviceTypeConfig = this._deviceTypeService.getDeviceType();
    this.translateService.get('syncModule.btnConnect').subscribe(data => this.buttonConnectlabel = data);
    this.translateService.get('syncModule.btnDisconnect').subscribe(data => this.buttonDisconnectlabel = data);
    this.loadEmployeeEditStatus();
    this.getDevice();
  }

  loadEmployeeEditStatus() {
    this.selection.clear();
    this.preventAbuse = true;
    this.dataService.get('Sync/GetEmployeeEditStatus?keyword=' + this.keyword).subscribe((data: any) => {
      this.dataSource = new MatTableDataSource(data);
      this.dataSource.sort = this.sort;
      this.preventAbuse = false;
    }, (err: any) => {
      this.translateService.get('messageSystem.loadFail').subscribe(data => MessageConstants.GET_FAILSE_MSG = data);
      this.notificationService.printErrorMessage(MessageConstants.GET_FAILSE_MSG);
      this.preventAbuse = false;
    });
  }

  getDevice(){
    this.preventAbuse = true;
    if (this.deviceTypeConfig == 'idemiadevice') {
      this.dataService.get('TDevice/getdevicemorpho').subscribe((data: any) => {
        this.device = data;
        this.preventAbuse = false;
      }, (err: any) => {
        this.translateService.get('messageSystem.loadFail').subscribe(data => MessageConstants.GET_FAILSE_MSG = data);
        this.notificationService.printErrorMessage(MessageConstants.GET_FAILSE_MSG);
        this.preventAbuse = false;
      });
    } else if (this.deviceTypeConfig == 'unvdevice') {
      this.dataService.get('TDevice/getdeviceunv').subscribe((data: any) => {
        this.device = data;
        this.preventAbuse = false;
      }, (err: any) => {
        this.translateService.get('messageSystem.loadFail').subscribe(data => MessageConstants.GET_FAILSE_MSG = data);
        this.notificationService.printErrorMessage(MessageConstants.GET_FAILSE_MSG);
        this.preventAbuse = false;
      });
    } else if(this.deviceTypeConfig == 'handevice'){
      this.dataService.get('TDevice/getdevicehan').subscribe((data: any) => {
        this.device = data;
        this.preventAbuse = false;
      }, (err: any) => {
        this.translateService.get('messageSystem.loadFail').subscribe(data => MessageConstants.GET_FAILSE_MSG = data);
        this.notificationService.printErrorMessage(MessageConstants.GET_FAILSE_MSG);
        this.preventAbuse = false;
      });
    }
  }

  // syncAll() {
  //   let message: string = '';
  //   this.translateService.get('messageSystem.confirmSyncAll').subscribe(data => message = data);
  //   this.notificationService.printConfirmationDialog(message, () => this.confirmSyncAll());
  // }

  // confirmSyncAll() {
  //   this.spinner.show();
  //   let now = this.datePipe.transform(new Date(), 'yyyyMMddHHmmss');
  //   let fileName = "SyncAll" + now + ".log"; // khong được sửa filename - liên quan đến cắt chuỗi
  //   this.preventAbuse = true;
  //   this.dataService.postDeviceReturnFile(this.deviceTypeConfig + '/syncall?fileName=' + fileName, { observe: 'response', responseType: 'blob' }).subscribe((res: any) => {
  //     let blod: Blob = res.body as Blob;
  //     let a = document.createElement('a');
  //     a.download = fileName;
  //     a.href = window.URL.createObjectURL(blod);
  //     a.click();
  //     this.selection.clear();
  //     this.loadEmployeeEditStatus();
  //     this.spinner.hide();
  //   }, (err: any) => {
  //     this.preventAbuse = false;
  //     this.notificationService.printErrorMessage(err.message);
  //     this.spinner.hide();
  //   });
  // }

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

  isAllSelected1() {
    const numSelected = this.selection1.selected.length;
    const numRows = this.dataSourceEmpNotInDevice.data.length;
    return numSelected === numRows;
  }
  masterToggle1() {
    if (this.isAllSelected1()) {
      this.selection1.clear();
      return;
    }
    this.selection1.select(...this.dataSourceEmpNotInDevice.data);
  }
  checkboxLabel1(row?: any): string {
    if (!row) {
      return `${this.isAllSelected1() ? 'deselect' : 'select'} all`;
    }
    return `${this.selection1.isSelected(row) ? 'deselect' : 'select'} row ${row.position + 1}`;
  }

  isAllSelected2() {
    const numSelected = this.selection2.selected.length;
    const numRows = this.dataSourceEmpInDevice.data.length;
    return numSelected === numRows;
  }
  masterToggle2() {
    if (this.isAllSelected2()) {
      this.selection2.clear();
      return;
    }
    this.selection2.select(...this.dataSourceEmpInDevice.data);
  }
  checkboxLabel2(row?: any): string {
    if (!row) {
      return `${this.isAllSelected2() ? 'deselect' : 'select'} all`;
    }
    return `${this.selection2.isSelected(row) ? 'deselect' : 'select'} row ${row.position + 1}`;
  }

  applyFilter(event: Event) {
    this.keyword = (event.target as HTMLInputElement).value;
    this.loadEmployeeEditStatus();
  }
  applyFilter1(event: Event) {
    let emName = (event.target as HTMLInputElement).value;
    this.dataSourceEmpNotInDevice.filter = emName.trim().toLowerCase();
    this.selection1.clear();
  }
  applyFilter2(event: Event) {
    let emName = (event.target as HTMLInputElement).value;
    this.dataSourceEmpInDevice.filter = emName.trim().toLowerCase();
    this.selection2.clear();
  }

  // additionalSync() {
  //   this.spinner.show();
  //   let emIdList: any[] = [];
  //   this.selection.selected.forEach((value: any) => {
  //     let id = value.emId;
  //     emIdList.push(id);
  //   });
  //   if (emIdList.length == 0) {
  //     return;
  //   }
  //   let stringEmIdList = JSON.stringify(emIdList);
  //   let now = this.datePipe.transform(new Date(), 'yyyyMMddHHmmss');
  //   let fileName = "Additional_Sync" + now + ".log"; // khong được sửa filename - liên quan đến cắt chuỗi
  //   this.preventAbuse = true;
  //   this.dataService.postDeviceReturnFile(this.deviceTypeConfig + '/additionalsync?strListEmId=' + stringEmIdList + '&fileName=' + fileName, { observe: 'response', responseType: 'blob' }).subscribe((res: any) => {
  //     let blod: Blob = res.body as Blob;
  //     let a = document.createElement('a');
  //     a.download = fileName;
  //     a.href = window.URL.createObjectURL(blod);
  //     a.click();
  //     this.selection.clear();
  //     this.loadEmployeeEditStatus();
  //     this.spinner.hide();
  //   }, (err: any) => {
  //     this.preventAbuse = false;
  //     this.spinner.hide();
  //     this.notificationService.printErrorMessage(err.message);
  //   });
  // }

  @ViewChild('dialog') dialogTemplate!: TemplateRef<any>;
  openDialog(config?: MatDialogConfig) {
    this.getDevice();
    const dialogRef = this.dialog.open(this.dialogTemplate, {
      width: '1480px', autoFocus: false
    });
    dialogRef.disableClose = true;
    dialogRef.afterClosed().subscribe(result => {
      this.devId = 0;
      this.connectedSingleDev = false;
      this.selection1.clear();
      this.selection2.clear();
      this.keyword1='';
      this.keyword2='';
    });
  }

  // setConnection() {
  //   if (this.devId == 0) {
  //     return;
  //   }
  //   if (!this.connectedSingleDev) {
  //     this.getEmployeeInDevice();
  //     this.getEmployeeNotInDevice();
  //   } else {
  //     this.connectedSingleDev = false;
  //     this.selection1.clear();
  //     this.selection2.clear();
  //     this.keyword1='';
  //     this.keyword2='';
  //     let arrNew: any = [];
  //     this.dataSourceEmpNotInDevice = new MatTableDataSource(arrNew);
  //     this.dataSourceEmpInDevice = new MatTableDataSource(arrNew);
  //   }
  // }

  // getEmployeeInDevice() {
  //   this.spinner.show();
  //   this.selection1.clear();
  //   this.preventAbuse = true;
  //   this.dataService.getDevice(this.deviceTypeConfig + '/getemployeeindevice?devId=' + this.devId).subscribe((res: any) => {
  //     this.connectedSingleDev = res.connected;
  //     if (this.connectedSingleDev) {
  //       this.dataSourceEmpInDevice = new MatTableDataSource(res.employeeListInDev);
  //       this.dataSourceEmpInDevice.sort = this.sort;
  //       this.dataSourceEmpInDevice.filter = this.keyword2.trim().toLowerCase();
  //     }
  //     this.preventAbuse = false;
  //     this.spinner.hide();
  //   }, (err: any) => {
  //     this.translateService.get('messageSystem.loadFail').subscribe(data => MessageConstants.GET_FAILSE_MSG = data);
  //     this.notificationService.printErrorMessage(MessageConstants.GET_FAILSE_MSG);
  //     this.connectedSingleDev = false;
  //     this.preventAbuse = false;
  //     this.spinner.hide();
  //   });
  // }

  // getEmployeeNotInDevice() {
  //   this.spinner.show();
  //   this.selection2.clear();
  //   this.preventAbuse = true;
  //   this.dataService.getDevice(this.deviceTypeConfig + '/getemployeenotindevice?devId=' + this.devId).subscribe((res: any) => {
  //     this.connectedSingleDev = res.connected;
  //     if (this.connectedSingleDev) {
  //       this.dataSourceEmpNotInDevice = new MatTableDataSource(res.employeeListNotInDev);
  //       this.dataSourceEmpNotInDevice.sort = this.sort;
  //       this.dataSourceEmpNotInDevice.filter = this.keyword1.trim().toLowerCase();
  //     }
  //     this.preventAbuse = false;
  //     this.spinner.hide();
  //   }, (err: any) => {
  //     this.translateService.get('messageSystem.loadFail').subscribe(data => MessageConstants.GET_FAILSE_MSG = data);
  //     this.notificationService.printErrorMessage(MessageConstants.GET_FAILSE_MSG);
  //     this.connectedSingleDev = false;
  //     this.preventAbuse = false;
  //     this.spinner.hide();
  //   });
  // }

  // syncToSingleDev() {
  //   let idArr: any[] = [];
  //   this.selection1.selected.forEach((value: any) => {
  //     let id = value.EmId;
  //     idArr.push(id);
  //   });
  //   if (idArr.length == 0) {
  //     return;
  //   }
  //   this.preventAbuse = true;
  //   this.dataService.postDevice(this.deviceTypeConfig + '/syncemployeetodevice?strListEmId=' + JSON.stringify(idArr) + '&devId=' + this.devId).subscribe((res: any) => {
  //     this.connectedSingleDev = res.connected;
  //     if (this.connectedSingleDev) {
  //       this.getEmployeeInDevice();
  //       this.getEmployeeNotInDevice();
  //     }
  //     this.preventAbuse = false;
  //   }, (err: any) => {
  //     this.preventAbuse = false;
  //     this.notificationService.printErrorMessage(err.message);
  //   });
  // }

  // deleteSingleDev() {
  //   let idArr: any[] = [];
  //   this.selection2.selected.forEach((value: any) => {
  //     let id = value.EmId;
  //     idArr.push(id);
  //   });
  //   if (idArr.length == 0) {
  //     return;
  //   }
  //   this.preventAbuse = true;
  //   this.dataService.postDevice(this.deviceTypeConfig + '/deleteemployeeindevice?strListEmId=' + JSON.stringify(idArr) + '&devId=' + this.devId).subscribe((res: any) => {
  //     this.connectedSingleDev = res.connected;
  //     if (this.connectedSingleDev) {
  //       this.getEmployeeInDevice();
  //       this.getEmployeeNotInDevice();
  //     }
  //     this.preventAbuse = false;
  //   }, (err: any) => {
  //     this.preventAbuse = false;
  //     this.notificationService.printErrorMessage(err.message);
  //   });
  // }

}
