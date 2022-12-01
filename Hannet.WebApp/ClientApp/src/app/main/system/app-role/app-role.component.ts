
import { TranslateService } from '@ngx-translate/core';
import { MessageConstants } from 'src/app/core/common/message.constants';
import { NotificationService } from '../../../core/services/notification.service';
import { SystemConstants } from '../../../core/common/system.constants';
import { DataService } from '../../../core/services/data.service';
import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import {MatTableDataSource} from '@angular/material/table';
import {MatPaginator, PageEvent} from '@angular/material/paginator';
import {MatSort} from '@angular/material/sort';
import {SelectionModel} from '@angular/cdk/collections';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { PaginatorCustomService } from 'src/app/core/services/paginator-custom.service';
import { FormGroup, FormBuilder, Validators, AbstractControl, FormControl } from '@angular/forms';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { MatOptionSelectionChange } from '@angular/material/core';

const user=JSON.parse(localStorage.getItem(SystemConstants.CURRENT_USER)!) ;

@Component({
  selector: 'app-role',
  templateUrl: './app-role.component.html',
  styleUrls: ['./app-role.component.css']
})
export class AppRoleComponent implements OnInit {

  displayedColumns: string[] = ['select','position', 'name', 'description', 'createdDate', 'createdBy', 'action',];
  dataSource =new MatTableDataSource<any>() ;
  preventAbuse=false;
  page=0;
  keyword:string='';
  totalRow:number=0;
  totalPage:number=0;
  roleParents:any;
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  @ViewChild('dialog') dialogTemplate!: TemplateRef<any>;
  selection = new SelectionModel<any>(true, []);
  roleForm!: FormGroup;
  title!:string;
  action!:string;
  pageSizeOptions: number[] = [10, 25, 50, 100];
  pageSize=this.pageSizeOptions[0];
  isAllChecked=false;
  parentName:any;
  filteredOptions!: Observable<any[]>;
  users: any;
  roleParentControl = new FormControl();

  constructor(private dataService:DataService, public dialog: MatDialog, private notification: NotificationService,private pagin:PaginatorCustomService,private translateService:TranslateService, private formBuilder: FormBuilder) {
    this.roleForm=this.formBuilder.group({
      id:'',
      name:['', Validators.compose([Validators.required,Validators.maxLength(50)])],
      description:['',Validators.compose([Validators.required,Validators.maxLength(100)])],
      parentId:'',
      createdBy:'',
      updatedBy:'',
      deletedBy:'',
      createdDate:'',
      updatedDate:'',
      isDeleted:'',
      icon:['',Validators.compose([Validators.maxLength(100)])],
      link:['',Validators.compose([Validators.maxLength(100)])],
      activeLink:['',Validators.compose([Validators.maxLength(100)])],
      order_By:[''],
    });
    this.filteredOptions = this.roleParentControl.valueChanges.pipe(
      startWith(''),
      map(value => (typeof value === 'string' ? value : value)),
      map(name => (name ? this._filter(name) : this.roleParents.slice())),
    );
   }

  ngOnInit(): void {
    this.getAllRoles();
    this.getRoles();

  }

  private _filter(value: any): any[] {
    const filterValue = value?.toLowerCase();
    return this.roleParents.filter((option: any) => option?.description.toLowerCase().includes(filterValue));
  }

  openDialog(action:string,item?:any,config?: MatDialogConfig) {
    this.action=action;

    if(action=='create'){
      this.translateService.get('appRoleModule.createMessage').subscribe(data=>this.title=data);
    }
    else{
      this.translateService.get('appRoleModule.editMessage').subscribe(data=>this.title=data);
      this.roleForm.controls['id'].setValue(item.id);
      let itemFilter=this.dataSource.filteredData.filter((x:any)=>x.id==item.id)[0];
      this.roleForm.controls['name'].setValue(itemFilter.name);
      this.roleForm.controls['description'].setValue(itemFilter.description);
      this.roleForm.controls['parentId'].setValue(itemFilter.parentId);
      this.roleForm.controls['createdDate'].setValue(itemFilter.createdDate);
      this.roleForm.controls['createdBy'].setValue(itemFilter.createdBy);
      this.roleForm.controls['updatedDate'].setValue(itemFilter.updatedDate);
      this.roleForm.controls['updatedBy'].setValue(itemFilter.updatedBy);
      this.roleForm.controls['icon'].setValue(itemFilter.icon);
      this.roleForm.controls['link'].setValue(itemFilter.link);
      this.roleForm.controls['activeLink'].setValue(itemFilter.activeLink);
      this.roleForm.controls['order_By'].setValue(itemFilter.order_By);
      let parent= this.roleParents.filter((x:any)=>x.id==item.parentId)[0];
      this.parentName=parent?.description;
      this.roleParentControl.setValue(this.parentName);
    }
    const dialogRef = this.dialog.open(this.dialogTemplate, {
      width: '750px',
    });
    dialogRef.afterClosed().subscribe(result => {
    this.onReset();
    });
  }

  getRoles(){
    this.dataService.get('approles/getall').subscribe((data:any)=>{
      this.roleParents=data;
    }, err=>{
    });
  }

  getAllRoles(){
    this.preventAbuse=true;
    this.dataService.get('approles/getlistpaging?page='+this.page+"&pageSize="+this.pageSize+"&keyword="+this.keyword).subscribe((data:any)=>{
      this.dataSource= new MatTableDataSource(data.items) ;
      this.dataSource.sort = this.sort;
      this.totalRow=data.totalCount;
    }, err=>{
      this.translateService.get('messageSystem.loadFail').subscribe(data=>MessageConstants.GET_FAILSE_MSG=data);
      this.notification.printErrorMessage(MessageConstants.GET_FAILSE_MSG);
    });
    this.preventAbuse=false;
  }

  applyFilter(event: Event) {
    //const filterValue = (event.target as HTMLInputElement).value;
    this.keyword= (event.target as HTMLInputElement).value;
    this.page=0;
    this.getAllRoles();
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

  get getValidForm(): { [key: string]: AbstractControl } {
   return this.roleForm.controls;
  }

  getParentId(event:MatOptionSelectionChange, option:any){
    if(event.source.selected){
    this.roleForm.controls['parentId'].setValue(option.id);
    }
  }

  addData(){
    if(this.roleForm.invalid){
      return;
    }
    this.preventAbuse=true;
    if(this.action=='create'){
      let role= {name:this.roleForm.controls['name'].value, description:this.roleForm.controls['description'].value, parentId:this.roleForm.controls['parentId'].value!=''?this.roleForm.controls['parentId'].value:null,icon: this.roleForm.controls['icon'].value,link: this.roleForm.controls['link'].value,activeLink: this.roleForm.controls['activeLink'].value,order_By:this.roleForm.controls['order_By'].value,createdBy:user.id}
      this.dataService.post('approles/create', role).subscribe(data=>{
        let message!:string;
        this.translateService.get('messageSystem.createSuccess').subscribe(mes=>message=mes);
        this.notification.printSuccessMessage(message);
        this.getAllRoles();
        this.getRoles();
        this.dialog.closeAll();
        this.onReset();
      }, err=>{
        let error!:string;
        this.translateService.get('messageSystem.createFail').subscribe(mes=>error=mes);
        this.notification.printErrorMessage(err);
      });
    }
    else if(this.action=='edit'){
      let role= {name:this.roleForm.controls['name'].value, description:this.roleForm.controls['description'].value, parentId:this.roleForm.controls['parentId'].value!=''?this.roleForm.controls['parentId'].value:null,id:this.roleForm.controls['id'].value, updatedBy:user.id, createdDate:this.roleForm.controls['createdDate'].value, createdBy:this.roleForm.controls['createdBy'].value,icon: this.roleForm.controls['icon'].value,link: this.roleForm.controls['link'].value,activeLink: this.roleForm.controls['activeLink'].value,order_By:this.roleForm.controls['order_By'].value}
      this.dataService.put('approles/update', role).subscribe(data=>{
        let message!:string;
        this.translateService.get('messageSystem.updateSuccess').subscribe(mes=>message=mes);
        this.notification.printSuccessMessage(message);
        this.getAllRoles();
        this.getRoles();
        this.dialog.closeAll();
        this.onReset();
      }, err=>{
        let error!:string;
        this.translateService.get('messageSystem.updateFail').subscribe(mes=>error=mes);
        this.notification.printErrorMessage(err);
      });
    }


    this.onReset();
    this.preventAbuse=false;

  }

  onReset(){
   // this.parentName=undefined;
    this.action=='';
    this.dialog.closeAll();
    this.roleForm.reset();
    this.roleParentControl.reset();
  }



  removeData(){
    let roleChecked: any[]=[];
    this.selection.selected.forEach((value:any)=>{
      let id= value.id;
      roleChecked.push(id);
    });
    this.translateService.get('messageSystem.confirmDelete').subscribe(data=>MessageConstants.CONFIRM_DELETE_MSG=data);
    this.notification.printConfirmationDialog(MessageConstants.CONFIRM_DELETE_MSG, () => this.deleteItemConfirm(JSON.stringify(roleChecked)));
  }

  deleteItemConfirm(id: string) {
    this.preventAbuse=true;
    this.dataService.delete('approles/deletemulti', 'checkedList', id).subscribe(response => {
      this.translateService.get('messageSystem.deleteSuccess').subscribe(data=>MessageConstants.DELETED_OK_MSG=data);
      this.notification.printSuccessMessage(MessageConstants.DELETED_OK_MSG);
      this.selection.clear();
      this.getAllRoles();
      this.getRoles();
    }, err => {
      this.translateService.get('messageSystem.deletefail').subscribe(data=>MessageConstants.DELETE_FAILSE_MSG=data);
      this.notification.printErrorMessage(MessageConstants.DELETE_FAILSE_MSG);
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

  onChangePage(pe:PageEvent) {
    this.page=pe.pageIndex;
    this.pageSize=pe.pageSize;
    this.getAllRoles();
  }
}
