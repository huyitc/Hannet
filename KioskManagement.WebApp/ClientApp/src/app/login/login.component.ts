import { TranslateService } from '@ngx-translate/core';
import { MessageConstants } from './../core/common/message.constants';
import { UrlConstants } from './../core/common/url.constants';
import { AuthenService } from './../core/services/authen.service';
import { Component, OnInit } from '@angular/core';
import { NotificationService } from '../core/services/notification.service';
import { Router } from '@angular/router';
import { SystemConstants } from '../core/common/system.constants';
import { HttpClient } from '@angular/common/http';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  model: any ={};
  returlUrl: string |undefined;
  loginForm:any;
  preventAbuse = false;


  constructor(private authenService: AuthenService,private notificationService: NotificationService, private router: Router, private translate: TranslateService,private httpClient:HttpClient) {
  }


  login(){
    if(!this.validate()){
      return;
    }
    this.preventAbuse = true;
    this.authenService.login(this.model).subscribe(data => { this.router.navigate([UrlConstants.HOME]); this.preventAbuse = false; },
      err => {
        if (err.status === 401) {
          this.translate.get('messageSystem.incorrectAccount').subscribe(mes => MessageConstants.SYS_ERROR_LOGIN_FAILSE = mes)
          this.notificationService.printErrorMessage(MessageConstants.SYS_ERROR_LOGIN_FAILSE);
        }
        else {
          this.translate.get('messageSystem.serverNotConnect').subscribe(mes => MessageConstants.SYS_ERROR_LOGIN_FAILSE = mes)
          this.notificationService.printErrorMessage(MessageConstants.SYS_ERROR_LOGIN_FAILSE);
        }
        this.preventAbuse = false;

      });
  }

  validate():boolean{
    if(this.model.username==undefined){
      return false;
    }
    else if(this.model.password==undefined){
      return false;
    }
    else
    return true;
  }


  ngOnInit(): void {
    localStorage.removeItem(SystemConstants.CURRENT_USER);
    localStorage.removeItem(SystemConstants.CURRENT_USER_ROLE);
  }

}
