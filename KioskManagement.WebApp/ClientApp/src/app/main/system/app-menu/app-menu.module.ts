import { UserRoleModule } from 'src/app/core/common/userRole.pipe';
import { Routes, RouterModule } from '@angular/router';
import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppMenuComponent } from './app-menu.component';
import { MaterialModule } from './../../material/material.module';
import { NgProgressHttpModule } from 'ngx-progressbar/http';
import { NgProgressModule } from 'ngx-progressbar';

const routes:Routes=[{path:'',component:AppMenuComponent}]

@NgModule({
  declarations: [
    AppMenuComponent
  ],
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
export class AppMenuModule { }
