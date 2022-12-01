import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AuthenService } from 'src/app/core/services/authen.service';
import { DataService } from 'src/app/core/services/data.service';
import { NotificationService } from 'src/app/core/services/notification.service';
import { MaterialModule } from '../material/material.module';
import { MatRadioModule } from '@angular/material/radio';
import { UserRoleModule } from 'src/app/core/common/userRole.pipe';
import { PaginatorCustomService } from 'src/app/core/services/paginator-custom.service';

const router: Routes=[
  {path:'', component:HomeComponent}
]


@NgModule({
  declarations: [HomeComponent],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forChild(router),
    MaterialModule,
    MatRadioModule,
    UserRoleModule
  ]  ,
  exports:[HomeComponent],
  bootstrap:[HomeComponent],
  providers:[NotificationService, DataService, AuthenService, PaginatorCustomService, DatePipe],
  schemas:[CUSTOM_ELEMENTS_SCHEMA]
})
export class HomeModule { }
