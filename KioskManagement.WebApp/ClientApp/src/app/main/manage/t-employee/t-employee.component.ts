import { NgxSpinnerService } from 'ngx-spinner';
import { DeviceTypeService } from './../../../core/services/device-type.service';
import { UrlApiService } from './../../../core/services/url-api.service';
import { MatOptionSelectionChange } from '@angular/material/core';
import { SelectionModel } from '@angular/cdk/collections';
import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { TranslateService } from '@ngx-translate/core';
import { Observable } from 'rxjs';
import { MessageConstants } from 'src/app/core/common/message.constants';
import { DataService } from 'src/app/core/services/data.service';
import { NotificationService } from 'src/app/core/services/notification.service';
import { PaginatorCustomService } from 'src/app/core/services/paginator-custom.service';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-t-employee',
  templateUrl: './t-employee.component.html',
  styleUrls: ['./t-employee.component.css']
})
export class TEmployeeComponent implements OnInit {

  displayedColumns: string[] = ['select', 'position', 'employeeName', 'image', 'department', 'regency', 'employeeBirth', 'employeeGender', 'employeeIdentity',
    'employeePhone', 'employeeAddress', 'groupAccess', 'access', 'employeeStatus', 'action'];
  dataSource = new MatTableDataSource<any>();
  dataSourceCard = new MatTableDataSource<any>();
  displayedColumnCard: string[] = ['position', 'cardNo', 'cardNumber'];
  dataSourceVehicle = new MatTableDataSource<any>();
  displayedColumnVehicle: string[] = ['position', 'cvtName', 'cardNo', 'vehicleNumber', 'vehicleModel', 'vehicleColor', 'ticketType', 'vehicleImage']
  preventAbuse = false;
  page = 0;
  keyword: string = '';
  totalRow: number = 0;
  totalPage: number = 0;
  emNames: any;
  departments: any;
  regencys: any;
  deviceMorpho: any
  deviceUNV: any
  schedevdetails: any
  employeeTypes: any;
  groupAccesss: any;
  emCodes: any;
  name: any;
  department: any;
  regency: any;
  dob: any;
  phanloai: any;
  graccess: any;
  gender: any;
  phone: any;
  address: any;
  identity: any;
  email: any;
  groupDevice: any;
  image: any;
  cvtName: any;
  hand: string = 'Left';
  hands: any
  devMorphoId: number = 0;
  devUNVId: number = 0;
  urlFinger1Sigma: string = '';
  urlFinger2Sigma: string = '';
  urlFinger3Sigma: string = '';
  formatFinger1Sigma: string = '';
  formatFinger2Sigma: string = '';
  formatFinger3Sigma: string = '';
  srcFinger1Sigma: string = '../../../assets/images/img_null.png';
  srcFinger2Sigma: string = '../../../assets/images/img_null.png';
  srcFinger3Sigma: string = '../../../assets/images/img_null.png';

  urlFinger1LeftWave: string = '';
  urlFinger2LeftWave: string = '';
  urlFinger3LeftWave: string = '';
  urlFinger4LeftWave: string = '';
  formatFinger1LeftWave: string = '';
  formatFinger2LeftWave: string = '';
  formatFinger3LeftWave: string = '';
  formatFinger4LeftWave: string = '';

  urlFinger1RightWave: string = '';
  urlFinger2RightWave: string = '';
  urlFinger3RightWave: string = '';
  urlFinger4RightWave: string = '';
  formatFinger1RightWave: string = '';
  formatFinger2RightWave: string = '';
  formatFinger3RightWave: string = '';
  formatFinger4RightWave: string = '';

  srcFinger1Wave: string = '../../../assets/images/img_null.png';
  srcFinger2Wave: string = '../../../assets/images/img_null.png';
  srcFinger3Wave: string = '../../../assets/images/img_null.png';
  srcFinger4Wave: string = '../../../assets/images/img_null.png';
  urlFace: string = '';
  srcFace: string = '../../../assets/images/img_null.png';
  morphoDeviceType: string = "";
  unvDeviceConnection: boolean = false
  tEmId: number = 0; // emId phụ vụ việc thêm vân tay và mặt.
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  @ViewChild('modalCreateOrEditEmployee') dialogTemplate!: TemplateRef<any>;
  @ViewChild('modalCreateCard') modalCreateCard!: TemplateRef<any>;
  @ViewChild('modalCreateVehicle') modalCreateVehicle!: TemplateRef<any>;
  @ViewChild('modelView') modelView!: TemplateRef<any>;
  @ViewChild('modalCreateFinger') modelCreateFinger!: TemplateRef<any>;
  selection = new SelectionModel<any>(true, []);
  Form!: FormGroup;
  FormCreateCard!: FormGroup;
  FormCreateVehicle!: FormGroup;
  action: string = '';
  title: string = '';
  pageSizeOptions: number[] = [10, 25, 50, 100];
  pageSize = this.pageSizeOptions[0];
  isAllChecked = false;
  filteredOptions!: Observable<any[]>;
  accessFace = true;
  accessCard = true;
  accessFinger = true;
  accessVehicle = true;
  emName = ''; //tên nhân viên fill lên modal thêm thẻ.
  emid = 0; //id nhân viên để lấy ra các thẻ.
  cardVehicleType: any;
  ticketType: any;
  cardNo: any;
  vehicleInfo: any;
  cardTypes = [
    { id: 0, name: 'Access Control Management' },
    { id: 1, name: 'Parking Management' }
  ];
  deviceTypeConfig!: string;
  indexFingerTitle: string = '';
  middleFingerTitle: string = '';
  ringFingerTitle: string = '';
  littleFingerTitle: string = '';
  fingerStatusArr: any;
  indexFinger: number = 0;
  middleFinger: number = 0;
  ringFinger: number = 0;
  littleFinger: number = 0;
  isCreateQrcode: boolean = false;
  isShowQrCode: boolean = false;
  srcQrCode: string = '';

  constructor(private spinner: NgxSpinnerService, private dataService: DataService, public dialog: MatDialog,
    private notificationService: NotificationService, private pagin: PaginatorCustomService,
    private translateService: TranslateService, private formBuilder: FormBuilder, private datepipe: DatePipe, private _deviceTypeService: DeviceTypeService) {
    this.formRegister();
    let leftHandLabel = '';
    this.translateService.get('TEmployeeModule.leftHandLabel').subscribe(data => leftHandLabel = data);
    let rightHandLabel = '';
    this.translateService.get('TEmployeeModule.rightHandLabel').subscribe(data => rightHandLabel = data);
    this.hands = [{ "label": leftHandLabel, "value": "Left" }, { "label": rightHandLabel, "value": "Right" }];

    this.translateService.get('TEmployeeModule.indexFinger').subscribe(data => this.indexFingerTitle = data);
    this.translateService.get('TEmployeeModule.middleFinger').subscribe(data => this.middleFingerTitle = data);
    this.translateService.get('TEmployeeModule.ringFinger').subscribe(data => this.ringFingerTitle = data);
    this.translateService.get('TEmployeeModule.littleFinger').subscribe(data => this.littleFingerTitle = data);

    let normalFinger: string = '';
    let amputatedFinger: string = '';
    let bandagedFinger: string = '';
    this.translateService.get('TEmployeeModule.normalFinger').subscribe(data => normalFinger = data);
    this.translateService.get('TEmployeeModule.amputatedFinger').subscribe(data => amputatedFinger = data);
    this.translateService.get('TEmployeeModule.bandagedFinger').subscribe(data => bandagedFinger = data);
    this.fingerStatusArr = [{ "value": 0, "title": normalFinger }, { "value": 1, "title": amputatedFinger }, { "value": 2, "title": bandagedFinger }]
  }

  ngOnInit(): void {
    this.deviceTypeConfig = this._deviceTypeService.getDeviceType();
    this.getAllData();
    this.getDepartments();
    this.getRegencys();
    this.getEmployeeTypes();
    this.getGroupAccess();
    this.getDeviceMorpho();
    this.getScheDevDetail();
    this.getDeviceUNV();
  }
  formRegister() {
    this.Form = this.formBuilder.group({
      emId: 0,
      emTypeId: ['', Validators.compose([Validators.required])],
      regId: ['', Validators.compose([Validators.required])],
      depId: ['', Validators.compose([Validators.required])],
      emIdCreated: '',
      gaId: ['', Validators.compose([Validators.required])],
      emName: ['', Validators.compose([Validators.required, Validators.maxLength(50)])],
      emGender: '',
      emCode: ['', Validators.compose([Validators.required, Validators.maxLength(10)])],
      emBirthdate: '',
      emIdentityNumber: ['', Validators.compose([Validators.pattern("^[0-9]*$"), Validators.minLength(9), Validators.maxLength(12)])],
      emPhone: ['',
        Validators.compose([
          Validators.pattern("^(\\+84-?)[0-9]{9}|(086|096|097|098|032|033|034|035|036|037|038|039|088|091|094|083|084|085|081|082|089|090|093|070|079|077|076|078|092|056|058|099|059)[0-9]{7}$"),
          Validators.minLength(10)
        ])],
      emAddress: ['', Validators.compose([Validators.minLength(2), Validators.maxLength(100)])],
      emEmail: ['', Validators.compose([Validators.email, Validators.maxLength(50)])],
      emImage: '',
      emStatus: true,
      emTimeCheck: false,
      editStatus: true,
      pin: null,
      faceExist: null,
      createdDate: null,
      schDevId: [''],
      devIdSynchronized: null
    });

    this.FormCreateCard = this.formBuilder.group({
      emId: ['', Validators.compose([Validators.required])],
      caNo: ['', Validators.compose([Validators.required, Validators.maxLength(30)])],
      caNumber: ['', Validators.compose([Validators.required, Validators.maxLength(30)])],
      emIdCreated: '',
      caStatus: true,
      using: true,
      caDamaged: false,
      caLost: false,
      destroyed: false,
      destroyedDate: '',
      applyDate: this.datepipe.transform(Date.now(), "yyyy-MM-dd"),
      expriedDate: '',
      dateEdit: this.datepipe.transform(Date.now(), "yyyy-MM-dd"),
      caTypeCheck: 0,
      synAccessDevice: null,
      geId: null
    });

    this.FormCreateVehicle = this.formBuilder.group({
      cvtId: ['', Validators.compose([Validators.required])],
      caId: ['', Validators.compose([Validators.required])],
      typeid: ['', Validators.compose([Validators.required])],
      emId: [''],
      plateNumber: '',
      vehicleType: '',
      vehicleModel: '',
      vehicleColor: '',
      vehicleImage: '',
      status: true,
    });
  }
  //open dialog add or edit
  openDialog(action: string, item?: any, config?: MatDialogConfig) {
    this.action = action;
    var acclogin = JSON.parse(localStorage.CURRENT_USER).emId;
    this.Form.controls['emIdCreated'].setValue(acclogin);
    if (action == 'create') {
      this.translateService.get('TEmployeeModule.createMessage').subscribe(data => this.title = data);
      this.Form.controls['emTimeCheck'].setValue(true);
      this.Form.controls['emStatus'].setValue(true);
      this.Form.controls['emGender'].setValue('M');
      this.Form.controls['schDevId'].setValue(63);
      const dialogRef = this.dialog.open(this.dialogTemplate, { width: '800px', autoFocus: false });
      dialogRef.disableClose = true;
      dialogRef.afterClosed().subscribe(result => {
        this.onReset();
        this.unvDeviceConnection = false;
        this.devUNVId = 0;
      })
    }
    else if (action == 'createCard') {
      this.getDeviceMorpho();
      this.selection.selected.forEach((value: any) => {
        this.emName = value.emName;
        this.FormCreateCard.controls['emId'].setValue(value.emId);
      });
      this.FormCreateCard.controls['caStatus'].setValue(true);
      this.FormCreateCard.controls['applyDate'].setValue(this.datepipe.transform(Date.now(), "yyyy-MM-dd"));
      this.FormCreateCard.controls['dateEdit'].setValue('');
      this.FormCreateCard.controls['caTypeCheck'].setValue(0);
      this.translateService.get('TCardNo.CreateCard').subscribe(data => this.title = data);

      const dialogRef = this.dialog.open(this.modalCreateCard, { width: '800px', autoFocus: false });
      dialogRef.disableClose = true;
      dialogRef.afterClosed().subscribe(result => {
        this.onReset();
        this.devMorphoId = 0;
        this.morphoDeviceType = "";
        this.isCreateQrcode = false;
        this.isShowQrCode = false;
        this.srcQrCode = '';
      });
    }
    else if (action == 'createVehicle') {
      this.selection.selected.forEach((value: any) => {
        this.emName = value.emName;
        this.FormCreateVehicle.controls['emId'].setValue(value.emId);
      });
      this.FormCreateVehicle.controls['status'].setValue(true);
      this.translateService.get('pVehicleModule.createMessage').subscribe(data => this.title = data);
      // this.getAllCardVehicleType();
      // this.getAllTicketType();

      const dialogRef = this.dialog.open(this.modalCreateVehicle, { width: '650px', autoFocus: false });
      dialogRef.disableClose = true;
      dialogRef.afterClosed().subscribe(result => {
        this.onReset();
      });
    }
    else if (action == 'view') {
      // this.getAllVehicleByEmId(item.emId);
      // this.getCardByEmIdParam(item.emId);
      this.emCodes = item.emCode;
      this.emNames = item.emName;
      this.department = item.depName;
      this.regency = item.regName;
      this.dob = item.emBirthdate;
      this.phanloai = item.emType;
      this.phone = item.emPhone;
      this.address = item.emAddress;
      this.identity = item.emIdentityNumber;
      this.email = item.emEmail;
      this.graccess = item.gaName;
      this.Form.controls['emImage'].setValue(item.emImage);
      if (item.emGender == 'Nam') {
        this.gender = item.emGender
      }
      else {
        this.gender = item.emGender
      }
      const dialogRef = this.dialog.open(this.modelView, { width: '900px' });
      dialogRef.disableClose = true;
      dialogRef.afterClosed().subscribe(result => {
        this.onReset();
      });
    }
    else if (action == 'edit') {
      this.preventAbuse = true;
      this.translateService.get('TEmployeeModule.editMessage').subscribe(data => this.title = data);
      this.dataService.get('TEmployee/getbyid/' + item.emId).subscribe((data: any) => {
        let item = data;
        var date = this.datepipe.transform(item.emBirthdate, "yyyy-MM-dd");
        this.Form.controls['emId'].setValue(item.emId);
        this.Form.controls['emTypeId'].setValue(item.emTypeId);
        this.Form.controls['depId'].setValue(item.depId);
        this.Form.controls['regId'].setValue(item.regId);
        this.Form.controls['gaId'].setValue(item.gaId);
        this.Form.controls['emName'].setValue(item.emName);
        this.Form.controls['emStatus'].setValue(item.emStatus);
        this.Form.controls['emTimeCheck'].setValue(item.emTimeCheck);
        this.Form.controls['emCode'].setValue(item.emCode);
        this.Form.controls['emBirthdate'].setValue(date);
        this.Form.controls['emIdentityNumber'].setValue(item.emIdentityNumber);
        this.Form.controls['emPhone'].setValue(item.emPhone);
        this.Form.controls['emAddress'].setValue(item.emAddress);
        this.Form.controls['emEmail'].setValue(item.emEmail);
        this.Form.controls['emImage'].setValue(item.emImage);
        this.Form.controls['emGender'].setValue(item.emGender);
        this.Form.controls['editStatus'].setValue(item.editStatus);
        this.Form.controls['pin'].setValue(item.pin);
        this.Form.controls['faceExist'].setValue(item.faceExist);
        this.Form.controls['createdDate'].setValue(item.createdDate);
        this.Form.controls['schDevId'].setValue(item.schDevId);
        this.Form.controls['devIdSynchronized'].setValue(item.devIdSynchronized);
        this.preventAbuse = false;
        const dialogRef = this.dialog.open(this.dialogTemplate, { width: '800px', autoFocus: false });
        dialogRef.disableClose = true;
        dialogRef.afterClosed().subscribe(result => {
          this.onReset();
          this.unvDeviceConnection = false;
          this.devUNVId = 0;
        });
      }, (err: any) => {
        this.translateService.get('messageSystem.loadFail').subscribe(data => MessageConstants.GET_FAILSE_MSG = data);
        this.notificationService.printErrorMessage(MessageConstants.GET_FAILSE_MSG);
        this.preventAbuse = false;
      });
    } else if (action == 'createFinger') {
      this.getDeviceMorpho();
      this.selection.selected.forEach((value: any) => {
        this.emName = value.emName;
        this.tEmId = value.emId;
      });
      this.translateService.get('TEmployeeModule.createFinger').subscribe(data => this.title = data);
      const dialogRef = this.dialog.open(this.modelCreateFinger, { width: '800px' });
      dialogRef.disableClose = true;
      dialogRef.afterClosed().subscribe(result => {
        this.onReset();
        this.resetFinger();
      });
    }
  }
  //get all vehicle truyền vào emid
  // getAllVehicleByEmId(emId: number) {
  //   this.preventAbuse = true;
  //   this.dataService.get('PVehicleInfo/getAllByEmId?emId=' + emId).subscribe((data: any) => {
  //     this.vehicleInfo = data;
  //     this.dataSourceVehicle = new MatTableDataSource(this.vehicleInfo);
  //     this.dataSourceVehicle.sort = this.sort;
  //     this.totalRow = data.totalCount;
  //     this.preventAbuse = false;
  //   }, (err: any) => {
  //     this.translateService.get('messageSystem.loadFail').subscribe(data => MessageConstants.GET_FAILSE_MSG = data);
  //     this.notificationService.printErrorMessage(MessageConstants.GET_FAILSE_MSG);
  //     this.preventAbuse = false;
  //   });
  // }
  //get all card no truyền emid vào
  // getCardByEmIdParam(emId: number) {
  //   this.preventAbuse = true;
  //   this.dataService.get('TCardNo/GetCardValidByEmId?emId=' + emId).subscribe((data: any) => {
  //     this.dataSourceCard = new MatTableDataSource(data);
  //     this.dataSourceCard.sort = this.sort;
  //     this.totalRow = data.totalCount;
  //     this.preventAbuse = false;
  //   }, (err: any) => {
  //     this.translateService.get('messageSystem.loadFail').subscribe(data => MessageConstants.GET_FAILSE_MSG = data);
  //     this.notificationService.printErrorMessage(MessageConstants.GET_FAILSE_MSG);
  //     this.preventAbuse = false;
  //   });
  // }
  //getall department
  getDepartments() {
    this.dataService.get('TDepartment/getall').subscribe((data: any) => {
      this.departments = data;
    }, err => {

    });
  }
  //getall chức vụ regency
  getRegencys() {
    this.dataService.get('TRegency/getall').subscribe((data: any) => {
      this.regencys = data;
    }, err => {

    });
  }
  //getall thiết bị morpho
  getDeviceMorpho() {
    this.preventAbuse = true;
    this.dataService.get('TDevice/getdevicemorpho').subscribe((data: any) => {
      this.deviceMorpho = data;
      this.preventAbuse = false;
    }, (err: any) => {
      this.translateService.get('messageSystem.loadFail').subscribe(data => MessageConstants.GET_FAILSE_MSG = data);
      this.notificationService.printErrorMessage(MessageConstants.GET_FAILSE_MSG);
      this.preventAbuse = false;
    });
  }
  //getall thiết bị unv
  getDeviceUNV() {
    this.preventAbuse = true;
    this.dataService.get('TDevice/getdeviceunv').subscribe((data: any) => {
      this.deviceUNV = data;
      this.preventAbuse = false;
    }, (err: any) => {
      this.translateService.get('messageSystem.loadFail').subscribe(data => MessageConstants.GET_FAILSE_MSG = data);
      this.notificationService.printErrorMessage(MessageConstants.GET_FAILSE_MSG);
      this.preventAbuse = false;
    });
  }
  getScheDevDetail() {
    this.preventAbuse = true;
    this.dataService.get('AScheduleDeviceDetail/getall').subscribe((data: any) => {
      this.schedevdetails = data;
      this.preventAbuse = false;
    }, (err: any) => {
      this.translateService.get('messageSystem.loadFail').subscribe(data => MessageConstants.GET_FAILSE_MSG = data);
      this.notificationService.printErrorMessage(MessageConstants.GET_FAILSE_MSG);
      this.preventAbuse = false;
    });
  }
  getEmployeeTypes() {
    this.dataService.get('TEmployee/getEmployeeType').subscribe((data: any) => {
      this.employeeTypes = data;
    }, err => {

    });
  }
  getGroupAccess() {
    this.dataService.get('TGroupAccess/getall').subscribe((data: any) => {
      this.groupAccesss = data;
    }, err => {

    });
  }
  //get all loại thẻ xe
  // getAllCardVehicleType() {
  //   this.dataService.get('PCardVehicleType/getall').subscribe((data: any) => {
  //     this.cardVehicleType = data;
  //   }, err => {
  //   });
  // }
  //get all loại vé
  // getAllTicketType() {
  //   this.dataService.get('PTicketType/getall').subscribe((data: any) => {
  //     this.ticketType = data;
  //   }, err => {
  //   });
  // }
  // //get all thẻ theo nhân viên
  // getAllCardByEmId() {
  //   this.selection.selected.forEach((value: any) => {
  //     this.emid = value.emId;
  //   });
  //   if (this.emid != 0) {
  //     this.dataService.get('TCardNo/GetCardValidByEmId?emId=' + this.emid).subscribe((data: any) => {
  //       this.cardNo = data;
  //     }, err => {
  //     });
  //   }
  // }
  //getAllData by paging
  getAllData() {
    this.preventAbuse = true;
    this.dataService.get('TEmployee/getAllByPaging?page=' + this.page + "&pageSize=" + this.pageSize + "&keyword=" + this.keyword).subscribe((data: any) => {
      this.dataSource = new MatTableDataSource(data.items);
      this.dataSource.sort = this.sort;
      this.totalRow = data.totalCount;
      this.preventAbuse = false;
    }, err => {
      this.translateService.get('messageSystem.loadFail').subscribe(data => MessageConstants.GET_FAILSE_MSG = data);
      this.notificationService.printErrorMessage(MessageConstants.GET_FAILSE_MSG);
      this.preventAbuse = false;
    });
  }
  get control() { return this.Form.controls; }
  //search
  applyFilter(event: Event) {
    //const filterValue = (event.target as HTMLInputElement).value;
    this.keyword = (event.target as HTMLInputElement).value;
    this.page = 0;
    this.getAllData();
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
    return this.Form.controls;
  }
  get getValidFormCreateCard(): { [key: string]: AbstractControl } {
    return this.FormCreateCard.controls;
  }
  get getValidFormCreateVehicle(): { [key: string]: AbstractControl } {
    return this.FormCreateVehicle.controls;
  }

  isDisableEmTimeCheck: boolean = false;
  changeEmType(event: MatOptionSelectionChange, item: any) {
    if (event.source.selected) {
      let emTypeId = item.emTypeId;
      if (emTypeId != 1) {
        this.isDisableEmTimeCheck = true;
        this.Form.controls['emTimeCheck'].setValue(false);
      } else {
        this.isDisableEmTimeCheck = false;
      }
    }
  }
  changeCardType(event: MatOptionSelectionChange, item: any) {
    if (event.source.selected) {
      let id = item.id;
      if (id == 0) {
        this.FormCreateCard.controls["applyDate"].clearValidators();
        this.FormCreateCard.controls["applyDate"].updateValueAndValidity();
      } else {
        this.FormCreateCard.controls["applyDate"].setValidators([Validators.required]);
        this.FormCreateCard.controls["applyDate"].updateValueAndValidity();
      }
    }
  }
  //Save change data
  addData() {
    if (this.Form.invalid) {
      return;
    }
    if (this.action == 'create') {
      let data = this.Form.value;
      this.preventAbuse = true;
      this.spinner.show();
      this.dataService.post('TEmployee/create', data).subscribe(data => {
        let message!: string;
        this.translateService.get('messageSystem.createSuccess').subscribe(mes => message = mes);
        this.notificationService.printSuccessMessage(message);
        this.getAllData();
        this.preventAbuse = false;
        this.spinner.hide();
      }, err => {
        this.preventAbuse = false;
        this.spinner.hide();
        let error!: string;
        this.translateService.get('messageSystem.createFail').subscribe(mes => error = mes);
        this.notificationService.printErrorMessage(error);
      });
    }
    else if (this.action == 'edit') {
      let dto = this.Form.value;
      this.preventAbuse = true;
      this.spinner.show();
      this.dataService.put('TEmployee/update', dto).subscribe(data => {
        let message!: string;
        this.translateService.get('messageSystem.updateSuccess').subscribe(mes => message = mes);
        this.notificationService.printSuccessMessage(message);
        this.getAllData();
        this.getDepartments();
        this.preventAbuse = false;
        this.spinner.hide();
      }, err => {
        this.spinner.hide();
        this.preventAbuse = false;
        let error!: string;
        this.translateService.get('messageSystem.updateFail').subscribe(mes => error = mes);
        this.notificationService.printErrorMessage(error);
      })
    }
  }
  // thêm thẻ
  // addCardNo() {
  //   if (this.FormCreateCard.invalid) {
  //     return;
  //   }
  //   let errMess!: string
  //   this.translateService.get('TCardNo.dateErrMessage').subscribe(value => errMess = value);
  //   if (this.FormCreateCard.controls['applyDate'].value != '' && this.FormCreateCard.controls['expriedDate'].value != '' && this.FormCreateCard.controls['applyDate'].value > this.FormCreateCard.controls['expriedDate'].value) {
  //     this.notificationService.printErrorMessage(errMess);
  //     return;
  //   }
  //   var acclogin = JSON.parse(localStorage.CURRENT_USER).emId;
  //   this.FormCreateCard.controls['using'].setValue(true);
  //   this.FormCreateCard.controls['caDamaged'].setValue(false);
  //   this.FormCreateCard.controls['caLost'].setValue(false);
  //   this.FormCreateCard.controls['destroyed'].setValue(false);
  //   if (acclogin != null) {
  //     this.FormCreateCard.controls['emIdCreated'].setValue(acclogin);
  //   }
  //   let data = this.FormCreateCard.value;
  //   if (this.FormCreateCard.controls['caStatus'].value == false) {
  //     this.isCreateQrcode = false;
  //   }
  //   this.preventAbuse = true;
  //   this.spinner.show();
  //   this.dataService.post('TCardNo/create', data).subscribe((data: any) => {
  //     let message!: string;
  //     this.translateService.get('messageSystem.createSuccess').subscribe(mes => message = mes);
  //     this.notificationService.printSuccessMessage(message);
  //     this.selection.clear();
  //     this.getAllData();
  //     if (this.deviceTypeConfig == 'hundevice') {
  //       let qrcodeModel = {
  //         emId: data.emId,
  //         caId: data.caId,
  //         caNo: data.caNo,
  //         startDate: data.applyDate == null ? '' : data.applyDate,
  //         endDate: data.expriedDate == null ? '' : data.expriedDate,
  //         isCreate: this.isCreateQrcode
  //       }
  //       this.dataService.post('TEmployeeQrcode/create', qrcodeModel).subscribe((res: any) => {
  //         this.preventAbuse = false;
  //         this.spinner.hide();
  //         if (res != null) {
  //           this.isShowQrCode = true;
  //           this.srcQrCode = 'data:image/png;base64,' + res.qrCodeImg
  //         }
  //       }, (err: any) => {
  //         this.preventAbuse = false;
  //         this.spinner.hide();
  //         this.notificationService.printErrorMessage(err.message);
  //       });
  //     } else {
  //       this.preventAbuse = false;
  //       this.spinner.hide();
  //     }
  //   }, err => {
  //     this.spinner.hide();
  //     this.preventAbuse = false;
  //     let error!: string;
  //     this.translateService.get('messageSystem.createFail').subscribe(mes => error = mes);
  //     this.notificationService.printErrorMessage(error);
  //   });
  // }
  //thêm phương tiện
  // addVehicle() {
  //   if (this.FormCreateVehicle.invalid) {
  //     return;
  //   }
  //   let PVehicleInfo = this.FormCreateVehicle.value;
  //   this.preventAbuse = true;
  //   this.spinner.show();
  //   this.dataService.post('PVehicleInfo/create', PVehicleInfo).subscribe(data => {
  //     let message!: string;
  //     this.translateService.get('messageSystem.createSuccess').subscribe(mes => message = mes);
  //     this.notificationService.printSuccessMessage(message);
  //     this.getAllData();
  //     this.preventAbuse = false;
  //     this.spinner.hide();
  //     this.selection.clear();
  //   }, err => {
  //     this.preventAbuse = false;
  //     this.spinner.hide();
  //     let error!: string;
  //     this.translateService.get('pVehicleModule.cardNoDiferent').subscribe(mes => error = mes);
  //     this.notificationService.printErrorMessage(error);
  //   });
  // }
  //reset form
  onReset() {
    this.action == '';
    this.dialog.closeAll();
    this.Form.reset();
    this.Form.controls['emId'].setValue(0);
    this.FormCreateCard.reset();
    this.FormCreateVehicle.reset();
    this.selection.clear();
  }
  isSectionSelectedEmStatus(): boolean {
    if (this.selection.selected.length == 0) {
      return false;
    }
    var check = this.selection.selected.every(function (item: any) {
      return item.emStatus == true;
    });
    return check;
  }
  isSectionSelectedNotEmStatus(): boolean {
    if (this.selection.selected.length == 0) {
      return false;
    }
    var check = this.selection.selected.every(function (item: any) {
      return item.emStatus == false;
    });
    return check;
  }
  //lock muilti
  lockEMployee() {
    let lstEmChecked: any[] = [];
    this.selection.selected.forEach((value: any) => {
      let id = value.emId;
      lstEmChecked.push(id);
    });
    this.translateService.get('messageSystem.confirmLock').subscribe(data => MessageConstants.CONFIRM_LOCK_MSG = data);
    this.notificationService.printConfirmationDialog(MessageConstants.CONFIRM_LOCK_MSG, () => this.lockItemConfirm(JSON.stringify(lstEmChecked)));
  }
  lockItemConfirm(lstemId: string) {
    this.preventAbuse = true;
    this.spinner.show();
    this.dataService.delete('TEmployee/lockmulti', 'checkedList', lstemId).subscribe((response: any) => {
      this.translateService.get('messageSystem.deleteSuccess').subscribe(data => MessageConstants.DELETED_OK_MSG = data);
      this.translateService.get('messageSystem.deleteFail').subscribe(data => MessageConstants.DELETE_FAILSE_MSG = data);
      if (response[0] > 0) {
        this.notificationService.printSuccessMessage(MessageConstants.DELETED_OK_MSG + response[0]);
      }
      if (response[1] > 0) {
        this.notificationService.printErrorMessage(MessageConstants.DELETE_FAILSE_MSG + response[1]);
      }

      this.selection.clear();
      this.getAllData();
      this.getDepartments();
      this.spinner.hide();
      this.preventAbuse = false;
    }, err => {
      this.spinner.hide();
      this.preventAbuse = false;
      this.translateService.get('messageSystem.deletefail').subscribe(data => MessageConstants.DELETE_FAILSE_MSG = data);
      this.notificationService.printErrorMessage(MessageConstants.DELETE_FAILSE_MSG);
    });
  }
  //end remove
  //paging
  ngAfterViewInit() {
    this.paginator._intl.itemsPerPageLabel = this.pagin.setLable;
    this.paginator._intl.firstPageLabel = this.pagin.firstButton;
    this.paginator._intl.nextPageLabel = this.pagin.nextButton;
    this.paginator._intl.lastPageLabel = this.pagin.lastButton;
    this.paginator._intl.previousPageLabel = this.pagin.preButton;
  }
  //change paging
  onChangePage(pe: PageEvent) {
    this.page = pe.pageIndex;
    this.pageSize = pe.pageSize;
    this.getAllData();
  }
  // Image upload
  clearImage() {
    this.Form.controls['emImage'].setValue('');
  }
  imagePreview(e: any) {
    const file = (e.target as HTMLInputElement).files![0];
    const reader = new FileReader();
    reader.onload = () => {
      let base64String = reader.result as string;
      let img = base64String.split('base64,')[1];
      this.Form.controls['emImage'].setValue(img);
    }
    reader.readAsDataURL(file);

  }
  imagePreviewVehicle(e: any) {
    const file = (e.target as HTMLInputElement).files![0];
    const reader = new FileReader();
    reader.onload = () => {
      let base64String = reader.result as string;
      let img = base64String.split('base64,')[1];
      this.FormCreateVehicle.controls['vehicleImage'].setValue(img);
    }
    reader.readAsDataURL(file);
  }
  clearImageVehicle() {
    this.FormCreateVehicle.controls['vehicleImage'].setValue('');
  }
  //end Image
  checkMorphoDevice(event: MatOptionSelectionChange, item: any) {
    this.morphoDeviceType = "";
    if (event.source.selected) {
      let devId = item.devId;
      this.dataService.getDevice(this.deviceTypeConfig + '/connect?devId=' + devId).subscribe((res: any) => {
        if (res.connected) {
          this.morphoDeviceType = res.deviceType
        } else {
          this.morphoDeviceType = "";
        }
      }, (err: any) => {
        this.morphoDeviceType = "";
      });
    }
  }
  checkUNVDevice(event: MatOptionSelectionChange, item: any) {
    this.unvDeviceConnection = false;
    if (event.source.selected) {
      let devId = item.devId;
      this.dataService.getDevice(this.deviceTypeConfig + '/connect?devId=' + devId).subscribe((res: any) => {
        this.unvDeviceConnection = res.connected;
      }, (err: any) => {
        this.unvDeviceConnection = false;
      });
    }
  }
  resetFinger() {
    this.devMorphoId = 0;
    this.morphoDeviceType = "";
    this.urlFinger1Sigma = '';
    this.urlFinger2Sigma = '';
    this.urlFinger3Sigma = '';
    this.formatFinger1Sigma = '';
    this.formatFinger2Sigma = '';
    this.formatFinger3Sigma = '';
    this.srcFinger1Sigma = '../../../assets/images/img_null.png';
    this.srcFinger2Sigma = '../../../assets/images/img_null.png';
    this.srcFinger3Sigma = '../../../assets/images/img_null.png';
    this.hand = "Left";

    this.urlFinger1LeftWave = '';
    this.urlFinger2LeftWave = '';
    this.urlFinger3LeftWave = '';
    this.urlFinger4LeftWave = '';
    this.formatFinger1LeftWave = '';
    this.formatFinger2LeftWave = '';
    this.formatFinger3LeftWave = '';
    this.formatFinger4LeftWave = '';

    this.urlFinger1RightWave = '';
    this.urlFinger2RightWave = '';
    this.urlFinger3RightWave = '';
    this.urlFinger4RightWave = '';
    this.formatFinger1RightWave = '';
    this.formatFinger2RightWave = '';
    this.formatFinger3RightWave = '';
    this.formatFinger4RightWave = '';

    this.srcFinger1Wave = '../../../assets/images/img_null.png';
    this.srcFinger2Wave = '../../../assets/images/img_null.png';
    this.srcFinger3Wave = '../../../assets/images/img_null.png';
    this.srcFinger4Wave = '../../../assets/images/img_null.png';
    this.urlFace = '';
    this.srcFace = '../../../assets/images/img_null.png';
    this.indexFinger = 0;
    this.middleFinger = 0;
    this.ringFinger = 0;
    this.littleFinger = 0;
  }
  enrollFinger1() {
    this.srcFinger1Sigma = '../../../assets/images/img_null.png';
    this.urlFinger1Sigma = '';
    this.formatFinger1Sigma = '';
    this.preventAbuse = true;
    let enrollOption = {
      devId: this.devMorphoId
    }
    this.dataService.postDevice(this.deviceTypeConfig + '/startenroll', enrollOption)
      .subscribe((res: any) => {
        if (res.connected) {
          this.urlFinger1Sigma = res.enrollResult.UrlFinger1;
          if (this.urlFinger1Sigma != '') {
            this.formatFinger1Sigma = res.enrollResult.FormatFringer1;
            this.srcFinger1Sigma = 'data:image/png;base64,' + this.formatFinger1Sigma;
          }
        } else {
          this.morphoDeviceType = "";
        }
        this.preventAbuse = false;
      }, (err) => {
        this.morphoDeviceType = "";
        this.preventAbuse = false;
      });
  }
  enrollFinger2() {
    this.srcFinger2Sigma = '../../../assets/images/img_null.png';
    this.urlFinger2Sigma = '';
    this.formatFinger2Sigma = '';
    this.preventAbuse = true;
    let enrollOption = {
      devId: this.devMorphoId
    }
    this.dataService.postDevice(this.deviceTypeConfig + '/startenroll', enrollOption)
      .subscribe((res: any) => {
        if (res.connected) {
          this.urlFinger2Sigma = res.enrollResult.UrlFinger1;
          if (this.urlFinger2Sigma != '') {
            this.formatFinger2Sigma = res.enrollResult.FormatFringer1;
            this.srcFinger2Sigma = 'data:image/png;base64,' + this.formatFinger2Sigma;
          }
        } else {
          this.morphoDeviceType = "";
        }
        this.preventAbuse = false;
      }, (err) => {
        this.morphoDeviceType = "";
        this.preventAbuse = false;
      });
  }
  enrollFinger3() {
    this.srcFinger3Sigma = '../../../assets/images/img_null.png';
    this.urlFinger3Sigma = '';
    this.formatFinger3Sigma = '';
    this.preventAbuse = true;
    let enrollOption = {
      devId: this.devMorphoId
    }
    this.dataService.postDevice(this.deviceTypeConfig + '/startenroll', enrollOption)
      .subscribe((res: any) => {
        if (res.connected) {
          this.urlFinger3Sigma = res.enrollResult.UrlFinger1;
          if (this.urlFinger3Sigma != '') {
            this.formatFinger3Sigma = res.enrollResult.FormatFringer1;
            this.srcFinger3Sigma = 'data:image/png;base64,' + this.formatFinger3Sigma;
          }
        } else {
          this.morphoDeviceType = "";
        }
        this.preventAbuse = false;
      }, (err) => {
        this.morphoDeviceType = "";
        this.preventAbuse = false;
      });
  }
  enrollFace() {
    this.urlFace = '';
    this.srcFace = '../../../assets/images/img_null.png';
    this.preventAbuse = true;
    let enrollOption = {
      devId: this.devMorphoId
    }
    this.dataService.postDevice(this.deviceTypeConfig + '/startenroll', enrollOption)
      .subscribe((res: any) => {
        if (res.connected) {
          this.urlFace = res.enrollResult.UrlFinger11;
          if (this.urlFace != '') {
            this.srcFace = '../../../assets/images/isExitFace.jpg';
          }
        } else {
          this.morphoDeviceType = "";
        }
        this.preventAbuse = false;
      }, (err) => {
        this.morphoDeviceType = "";
        this.preventAbuse = false;
      });
  }
  enrollMorphoWave() {
    if (this.hand == 'Left') {
      this.srcFinger1Wave = '../../../assets/images/img_null.png';
      this.urlFinger1LeftWave = '';
      this.formatFinger1LeftWave = '';

      this.srcFinger2Wave = '../../../assets/images/img_null.png';
      this.urlFinger2LeftWave = '';
      this.formatFinger2LeftWave = '';

      this.srcFinger3Wave = '../../../assets/images/img_null.png';
      this.urlFinger3LeftWave = '';
      this.formatFinger3LeftWave = '';

      this.srcFinger4Wave = '../../../assets/images/img_null.png';
      this.urlFinger4LeftWave = '';
      this.formatFinger4LeftWave = '';
      this.preventAbuse = true;

      let morphoWaveEnrollOption = {
        LeftHandIndexStatus: this.indexFinger,
        LeftHandMiddletatus: this.middleFinger,
        LeftHandRingStatus: this.ringFinger,
        LeftHandLittleStatus: this.littleFinger
      }
      let enrollOption = {
        devId: this.devMorphoId,
        hand: this.hand,
        morphoWaveEnrollOption: morphoWaveEnrollOption
      }
      this.dataService.postDevice(this.deviceTypeConfig + '/startenroll', enrollOption)
        .subscribe((res: any) => {
          if (res.connected) {
            this.urlFinger1LeftWave = res.enrollResult.UrlFinger1;
            if (this.urlFinger1LeftWave != '') {
              this.formatFinger1LeftWave = res.enrollResult.FormatFringer1;
              this.srcFinger1Wave = 'data:image/png;base64,' + this.formatFinger1LeftWave;
            }

            this.urlFinger2LeftWave = res.enrollResult.UrlFinger2;
            if (this.urlFinger2LeftWave != '') {
              this.formatFinger2LeftWave = res.enrollResult.FormatFringer2;
              this.srcFinger2Wave = 'data:image/png;base64,' + this.formatFinger2LeftWave;
            }

            this.urlFinger3LeftWave = res.enrollResult.UrlFinger3;
            if (this.urlFinger3LeftWave != '') {
              this.formatFinger3LeftWave = res.enrollResult.FormatFringer3;
              this.srcFinger3Wave = 'data:image/png;base64,' + this.formatFinger3LeftWave;
            }

            this.urlFinger4LeftWave = res.enrollResult.UrlFinger4;
            if (this.urlFinger4LeftWave != '') {
              this.formatFinger4LeftWave = res.enrollResult.FormatFringer4;
              this.srcFinger4Wave = 'data:image/png;base64,' + this.formatFinger4LeftWave;
            }
          } else {
            this.morphoDeviceType = "";
          }
          this.preventAbuse = false;
        }, (err) => {
          this.morphoDeviceType = "";
          this.preventAbuse = false;
        });
    } else {
      this.srcFinger1Wave = '../../../assets/images/img_null.png';
      this.urlFinger1RightWave = '';
      this.formatFinger1RightWave = '';

      this.srcFinger2Wave = '../../../assets/images/img_null.png';
      this.urlFinger2RightWave = '';
      this.formatFinger2RightWave = '';

      this.srcFinger3Wave = '../../../assets/images/img_null.png';
      this.urlFinger3RightWave = '';
      this.formatFinger3RightWave = '';

      this.srcFinger4Wave = '../../../assets/images/img_null.png';
      this.urlFinger4RightWave = '';
      this.formatFinger4RightWave = '';
      this.preventAbuse = true;

      let morphoWaveEnrollOption = {
        RightHandIndexStatus: this.indexFinger,
        RightHandMiddletatus: this.middleFinger,
        RightHandRingStatus: this.ringFinger,
        RightHandLittleStatus: this.littleFinger
      }
      let enrollOption = {
        devId: this.devMorphoId,
        hand: this.hand,
        morphoWaveEnrollOption: morphoWaveEnrollOption
      }
      this.dataService.postDevice(this.deviceTypeConfig + '/startenroll', enrollOption)
        .subscribe((res: any) => {
          if (res.connected) {
            this.urlFinger1RightWave = res.enrollResult.UrlFinger1;
            if (this.urlFinger1RightWave != '') {
              this.formatFinger1RightWave = res.enrollResult.FormatFringer1;
              this.srcFinger1Wave = 'data:image/png;base64,' + this.formatFinger1RightWave;
            }

            this.urlFinger2RightWave = res.enrollResult.UrlFinger2;
            if (this.urlFinger2RightWave != '') {
              this.formatFinger2RightWave = res.enrollResult.FormatFringer2;
              this.srcFinger2Wave = 'data:image/png;base64,' + this.formatFinger2RightWave;
            }

            this.urlFinger3RightWave = res.enrollResult.UrlFinger3;
            if (this.urlFinger3RightWave != '') {
              this.formatFinger3RightWave = res.enrollResult.FormatFringer3;
              this.srcFinger3Wave = 'data:image/png;base64,' + this.formatFinger3RightWave;
            }

            this.urlFinger4RightWave = res.enrollResult.UrlFinger4;
            if (this.urlFinger4RightWave != '') {
              this.formatFinger4RightWave = res.enrollResult.FormatFringer4;
              this.srcFinger4Wave = 'data:image/png;base64,' + this.formatFinger4RightWave;
            }
          } else {
            this.morphoDeviceType = "";
          }
          this.preventAbuse = false;
        }, (err) => {
          this.morphoDeviceType = "";
          this.preventAbuse = false;
        });
    }
  }
  addFinger() {
    let employeeFinger: any = {
      EmId: 0,
      UrlFinger1Sigma: '',
      UrlFinger2Sigma: '',
      UrlFinger3Sigma: '',
      UrlFinger1LeftWave: '',
      UrlFinger2LeftWave: '',
      UrlFinger3LeftWave: '',
      UrlFinger4LeftWave: '',
      UrlFinger1RightWave: '',
      UrlFinger2RightWave: '',
      UrlFinger3RightWave: '',
      UrlFinger4RightWave: '',
      FormatFinger1Sigma: null,
      FormatFinger2Sigma: null,
      FormatFinger3Sigma: null,
      FormatFinger1LeftWave: null,
      FormatFinger2LeftWave: null,
      FormatFinger3LeftWave: null,
      FormatFinger4LeftWave: null,
      FormatFinger1RightWave: null,
      FormatFinger2RightWave: null,
      FormatFinger3RightWave: null,
      FormatFinger4RightWave: null,
    };
    employeeFinger.EmId = this.tEmId;
    employeeFinger.UrlFinger1Sigma = this.urlFinger1Sigma;
    employeeFinger.UrlFinger2Sigma = this.urlFinger2Sigma;
    employeeFinger.UrlFinger3Sigma = this.urlFinger3Sigma;
    employeeFinger.UrlFinger1LeftWave = this.urlFinger1LeftWave;
    employeeFinger.UrlFinger2LeftWave = this.urlFinger2LeftWave;
    employeeFinger.UrlFinger3LeftWave = this.urlFinger3LeftWave;
    employeeFinger.UrlFinger4LeftWave = this.urlFinger4LeftWave;
    employeeFinger.UrlFinger1RightWave = this.urlFinger1RightWave;
    employeeFinger.UrlFinger2RightWave = this.urlFinger2RightWave;
    employeeFinger.UrlFinger3RightWave = this.urlFinger3RightWave;
    employeeFinger.UrlFinger4RightWave = this.urlFinger4RightWave;
    if (this.formatFinger1Sigma != '') {
      employeeFinger.FormatFinger1Sigma = this.formatFinger1Sigma;
    }
    if (this.formatFinger2Sigma != '') {
      employeeFinger.FormatFinger2Sigma = this.formatFinger2Sigma;
    }
    if (this.formatFinger3Sigma != '') {
      employeeFinger.FormatFinger3Sigma = this.formatFinger3Sigma;
    }

    if (this.formatFinger1LeftWave != '') {
      employeeFinger.FormatFinger1LeftWave = this.formatFinger1LeftWave;
    }
    if (this.formatFinger2LeftWave != '') {
      employeeFinger.FormatFinger2LeftWave = this.formatFinger2LeftWave;
    }
    if (this.formatFinger3LeftWave != '') {
      employeeFinger.FormatFinger3LeftWave = this.formatFinger3LeftWave;
    }
    if (this.formatFinger4LeftWave != '') {
      employeeFinger.FormatFinger4LeftWave = this.formatFinger4LeftWave;
    }


    if (this.formatFinger1RightWave != '') {
      employeeFinger.FormatFinger1RightWave = this.formatFinger1RightWave;
    }
    if (this.formatFinger2RightWave != '') {
      employeeFinger.FormatFinger2RightWave = this.formatFinger2RightWave;
    }
    if (this.formatFinger3RightWave != '') {
      employeeFinger.FormatFinger3RightWave = this.formatFinger3RightWave;
    }
    if (this.formatFinger4RightWave != '') {
      employeeFinger.FormatFinger4RightWave = this.formatFinger4RightWave;
    }
    if (this.urlFinger1Sigma != '' || this.urlFinger2Sigma != '' || this.urlFinger3Sigma != '' || this.urlFinger1LeftWave != '' || this.urlFinger2LeftWave != '' || this.urlFinger3LeftWave != '' || this.urlFinger4LeftWave != '' || this.urlFinger1RightWave != '' || this.urlFinger2RightWave != '' || this.urlFinger3RightWave != '' || this.urlFinger4RightWave != '') {
      this.preventAbuse = true;
      this.spinner.show();
      this.dataService.post('TEmployeeFinger/create', employeeFinger)
        .subscribe((res: any) => {
          let message!: string;
          this.translateService.get('messageSystem.createSuccess').subscribe(mes => message = mes);
          this.notificationService.printSuccessMessage(message);
          this.getAllData();
          this.spinner.hide();
          this.preventAbuse = false;
        }, (err) => {
          this.preventAbuse = false;
          this.spinner.hide();
          let error!: string;
          this.translateService.get('messageSystem.createFail').subscribe(mes => error = mes);
          this.notificationService.printErrorMessage(error);
        });
    }

    if (this.urlFace != '') {
      let employeeFace: any = {
        EmId: 0,
        FaceData: '',
        DevTypeCode: 'Morpho'
      };
      employeeFace.EmId = this.tEmId;
      employeeFace.FaceData = this.urlFace;
      this.preventAbuse = true;
      this.spinner.show();
      this.dataService.post('TEmployeeFace/create', employeeFace)
        .subscribe((res: any) => {
          let message!: string;
          this.translateService.get('messageSystem.createSuccess').subscribe(mes => message = mes);
          this.notificationService.printSuccessMessage(message);
          this.getAllData();
          this.spinner.hide();
          this.preventAbuse = false;
        }, (err) => {
          this.preventAbuse = false;
          this.spinner.hide();
          let error!: string;
          this.translateService.get('messageSystem.createFail').subscribe(mes => error = mes);
          this.notificationService.printErrorMessage(error);
        });
    }
  }
  getCaNo() {
    if (this.devMorphoId == 0) {
      return;
    }
    this.FormCreateCard.controls['caNo'].setValue('');
    this.preventAbuse = true;
    this.dataService.getDevice(this.deviceTypeConfig + '/getidcard?devId=' + this.devMorphoId).subscribe((res: any) => {
      if (res.connected) {
        this.FormCreateCard.controls['caNo'].setValue(res.cardId);
      } else {
        this.morphoDeviceType = "";
      }
      this.preventAbuse = false;
    }, (err: any) => {
      this.morphoDeviceType = "";
      this.preventAbuse = false;
    });
  }
  takePhoto() {
    this.Form.controls['emImage'].setValue('');
    this.preventAbuse = true;
    this.dataService.getDevice(this.deviceTypeConfig + '/takephoto?devId=' + this.devUNVId).subscribe((res: any) => {
      if (res.connected) {
        if (res.image != null) {
          this.Form.controls['emImage'].setValue(res.image);
        }
      } else {
        this.unvDeviceConnection = false;
      }
      this.preventAbuse = false;
    }, (err: any) => {
      this.unvDeviceConnection = false;
      this.preventAbuse = false;
    });
  }
  changeHand(event: MatOptionSelectionChange, item: any) {
    if (event.source.selected) {
      this.indexFinger = 0;
      this.middleFinger = 0;
      this.ringFinger = 0;
      this.littleFinger = 0;
      this.srcFinger1Wave = '../../../assets/images/img_null.png';
      this.srcFinger2Wave = '../../../assets/images/img_null.png';
      this.srcFinger3Wave = '../../../assets/images/img_null.png';
      this.srcFinger4Wave = '../../../assets/images/img_null.png';
    }
  }
}
