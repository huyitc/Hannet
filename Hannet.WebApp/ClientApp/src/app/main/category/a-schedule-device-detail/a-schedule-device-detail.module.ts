import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AScheduleDeviceDetailComponent } from './a-schedule-device-detail.component';
import { Routes, RouterModule } from '@angular/router';
import { NgProgressModule } from 'ngx-progressbar';
import { MaterialModule } from '../../material/material.module';

const routes: Routes=[{path:'',component:AScheduleDeviceDetailComponent}]

@NgModule({
  declarations: [AScheduleDeviceDetailComponent],
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    NgProgressModule,
    MaterialModule
  ]
})
export class AScheduleDeviceDetailModule { }
