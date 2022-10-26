import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { TEmployeeComponent } from './t-employee.component';
import { MatRadioModule } from '@angular/material/radio';
import { MatTabsModule } from '@angular/material/tabs';
import { RouterModule, Routes } from '@angular/router';
import { NgProgressModule } from 'ngx-progressbar';
import { NgProgressHttpModule } from 'ngx-progressbar/http';
import { UserRoleModule } from 'src/app/core/common/userRole.pipe';
import { DataService } from 'src/app/core/services/data.service';
import { NotificationService } from 'src/app/core/services/notification.service';
import { PaginatorCustomService } from 'src/app/core/services/paginator-custom.service';
import { MaterialModule } from '../../material/material.module';

const routes:Routes=[{path:'',component:TEmployeeComponent}]

@NgModule({
  declarations: [TEmployeeComponent],
  imports: [
    CommonModule,
    MaterialModule,
    MatTabsModule,
    MatRadioModule,
    UserRoleModule,
    RouterModule.forChild(routes),
    NgProgressModule.withConfig({spinnerPosition: "right",
    color: "#red"}),
    NgProgressHttpModule
  ],
  providers:[DataService, NotificationService, PaginatorCustomService, DatePipe],
  schemas:[CUSTOM_ELEMENTS_SCHEMA]
})
export class TEmployeeModule { }
