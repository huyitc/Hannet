import { TranslateService } from '@ngx-translate/core';
import { Injectable } from '@angular/core';
declare var alertify: any;
@Injectable()
export class NotificationService {
  private _notifier: any = alertify;
  constructor(private translateService: TranslateService) {
    let btnOklabel = '';
    this.translateService.get('button.confirm').subscribe(data => btnOklabel = data);
    alertify.defaults.glossary.ok = btnOklabel;
    let btnCancellabel = '';
    this.translateService.get('button.cancel').subscribe(data => btnCancellabel = data);
    alertify.defaults.glossary.cancel  = btnCancellabel;
    let notificationTitle = '';
    this.translateService.get('messageSystem.notification').subscribe(data => notificationTitle = data);
    alertify.defaults.glossary.title = notificationTitle;
    alertify.defaults.theme.ok = "mat-raised-button mat-primary";
    alertify.defaults.theme.cancel = "mat-raised-button mat-warn";
    alertify.set('notifier','position', 'top-right');
  }


  printSuccessMessage(message: string) {
    this._notifier.success(message);
  }

  printErrorMessage(message: string) {
    this._notifier.error(message);
  }


  printConfirmationDialog(message: string, okCallback: () => any) {
    this._notifier.confirm(message, function(){ okCallback() }, function(){ })
    // this._notifier.confirm().set({message, function (e: any) {
    //   if (e) {
    //     okCallback();
    //   } else {
    //   }
    // }});
  }

}
