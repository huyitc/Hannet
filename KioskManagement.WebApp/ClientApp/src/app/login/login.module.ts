import { TranslateService, TranslateModule } from '@ngx-translate/core';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { LoginComponent } from './login.component';
import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { NotificationService } from '../core/services/notification.service';
import { AuthenService } from '../core/services/authen.service';
import { HttpClientModule } from '@angular/common/http';
import { DataService } from '../core/services/data.service';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatButtonModule} from '@angular/material/button';
import { NgProgressModule } from "ngx-progressbar";
import { NgProgressHttpModule } from "ngx-progressbar/http";
import { UserRoleModule } from '../core/common/userRole.pipe';

const routers: Routes = [
  { path: '', component: LoginComponent }
]

@NgModule({
  declarations: [LoginComponent],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    UserRoleModule,
    RouterModule.forChild(routers),
    HttpClientModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    MatButtonModule,
    NgProgressModule.withConfig({spinnerPosition: "right",
    color: "#red"}),
    NgProgressHttpModule,
    TranslateModule
  ],
  providers:[AuthenService, NotificationService, DataService, TranslateService],
  schemas:[CUSTOM_ELEMENTS_SCHEMA],
  exports:[]
})
export class LoginModule { }
