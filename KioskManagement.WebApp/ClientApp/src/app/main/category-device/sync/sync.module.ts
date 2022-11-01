import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SyncComponent } from './sync.component';
import { RouterModule, Routes } from '@angular/router';
import { NgProgressModule } from 'ngx-progressbar';
import { MaterialModule } from '../../material/material.module';

const routes: Routes=[{path:'',component:SyncComponent}]


@NgModule({
  declarations: [SyncComponent],
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    NgProgressModule,
    MaterialModule
  ]
})
export class SyncModule { }
