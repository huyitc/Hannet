import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TDeviceComponent } from './t-device.component';
import { RouterModule, Routes } from '@angular/router';
import { NgProgressModule } from 'ngx-progressbar';
import { MaterialModule } from '../../material/material.module';

const routes: Routes=[{path:'',component:TDeviceComponent}]

@NgModule({
  declarations: [TDeviceComponent],
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    NgProgressModule,
    MaterialModule
  ]
})
export class TDeviceModule { }
