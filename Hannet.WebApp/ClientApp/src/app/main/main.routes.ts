import { Routes } from '@angular/router';
import { MainComponent } from './main.component';

export const mainRoutes: Routes = [
  {
    path: '', component: MainComponent, children: [
      {
        path: '', redirectTo: 'home', pathMatch: 'full',
        data: {
          title: 'Trang chủ',
          breadcrumb: [
            {
              label: 'Trang chủ',
              url: ''
            }
          ]
        },
      },
      {
        path: 'home', loadChildren: () => import("./home/home.module").then(m => m.HomeModule),

      },
      {
        path: 'app-group', loadChildren: () => import("./system/app-group/app-group.module").then(m => m.AppGroupModule),
      },
      {
        path: 'app-role', loadChildren: () => import("./system/app-role/app-role.module").then(m => m.AppRoleModule),

      },
      {path:'app-user',loadChildren:()=>import('./system/app-user/app-user.module').then(x=>x.AppUserModule)},
      {path:'app-menu',loadChildren:()=>import('./system/app-menu/app-menu.module').then(x=>x.AppMenuModule)},
      {path:'t-department',loadChildren:()=>import('./category/t-department/t-department.module').then(x=>x.TDepartmentModule)},
      {path:'t-regency',loadChildren:()=>import('./category/t-regency/t-regency.module').then(x=>x.TRegencyModule)},
      {path:'t-device',loadChildren:()=>import('./category-device/t-device/t-device.module').then(x=>x.TDeviceModule)},
      {path:'a-zone',loadChildren:()=>import('./category-device/a-zone/a-zone.module').then(x=>x.AZoneModule)},
      {path:'t-devicetype',loadChildren:()=>import('./category-device/t-devicetype/t-devicetype.module').then(x=>x.TDevicetypeModule)},
      {path:'t-groupaccess',loadChildren:()=>import('./category-device/t-groupaccess/t-groupaccess.module').then(x=>x.TGroupaccessModule)},
      {path:'t-employee',loadChildren:()=>import('./manage/t-employee/t-employee.module').then(x=>x.TEmployeeModule)},
      {path:'a-schedule-device-detail',loadChildren:()=>import('./category/a-schedule-device-detail/a-schedule-device-detail.module').then(x=>x.AScheduleDeviceDetailModule)},
      {path:'check-in-by-day',loadChildren:()=>import('./manage/check-in-by-day/check-in-by-day.module').then(x=>x.CheckInByDayModule)},
    ]
  }
];
