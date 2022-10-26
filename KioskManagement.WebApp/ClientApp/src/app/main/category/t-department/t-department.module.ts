import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TDepartmentComponent } from './t-department.component';
import { RouterModule, Routes } from '@angular/router';
import { NgProgressModule } from 'ngx-progressbar';
import { NgProgressHttpModule } from 'ngx-progressbar/http';
import { UserRoleModule } from 'src/app/core/common/userRole.pipe';
import { MaterialModule } from '../../material/material.module';

const routes:Routes=[{path:'',component:TDepartmentComponent}]

@NgModule({
  declarations: [TDepartmentComponent],
  imports: [
    CommonModule,
    MaterialModule,
    UserRoleModule,
    NgProgressModule.withConfig({spinnerPosition: "right",
    color: "#red"}),
    NgProgressHttpModule,
    RouterModule.forChild(routes)
  ],
  schemas:[CUSTOM_ELEMENTS_SCHEMA],
})
export class TDepartmentModule { }
