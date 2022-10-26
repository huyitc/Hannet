import { MaterialModule } from './../../material/material.module';
import { DataService } from 'src/app/core/services/data.service';
import { NotificationService } from 'src/app/core/services/notification.service';
import { AppGroupComponent } from './app-group.component';
import { Router, Routes, RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { NgProgressHttpModule } from 'ngx-progressbar/http';
import { NgProgressModule } from 'ngx-progressbar';
import { UserRoleModule } from 'src/app/core/common/userRole.pipe';
import { CommonModule } from '@angular/common';

const routes: Routes=[{path:'',component:AppGroupComponent}]

@NgModule({
  declarations: [AppGroupComponent],
  imports: [
    CommonModule,
    NgProgressModule,
    NgProgressHttpModule,
    MaterialModule,
    UserRoleModule,
    RouterModule.forChild(routes)
  ],
  providers:[NotificationService,DataService]
})
export class AppGroupModule { }
