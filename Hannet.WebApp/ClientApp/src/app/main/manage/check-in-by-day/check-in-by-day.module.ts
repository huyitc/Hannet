import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { NgProgressModule } from 'ngx-progressbar';
import { MaterialModule } from '../../material/material.module';
import { CheckInByDayComponent } from './check-in-by-day.component';
import { DataService } from 'src/app/core/services/data.service';
import { NotificationService } from 'src/app/core/services/notification.service';
import { PaginatorCustomService } from 'src/app/core/services/paginator-custom.service';

const routes: Routes=[{path:'',component:CheckInByDayComponent}]

@NgModule({
  declarations: [CheckInByDayComponent],
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    NgProgressModule,
    MaterialModule
  ],
  providers:[DataService, NotificationService, PaginatorCustomService, DatePipe],
  schemas:[CUSTOM_ELEMENTS_SCHEMA]
})
export class CheckInByDayModule { }
