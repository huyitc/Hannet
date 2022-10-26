import { MatTreeModule } from '@angular/material/tree';
import { DataService } from 'src/app/core/services/data.service';
import { AuthGuard } from './../core/guards/auth.guard';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { UltillityService } from './../core/services/ultillity.service';
import {HttpClientModule } from '@angular/common/http';
import { NgModule, NO_ERRORS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';
import { mainRoutes } from './main.routes';
import { RouterModule } from '@angular/router';
import { MainComponent } from './main.component';
import { FormsModule } from '@angular/forms';
import { AuthenService } from '../core/services/authen.service';
import { MatToolbarModule } from '@angular/material/toolbar';
import {MatSidenavModule} from '@angular/material/sidenav';
import { FlexLayoutModule } from "@angular/flex-layout";
import { MatDividerModule } from '@angular/material/divider';
import { MatMenuModule } from '@angular/material/menu';
import { LayoutModule } from '@angular/cdk/layout';
import { MatListModule } from '@angular/material/list';
import { NgProgressModule } from 'ngx-progressbar';
import { NgProgressHttpModule } from 'ngx-progressbar/http';
import { NotificationService } from '../core/services/notification.service';
import {MatSlideToggleModule} from '@angular/material/slide-toggle';
import { MaterialModule } from './material/material.module';
import { TranslateService } from '@ngx-translate/core';
import { UserRoleModule } from '../core/common/userRole.pipe';
import { AScheduleDeviceDetailComponent } from './category/a-schedule-device-detail/a-schedule-device-detail.component';



@NgModule({
  declarations: [MainComponent],
  imports: [
    CommonModule,
    UserRoleModule,
    MaterialModule,
    HttpClientModule,
    FormsModule,
    MatToolbarModule,
    MatButtonModule,
    MatIconModule,
    MatSidenavModule,
    FlexLayoutModule,
    MatDividerModule,
    MatMenuModule,
    LayoutModule,
    MatListModule,
    MatSlideToggleModule,
    MatTreeModule,
    NgProgressModule.withConfig({spinnerPosition: "right",
    color: "#red"}),
    NgProgressHttpModule,
    RouterModule.forChild(mainRoutes)
  ],

  providers:[UltillityService, AuthenService, AuthGuard,DataService,NotificationService,TranslateService],
  schemas:[NO_ERRORS_SCHEMA]
})
export class MainModule { }
