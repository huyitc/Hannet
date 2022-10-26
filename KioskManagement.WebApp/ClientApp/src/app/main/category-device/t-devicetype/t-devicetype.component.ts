import { SelectionModel } from '@angular/cdk/collections';
import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { TranslateService } from '@ngx-translate/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { MessageConstants } from 'src/app/core/common/message.constants';
import { DataService } from 'src/app/core/services/data.service';
import { NotificationService } from 'src/app/core/services/notification.service';
import { PaginatorCustomService } from 'src/app/core/services/paginator-custom.service';

@Component({
  selector: 'app-t-devicetype',
  templateUrl: './t-devicetype.component.html',
  styleUrls: ['./t-devicetype.component.css']
})
export class TDevicetypeComponent implements OnInit {

  displayedColumns: string[] = ['select', 'position', 'devtypeName', 'devtypeCode', 'action',];
  dataSource = new MatTableDataSource<any>();
  preventAbuse = false;
  page = 0;
  keyword: string = '';
  totalRow: number = 0;
  totalPage: number = 0;
  devtypeParents: any;
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  @ViewChild('dialog') dialogTemplate!: TemplateRef<any>;
  selection = new SelectionModel<any>(true, []);
  devicetypeForm!: FormGroup;
  action: string = '';
  title: string = '';
  pageSizeOptions: number[] = [10, 25, 50, 100];
  pageSize = this.pageSizeOptions[0];
  isAllChecked = false;
  parentName: any;
  filteredOptions!: Observable<any[]>;
  users: any;
  DevicetypeParentControl = new FormControl();


  constructor(private dataService: DataService, public dialog: MatDialog,
    private notificationService: NotificationService, private pagin: PaginatorCustomService,
    private translateService: TranslateService, private formBuilder: FormBuilder) {

      this.devicetypeForm = this.formBuilder.group({
        devTypeId: '',
        devTypeName: ['', Validators.compose([Validators.required, Validators.maxLength(50)])],
        devTypeCode: ['', Validators.compose([Validators.required, Validators.maxLength(10)])],
      })
  }

  ngOnInit(): void {
    this.getTdevicetypetListPaging();
  }
  getTdevicetypetListPaging() {
    this.preventAbuse = true;
    this.dataService.get('TDeviceType/getlistpaging?page=' + this.page + "&pageSize=" + this.pageSize + "&keyword=" + this.keyword).subscribe((data: any) => {
      this.dataSource = new MatTableDataSource(data.items);
      this.dataSource.sort = this.sort;
      this.totalRow = data.totalCount;
      this.preventAbuse = false;
    }, err => {
      this.preventAbuse = false;
      this.translateService.get('messageSystem.loadFail').subscribe(data => MessageConstants.GET_FAILSE_MSG = data);
      this.notificationService.printErrorMessage(MessageConstants.GET_FAILSE_MSG);
    });
  }

  //open dialog add or edit
  openDialog(action: string, item?: any, config?: MatDialogConfig) {
    this.action = action;
    if (action == 'create') {
      this.translateService.get('tDeviceTypeModule.createMessage').subscribe(data => this.title = data);
    }
    else {
      this.translateService.get('tDeviceTypeModule.editMessage').subscribe(data => this.title = data);
      this.devicetypeForm.controls['devTypeId'].setValue(item.devTypeId);
      let itemFilter = this.dataSource.filteredData.filter((x: any) => x.devTypeId == item.devTypeId)[0];
      this.devicetypeForm.controls['devTypeName'].setValue(itemFilter.devTypeName);
      this.devicetypeForm.controls['devTypeCode'].setValue(itemFilter.devTypeCode);
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
    this.dialog.closeAll();
    this.devicetypeForm.reset();

  }

  //search
  applyFilter(event: Event) {
    //const filterValue = (event.target as HTMLInputElement).value;
    this.keyword = (event.target as HTMLInputElement).value;
    this.page = 0;
    this.getTdevicetypetListPaging();
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
    return this.devicetypeForm.controls;
  }

  //get devParentId theo devId
  getParentId(event:any){
    this.devicetypeForm.controls['devTypeParentId'].setValue(event.devTypeId);
  }

   //Save change data
   addData(){
    if(this.devicetypeForm.invalid){
      return;
    }
    this.preventAbuse=true;
    if(this.action=='create'){
      let deviceType={devTypeName:this.devicetypeForm.controls['devTypeName'].value,devTypeCode:this.devicetypeForm.controls['devTypeCode'].value};
      this.dataService.post('TDeviceType/create',deviceType).subscribe(data=>{
        let message!:string;
        this.translateService.get('messageSystem.createSuccess').subscribe(mes=>message=mes);
        this.notificationService.printSuccessMessage(message);
        this.getTdevicetypetListPaging();
        this.dialog.closeAll();
        this.onReset();
      },err=>{
        this.preventAbuse=false;
        let error!:string;
        this.translateService.get('messageSystem.createFail').subscribe(mes=>error=mes);
        this.notificationService.printErrorMessage(err);
      });
    }
    else if(this.action=='edit'){
      let deviceType={devTypeName:this.devicetypeForm.controls['devTypeName'].value,devTypeId:this.devicetypeForm.controls['devTypeId'].value,devTypeCode:this.devicetypeForm.controls['devTypeCode'].value};
      this.dataService.put('TDeviceType/update' , deviceType).subscribe(data=>{
        let message!:string;
        this.translateService.get('messageSystem.updateSuccess').subscribe(mes=>message=mes);
        this.notificationService.printSuccessMessage(message);
        this.getTdevicetypetListPaging();
        this.dialog.closeAll();
        this.onReset();
      },err=>{
        this.preventAbuse=false;
        let error!:string;
        this.translateService.get('messageSystem.updateFail').subscribe(mes=>error=mes);
        this.notificationService.printErrorMessage(err);
      })
    }
    this.onReset();
  }

  //remove muilti data
  removeData(){
    let deviceTypeChecked: any[]=[];
    this.selection.selected.forEach((value:any)=>{
      let id= value.devTypeId;
      deviceTypeChecked.push(id);
    });
    this.translateService.get('messageSystem.confirmDelete').subscribe(data=>MessageConstants.CONFIRM_DELETE_MSG=data);
    this.notificationService.printConfirmationDialog(MessageConstants.CONFIRM_DELETE_MSG, () => this.deleteItemConfirm(JSON.stringify(deviceTypeChecked)));
  }

   deleteItemConfirm(devTypeId: string) {
    this.preventAbuse=true;
    this.dataService.delete('TDeviceType/deletemulti', 'checkedList',devTypeId).subscribe(response => {
      this.translateService.get('messageSystem.deleteSuccess').subscribe(data=>MessageConstants.DELETED_OK_MSG=data);
      this.notificationService.printSuccessMessage(MessageConstants.DELETED_OK_MSG);
      this.selection.clear();
      this.getTdevicetypetListPaging();
    }, err => {
      this.preventAbuse=false;
      this.translateService.get('messageSystem.deletefail').subscribe(data=>MessageConstants.DELETE_FAILSE_MSG=data);
      this.notificationService.printErrorMessage(MessageConstants.DELETE_FAILSE_MSG);
    });
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
    this.getTdevicetypetListPaging();
  }
}
