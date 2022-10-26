import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { NgProgressHttpModule } from 'ngx-progressbar/http';
import { NgProgressModule } from 'ngx-progressbar';
import { TranslateModule } from '@ngx-translate/core';
import { MaterialModule } from '../../material/material.module';
import { AppUserComponent } from './app-user.component';

const routes:Routes=[{path:'',component:AppUserComponent}]


@NgModule({
  declarations: [AppUserComponent],
  imports: [
    CommonModule,
    MaterialModule,
    NgProgressModule.withConfig({spinnerPosition: "right",
    color: "#red"}),
    NgProgressHttpModule,
    TranslateModule,
    RouterModule.forChild(routes)
  ],
  schemas:[CUSTOM_ELEMENTS_SCHEMA]
})
export class AppUserModule { }
