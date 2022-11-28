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

  displayedColumns: string[] = ['select','position','emName', 'emCode', 'depName', 'regName', 
                                'emType','emGender','emBirthDay','emIdentityNumber',
                                'emAddress','description','emPhone','emEmail','emImage',
                                'emStatus','placeId','zoneName','option'];                              
  dataSource = new MatTableDataSource<any>();
  preventAbuse = false;
  page = 0;
  keyword: string = '';
  totalRow: number = 0;
  totalPage: number = 0;
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  @ViewChild('dialog') dialogTemplate!: TemplateRef<any>;
  selection = new SelectionModel<any>(true, []);
  employeeForm!: FormGroup;
  action: string = '';
  title: string = '';
  pageSizeOptions: number[] = [10, 25, 50, 100];
  pageSize = this.pageSizeOptions[0];
  isAllChecked = false;

  //option
  departments:any;
  regencys:any;
  zones:any;
  emtypes:any;

  constructor(private spinner: NgxSpinnerService, private dataService: DataService, public dialog: MatDialog,
    private notificationService: NotificationService, private pagin: PaginatorCustomService,
    private translateService: TranslateService, private formBuilder: FormBuilder, private datepipe: DatePipe) {

      this.employeeForm = this.formBuilder.group({
        emId:'',
        emName: ['', Validators.compose([Validators.required, Validators.maxLength(100)])],
        emCode: ['', Validators.compose([Validators.required, Validators.maxLength(15)])],
        depId: ['', Validators.compose([Validators.required])],
        regId: ['', Validators.compose([Validators.required])],
        emTypeId: ['', Validators.compose([Validators.required])],
        emGender:'',
        emBirthdate:'',
        emIdentityNumber:['',  Validators.compose([
          Validators.pattern('^[0-9]*$'),
          Validators.minLength(9),
          Validators.maxLength(12),
        ])],
        emAddress: ['', Validators.compose([Validators.maxLength(500)])],
        description:['',Validators.compose([Validators.maxLength(400)])],
        emPhone:['',Validators.compose([
          Validators.pattern(
            '^(\\+84-?)[0-9]{9}|(086|096|097|098|032|033|034|035|036|037|038|039|088|091|094|083|084|085|081|082|089|090|093|070|079|077|076|078|092|056|058|099|059)[0-9]{7}$'
          ),
          Validators.minLength(10),
        ])],
        emEmail:['',Validators.compose([Validators.email, Validators.maxLength(50)])],
        emImage:'',
        emStatus:true,
        placeId:['', Validators.compose([Validators.required])],
        zonId:['',Validators.compose([Validators.required])]
      })
  }

  ngOnInit(): void {
    this.loadData();
    this.getDepartments();
    this.getZones();
    this.getRegencys();
    this.getEmployeeTypes();
  }

  //getlistping
  loadData() {
    this.preventAbuse = true;
    this.dataService
      .get(
        'TEmployee/GetAllByPaging?page=' +
        this.page +
        '&pageSize=' +
        this.pageSize +
        '&keyword=' +
        this.keyword
      )
      .subscribe(
        (data: any) => {
          this.dataSource = new MatTableDataSource(data.items);
          this.dataSource.sort = this.sort;
          this.totalRow = data.totalCount;
          this.preventAbuse = false;
        },
        (err) => {
          this.translateService
            .get('messageSystem.loadFail')
            .subscribe((data) => (MessageConstants.GET_FAILSE_MSG = data));
          this.notificationService.printErrorMessage(
            MessageConstants.GET_FAILSE_MSG
          );
          this.preventAbuse = false;
        }
      );
  }

  get control() {
    return this.employeeForm.controls;
  }
  //search
  applyFilter(event: Event) {
    //const filterValue = (event.target as HTMLInputElement).value;
    this.keyword = (event.target as HTMLInputElement).value;
    this.page = 0;
    this.loadData();
    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  get getValidForm(): { [key: string]: AbstractControl } {
    return this.employeeForm.controls;
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
    this.loadData();
  }

//getall department
getDepartments() {
  this.dataService.get('TDepartment/getall').subscribe(
    (data: any) => {
      this.departments = data;
    },
    (err) => { }
  );
}

//getall chức vụ regency
getRegencys() {
  this.dataService.get('TRegency/getall').subscribe(
    (data: any) => {
      this.regencys = data;
    },
    (err) => { }
  );
}

getEmployeeTypes() {
  this.dataService.get('TEmployee/getEmployeeType').subscribe(
    (data: any) => {
      this.emtypes = data;
    },
    (err) => { }
  );
}

getZones() {
  this.dataService.get('AZone/getall').subscribe(
    (data: any) => {
      this.zones = data;
    },
    (err) => { }
  );
}

//open dialog add or edit
openDialog(action: string, item?: any, config?: MatDialogConfig) {
  this.action = action;
  if (action == 'create') {
    this.translateService.get('TEmployeeModule.createMessage').subscribe(data => this.title = data);
    this.employeeForm.controls['emStatus'].setValue(true);
    this.employeeForm.controls['emGender'].setValue("M");
  }
  else {
    this.translateService.get('TEmployeeModule.editMessage').subscribe(data => this.title = data);
    this.dataService.get('TEmployee/getbyid/' + item.emId).subscribe(
      (data: any) => {
        let item = data;
        var date = this.datepipe.transform(item.emBirthdate, 'yyyy-MM-dd');
        this.employeeForm.controls['emId'].setValue(item.emId);
        this.employeeForm.controls['emName'].setValue(item.emName);
        this.employeeForm.controls['emCode'].setValue(item.emCode);
        this.employeeForm.controls['depId'].setValue(item.depId);
        this.employeeForm.controls['regId'].setValue(item.regId);
        this.employeeForm.controls['emTypeId'].setValue(item.emTypeId);
        this.employeeForm.controls['emGender'].setValue(item.emGender);
        this.employeeForm.controls['emBirthdate'].setValue(item.emBirthdate);
        this.employeeForm.controls['emIdentityNumber'].setValue(item.emIdentityNumber);
        this.employeeForm.controls['emAddress'].setValue(item.emAddress);
        this.employeeForm.controls['description'].setValue(item.description);
        this.employeeForm.controls['emPhone'].setValue(item.emPhone);
        this.employeeForm.controls['emEmail'].setValue(item.emEmail);
        this.employeeForm.controls['emImage'].setValue(item.emImage);
        this.employeeForm.controls['emStatus'].setValue(item.emStatus);
        this.employeeForm.controls['placeId'].setValue(item.placeId);
        this.employeeForm.controls['zonId'].setValue(item.zonId);
      });
  }
  const dialogRef = this.dialog.open(this.dialogTemplate, { width: '750px' });
  dialogRef.disableClose = true;
  dialogRef.afterClosed().subscribe(result => {
    this.onReset();
  })
}


onReset() {
  // this.parentName=undefined;
  this.action == '';
  this.dialog.closeAll();
  this.employeeForm.reset();
}


//Save change data
addData(){
  if(this.employeeForm.invalid){
    return;
  }
  this.preventAbuse=true;
  if(this.action=='create'){
    this.spinner.show();
    let data = {emName:this.employeeForm.controls['emName'].value,
                emCode:this.employeeForm.controls['emCode'].value,
                depId:this.employeeForm.controls['depId'].value,
                regId:this.employeeForm.controls['regId'].value,
                emTypeId:this.employeeForm.controls['emTypeId'].value,
                emGender:this.employeeForm.controls['emGender'].value,
                emBirthdate:this.employeeForm.controls['emBirthdate'].value,
                emIdentityNumber:this.employeeForm.controls['emIdentityNumber'].value,
                emAddress:this.employeeForm.controls['emAddress'].value,
                description:this.employeeForm.controls['description'].value,
                emPhone:this.employeeForm.controls['emPhone'].value,
                emEmail:this.employeeForm.controls['emEmail'].value,
                emImage:this.employeeForm.controls['emImage'].value,
                emStatus:this.employeeForm.controls['emStatus'].value,
                placeId:this.employeeForm.controls['placeId'].value,
                zonId:this.employeeForm.controls['zonId'].value};
    this.dataService.post('TEmployee/create',data).subscribe(data=>{
      let message!:string;
      this.translateService.get('messageSystem.createSuccess').subscribe(mes=>message=mes);
      this.notificationService.printSuccessMessage(message);
      this.loadData();
      this.getDepartments();
      this.getEmployeeTypes();
      this.getRegencys();
      this.getEmployeeTypes();
      this.preventAbuse = false;
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
    let dto = this.employeeForm.value;
    this.dataService.put('TEmployee/update' , dto).subscribe(data=>{
      let message!:string;
      this.translateService.get('messageSystem.updateSuccess').subscribe(mes=>message=mes);
      this.notificationService.printSuccessMessage(message);
      this.loadData();
      this.getDepartments();
      this.getEmployeeTypes();
      this.getRegencys();
      this.getEmployeeTypes();
      this.preventAbuse = false;
      this.spinner.hide();
    },err=>{
      let error!:string;
      this.translateService.get('messageSystem.updateFail').subscribe(mes=>error=mes);
      this.notificationService.printErrorMessage(err);
      this.spinner.hide();
    })
  }
  this.preventAbuse=false;
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

removeData() {
  let roleChecked: any[] = [];
  console.log(this.selection.selected)
  this.selection.selected.forEach((value: any) => {
    let id = { emId: value.emId, aliasID: value.emCode};
    roleChecked.push(id);
  });
  this.translateService.get('messageSystem.confirmDelete').subscribe(data => MessageConstants.CONFIRM_DELETE_MSG = data);
  this.notificationService.printConfirmationDialog(MessageConstants.CONFIRM_DELETE_MSG, () => this.deleteItemConfirm(JSON.stringify(roleChecked)));
}

deleteItemConfirm(id: string) {
  this.preventAbuse = true;
  this.spinner.show();
  this.dataService.delete('TEmployee/DeleteMuitiple', 'checkedList', id).subscribe((res: any) => {
    this.translateService.get('messageSystem.deleteSuccess').subscribe(data => MessageConstants.DELETED_OK_MSG = data);
    this.notificationService.printSuccessMessage(MessageConstants.DELETED_OK_MSG);
    this.selection.clear();
    this.loadData();
    this.spinner.hide();
  }, (err: any) => {
    this.preventAbuse = false;
    this.translateService.get('messageSystem.deletefail').subscribe(data => MessageConstants.DELETE_FAILSE_MSG = data);
    this.notificationService.printErrorMessage(MessageConstants.DELETE_FAILSE_MSG);
    this.spinner.hide();
  });
}

  // Image upload
  clearImage() {
    this.employeeForm.controls['emImage'].setValue('');
  }
  imagePreview(e: any) {
    const file = (e.target as HTMLInputElement).files![0];
    const reader = new FileReader();
    reader.onload = () => {
      let base64String = reader.result as string;
      let img = base64String.split('base64,')[1];
      this.employeeForm.controls['emImage'].setValue(img);
    };
    reader.readAsDataURL(file);
  }

}
