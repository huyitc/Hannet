import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AZoneComponent } from './a-zone.component';
import { RouterModule, Routes } from '@angular/router';
import { NgProgressModule } from 'ngx-progressbar';
import { MaterialModule } from '../../material/material.module';

const routes: Routes=[{path:'',component:AZoneComponent}]

@NgModule({
  declarations: [AZoneComponent],
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    NgProgressModule,
    MaterialModule
  ]
})
export class AZoneModule { }
