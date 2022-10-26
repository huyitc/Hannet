import { DataService } from './../services/data.service';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { SystemConstants } from '../../core/common/system.constants';
import { UrlConstants } from '../../core/common/url.constants';

const user = JSON.parse(localStorage.getItem(SystemConstants.CURRENT_USER)!);
const menuUsers = JSON.parse(localStorage.getItem(SystemConstants.USER_MENUS)!);

@Injectable()
export class AuthGuard implements CanActivate {

  constructor(private router: Router, private httpClient: HttpClient) {

  }
  canActivate(activateRoute: ActivatedRouteSnapshot, routerState: RouterStateSnapshot) {
    // if (menuUsers?.length > 0) {
    //   // if (localStorage.getItem(SystemConstants.CURRENT_USER)) {
    //   //     return true;
    //   // }
    //   if (menuUsers.filter((x: any) => x.activeLink == routerState.url)[0] != undefined) {
    //     return true;

    //   }
    //   else {
    //     this.router.navigate([UrlConstants.LOGIN], {
    //       queryParams: {
    //         returnUrl: routerState.url
    //       }
    //     });
    //     return false;
    //   }
    // }
    // else {
    //   this.router.navigate([UrlConstants.LOGIN], {
    //     queryParams: {
    //       returnUrl: routerState.url
    //     }
    //   });
    //   return false;
    // }
    if (localStorage.getItem(SystemConstants.CURRENT_USER)) {
      return true;
    }
    else {
        this.router.navigate([UrlConstants.LOGIN], {
            queryParams: {
                returnUrl: routerState.url
            }
        });
        return false;
    }
  }
}
