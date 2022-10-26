import { DataService } from 'src/app/core/services/data.service';
import { UltillityService } from './../core/services/ultillity.service';
import { Component, ElementRef, OnInit, ViewChild, AfterViewInit, ChangeDetectorRef, TemplateRef } from '@angular/core';
import { SystemConstants } from '../core/common/system.constants';
import { UrlConstants } from '../core/common/url.constants';
import { AuthenService } from '../core/services/authen.service';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { MatSidenav } from '@angular/material/sidenav';
import { MatBreadcrumbService } from 'mat-breadcrumb';
import { MatTreeFlatDataSource, MatTreeFlattener, MatTreeNestedDataSource } from '@angular/material/tree';
import { FlatTreeControl, NestedTreeControl } from '@angular/cdk/tree';
import { NotificationService } from '../core/services/notification.service';
import { MatDialog } from '@angular/material/dialog';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';

interface FoodNode {
  menuName: string;
  icon?:string;
  link?: string;
  childrens?: FoodNode[];
}

interface ExampleFlatNode {
  expandable: boolean;
  name: string;
  level: number;
  icon?: string;
  link?:string;
}


// const user=JSON.parse(localStorage.getItem(SystemConstants.CURRENT_USER)!) ;

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css']
})
export class MainComponent implements OnInit, AfterViewInit {

  account : any
  userInfo: any;
  @ViewChild(MatSidenav,{static:true}) sidenav!: MatSidenav;
  treeControl = new NestedTreeControl<FoodNode>(node => node.childrens);
  dataSource = new MatTreeNestedDataSource<FoodNode>();
  @ViewChild('modelChagePassWord') dialogTemplate!: TemplateRef<any>;
  form!: FormGroup;

  constructor(
    public _authen : AuthenService,
    private utilityService: UltillityService,
    private elementRef: ElementRef, private observer: BreakpointObserver, private ref: ChangeDetectorRef, private MatBreadcrumbService: MatBreadcrumbService,private dataService:DataService,
    private notificationService: NotificationService,public dialog: MatDialog,private translateService: TranslateService,private formBuilder: FormBuilder) 
    {
      this.form = this.formBuilder.group({
        passwordOld: ['', Validators.compose([Validators.required])],
        passwordNew:['', Validators.compose([Validators.required])],
        passwordNewRepeat:['', Validators.compose([Validators.required])],
      }
      )
      this.account = JSON.parse(localStorage.getItem(SystemConstants.CURRENT_USER) as string)

   }

   hasChild = (_: number, node: FoodNode) => !!node.childrens && node.childrens.length > 0;

  ngOnInit(): void {
    this.getUser();
    //sthis.getlistUser();
    this.loadMenuUsers();
  }

  loadMenuUsers(){
    const user=JSON.parse(localStorage.getItem(SystemConstants.CURRENT_USER)!) ;
    this.dataService.get('approles/gettreeviewbyuser?userId='+user.id).subscribe((data:any)=>{
      this.dataSource.data=data;
    });
  }

  logOut() {
    localStorage.removeItem(SystemConstants.CURRENT_USER);
    localStorage.removeItem(SystemConstants.CURRENT_USER_ROLE);
    localStorage.removeItem(SystemConstants.USERS_PIPE);
    localStorage.removeItem(SystemConstants.USER_MENUS);
    this.utilityService.navigate(UrlConstants.LOGIN);
  }

  getUser(){
    let current= localStorage.getItem(SystemConstants.CURRENT_USER);
    this.userInfo=JSON.parse(current!) ;
  }

  getlistUser(){
    this.dataService.get('appusers/getall').subscribe((data:any)=>{
      localStorage.removeItem(SystemConstants.USERS_PIPE);
      localStorage.setItem(SystemConstants.USERS_PIPE, JSON.stringify(data));
    });
  }

  ngAfterViewInit() {
     this.observer.observe(['(max-width:800px)']).subscribe((res)=>{
      if(res.matches){
        this.sidenav.mode='over';
        this.sidenav.close();
      }
      else{
        this.sidenav.mode='side';
        this.sidenav.open();
      }
    });
    this.ref.detectChanges();
  }

  //chage Password
  openModelChaglePassword(){
    const dialogRef = this.dialog.open(this.dialogTemplate, { width: '600px', autoFocus: false });
    dialogRef.disableClose = true;
  }
  closeModelChangepassword(){
    this.dialog.closeAll();
    this.form.reset();
  }

  change(){
    const oldpass = this.form.controls['passwordOld'].value;
    const newpass = this.form.controls['passwordNew'].value;
    const newPassRepeat = this.form.controls['passwordNewRepeat'].value;
    if (!this.form.invalid) {
      if (oldpass == newpass) {
        let error!:string;
        this.translateService.get('changePassWord.passOldCheck').subscribe(mes=>error=mes);
        this.notificationService.printErrorMessage(error);
        return
      }
      if (newpass != newPassRepeat) {
        let error!:string;
        this.translateService.get('changePassWord.passNewCheck').subscribe(mes=>error=mes);
        this.notificationService.printErrorMessage(error);
        return
      }
      // check mật khẩu cũ có đúng mật khẩu đang đăng nhập không
      this.dataService.get('Accounts/Checkpassword?userName=' + JSON.parse(localStorage.CURRENT_USER).userName + '&pass=' + oldpass)
        .subscribe((respon: any) => {
          var check = respon;
          if (check == false) {
            let error!:string;
            this.translateService.get('changePassWord.errorPassOld').subscribe(mes=>error=mes);
            this.notificationService.printErrorMessage(error);
            return;
          }
        })
        
      //end check mật khẩu
      //Đổi mật khẩu
      var param = {
        id: JSON.parse(localStorage.CURRENT_USER).id,
        passwordHash: newpass
      }
      this.dataService.put('AppUsers/UpdateFromUser', param)
        .subscribe((respon: any) => {
          let success!:string;
            this.translateService.get('changePassWord.changePassSuccess').subscribe(mes=>success=mes);
            this.notificationService.printSuccessMessage(success);
          this.dialog.closeAll();
          this.form.reset();
          this.utilityService.navigate(UrlConstants.LOGIN);
        },
          (err) => {
            err.error;
          })
      //end đổi mk
    }
  }

  get getValidForm(): { [key: string]: AbstractControl } {
    return this.form.controls;
  }
}
