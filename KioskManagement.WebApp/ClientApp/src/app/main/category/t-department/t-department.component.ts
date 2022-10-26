import { SelectionModel } from '@angular/cdk/collections';
import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { FormGroup, FormControl, FormBuilder, Validators, AbstractControl } from '@angular/forms';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { TranslateService } from '@ngx-translate/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { Observable } from 'rxjs';
import { startWith, map } from 'rxjs/operators';
import { MessageConstants } from 'src/app/core/common/message.constants';
import { DataService } from 'src/app/core/services/data.service';
import { NotificationService } from 'src/app/core/services/notification.service';
import { PaginatorCustomService } from 'src/app/core/services/paginator-custom.service';

@Component({
  selector: 'app-t-department',
  templateUrl: './t-department.component.html',
  styleUrls: ['./t-department.component.css']
})
export class TDepartmentComponent implements OnInit {
  displayedColumns: string[] = ['select','position', 'departmentName','depDescription','depStatus', 'action',];
  dataSource =new MatTableDataSource<any>();
  preventAbuse=false;
  page=0;
  keyword:string='';
  totalRow:number=0;
  totalPage:number=0;
  depParents:any;
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  @ViewChild('dialog') dialogTemplate!: TemplateRef<any>;
  selection = new SelectionModel<any>(true, []);
  departmentForm!: FormGroup;
  action: string = '';
  title: string = '';
  pageSizeOptions: number[] = [10, 25, 50, 100];
  pageSize=this.pageSizeOptions[0];
  isAllChecked=false;
  parentName:any;
  filteredOptions!: Observable<any[]>;
  users: any;
  DepartmentParentControl = new FormControl();
  constructor(private spinner: NgxSpinnerService,private dataService:DataService,public dialog:MatDialog,
    private notificationService:NotificationService,private pagin:PaginatorCustomService,
    private translateService:TranslateService,private formBuilder:FormBuilder) 
    { 
      this.departmentForm=this.formBuilder.group({
        depId:'',
        depName:['',Validators.compose([Validators.required,Validators.maxLength(100)])],
        depDescription:[''],
        depStatus:'',
      })
      this.filteredOptions = this.DepartmentParentControl.valueChanges.pipe(
        startWith(''),
        map(value => (typeof value === 'string' ? value : value)),
        map(name => (name ? this._filter(name) : this.depParents.slice())),
      );
    }

  ngOnInit(): void {
    this.getAllDepartments();
    this.getDepartments();
  }
 //option with depDescription
 private _filter(value: any): any[] {
  const filterValue = value?.toLowerCase();
  return this.depParents.filter((option: any) => option?.depDescription.toLowerCase().includes(filterValue));
}

//open dialog add or edit
openDialog(action:string,item?:any,config?:MatDialogConfig){
  this.action=action;
  if(action=='create'){
    this.translateService.get('pDepartmentModule.createMessage').subscribe(data=>this.title=data);
    this.departmentForm.controls['depStatus'].setValue(true);
  }
  else{
    this.translateService.get('pDepartmentModule.editMessage').subscribe(data=>this.title=data);
    this.departmentForm.controls['depId'].setValue(item.depId);
    let itemFilter=this.dataSource.filteredData.filter((x:any)=>x.depId==item.depId)[0];
    this.departmentForm.controls['depName'].setValue(itemFilter.depName);
    this.departmentForm.controls['depDescription'].setValue(itemFilter.depDescription);
    this.departmentForm.controls['depStatus'].setValue(itemFilter.depStatus);
    let parent = this.depParents.filter((x:any)=>x.depId==item.depParentId)[0];
    this.parentName=parent?.depDescription;
    this.DepartmentParentControl.setValue(this.parentName);
  }
  const dialogRef = this.dialog.open(this.dialogTemplate,{width:'650px'});
  dialogRef.disableClose = true;
  dialogRef.afterClosed().subscribe(result=>{
    this.onReset();
  })
 }

 //getall department
getDepartments(){
  this.dataService.get('TDepartment/getall').subscribe((data:any)=>{
    this.depParents=data;
  },err=>{

  });
}

//getallDepartments by paging
getAllDepartments(){
  this.preventAbuse=true;
  this.dataService.get('TDepartment/GetAllByPaging?page='+this.page+"&pageSize="+this.pageSize+"&keyword="+this.keyword).subscribe((data:any)=>{
    this.dataSource= new MatTableDataSource(data.items) ;
    this.dataSource.sort = this.sort;
    this.totalRow=data.totalCount;
  },err=>{
    this.translateService.get('messageSystem.loadFail').subscribe(data=>MessageConstants.GET_FAILSE_MSG=data);
    this.notificationService.printErrorMessage(MessageConstants.GET_FAILSE_MSG);
  });
  this.preventAbuse=false;
}

//search
applyFilter(event: Event) {
  //const filterValue = (event.target as HTMLInputElement).value;
  this.keyword= (event.target as HTMLInputElement).value;
  this.page=0;
  this.getAllDepartments();
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
  return this.departmentForm.controls;
 }

//get depParentId theo depId
getParentId(event:any){
  this.departmentForm.controls['depParentId'].setValue(event.depId);
}

//Save change data
addData(){
  if(this.departmentForm.invalid){
    return;
  }
  this.preventAbuse=true;
  if(this.action=='create'){
    this.spinner.show();
    let department={depName:this.departmentForm.controls['depName'].value,depDescription:this.departmentForm.controls['depDescription'].value,depStatus:this.departmentForm.controls['depStatus'].value};
    this.dataService.post('TDepartment/create',department).subscribe(data=>{
      let message!:string;
      this.translateService.get('messageSystem.createSuccess').subscribe(mes=>message=mes);
      this.notificationService.printSuccessMessage(message);
      this.getAllDepartments();
      this.getDepartments();
      this.onReset();
      this.spinner.hide();
    },err=>{
      let error!:string;
      this.translateService.get('messageSystem.createFail').subscribe(mes=>error=mes);
      this.notificationService.printErrorMessage(error);
      this.spinner.hide();
    });
  }
  else if(this.action=='edit'){
    this.spinner.show();
    let department={depName:this.departmentForm.controls['depName'].value,depId:this.departmentForm.controls['depId'].value,depDescription:this.departmentForm.controls['depDescription'].value,depStatus:this.departmentForm.controls['depStatus'].value};
    this.dataService.put('TDepartment/update' , department).subscribe(data=>{
      let message!:string;
      this.translateService.get('messageSystem.updateSuccess').subscribe(mes=>message=mes);
      this.notificationService.printSuccessMessage(message);
      this.getAllDepartments();
      this.getDepartments();
      this.dialog.closeAll();
      this.onReset();
      this.spinner.hide();
    },err=>{
      let error!:string;
      this.translateService.get('messageSystem.updateFail').subscribe(mes=>error=mes);
      this.notificationService.printErrorMessage(error);
      this.spinner.hide();
    })
  }
  this.onReset();
  this.preventAbuse=false;
}

//reset form
onReset(){
  // this.parentName=undefined;
   this.action=='';
   this.departmentForm.reset();
 }

 //remove muilti data
 removeData(){
  let departmentChecked: any[]=[];
  this.selection.selected.forEach((value:any)=>{
    let id= value.depId;
    departmentChecked.push(id);
  });
  this.translateService.get('messageSystem.confirmDelete').subscribe(data=>MessageConstants.CONFIRM_DELETE_MSG=data);
  this.notificationService.printConfirmationDialog(MessageConstants.CONFIRM_DELETE_MSG, () => this.deleteItemConfirm(JSON.stringify(departmentChecked)));
}

 deleteItemConfirm(depId: string) {
  this.preventAbuse=true;
  this.spinner.show();
  this.dataService.delete('TDepartment/deletemulti', 'checkedList', depId).subscribe((response:any) => {
    this.translateService.get('messageSystem.deleteSuccess').subscribe(data => MessageConstants.DELETED_OK_MSG = data);
    this.translateService.get('messageSystem.deleteFail').subscribe(data => MessageConstants.DELETE_FAILSE_MSG = data);
    if (response[0] > 0) {
      this.notificationService.printSuccessMessage(MessageConstants.DELETED_OK_MSG + response[0]);
    }
    if (response[1] > 0) {
      this.notificationService.printErrorMessage(MessageConstants.DELETE_FAILSE_MSG + response[1]);
    }
    this.selection.clear();
    this.getAllDepartments();
    this.getDepartments();
    this.spinner.hide();
  }, err => {
    this.translateService.get('messageSystem.deletefail').subscribe(data=>MessageConstants.DELETE_FAILSE_MSG=data);
    this.notificationService.printErrorMessage(MessageConstants.DELETE_FAILSE_MSG);
    this.spinner.hide();
  });
  this.preventAbuse=false;
}

//paging
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
  this.getAllDepartments();
}
}
