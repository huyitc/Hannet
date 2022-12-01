import { UserRoleModule } from 'src/app/core/common/userRole.pipe';
import { MaterialModule } from '../../material/material.module';
import { DataService } from '../../../core/services/data.service';
import { AppRoleComponent } from './app-role.component';
import { Routes, RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgProgressHttpModule } from 'ngx-progressbar/http';
import { NgProgressModule } from 'ngx-progressbar';
import { NotificationService } from 'src/app/core/services/notification.service';
import { PaginatorCustomService } from 'src/app/core/services/paginator-custom.service';

const routes:Routes=[{path:'',component:AppRoleComponent}]

@NgModule({
  declarations: [AppRoleComponent],
  imports: [
    CommonModule,
    MaterialModule,
    UserRoleModule,
    RouterModule.forChild(routes),
    NgProgressModule.withConfig({spinnerPosition: "right",
    color: "#red"}),
    NgProgressHttpModule,
  ],
  providers:[DataService, NotificationService, PaginatorCustomService]
})
export class AppRoleModule { }
