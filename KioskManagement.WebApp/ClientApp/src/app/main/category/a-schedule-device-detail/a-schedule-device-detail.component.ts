import { filter } from 'rxjs/operators';
import { PaginatorCustomService } from './../../../core/services/paginator-custom.service';
import { FormGroup, FormBuilder, Validators, AbstractControl, FormControl } from '@angular/forms';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { SelectionModel } from '@angular/cdk/collections';
import { PageEvent, MatPaginator } from '@angular/material/paginator';
import { NotificationService } from '../../../core/services/notification.service';
import { TranslateService } from '@ngx-translate/core';
import { MatSort } from '@angular/material/sort';
import { MessageConstants } from '../../../core/common/message.constants';
import { DataService } from '../../../core/services/data.service';
import { MatTableDataSource } from '@angular/material/table';
import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
export class WeekdayDataModel {
  timeSlot!: string;
  index!: number;
  constructor(timeSlot: string, index: number) {
    this.timeSlot = timeSlot;
    this.index = index;
  }
}
export class Weekday {
  selectionWeekday!: SelectionModel<WeekdayDataModel>;
  dataSourceWeekday!: MatTableDataSource<WeekdayDataModel>;
  weekdayTitle!: string;
  displayedColumns!: string[];
  colorTitle: string;
  fxFlexOffset!: number;
  constructor(weekdayTitle: string, selectionWeekday: SelectionModel<WeekdayDataModel>,
    dataSourceWeekday: MatTableDataSource<WeekdayDataModel>, displayedColumns: string[], colorTitle: string, fxFlexOffset: number) {
    this.weekdayTitle = weekdayTitle;
    this.selectionWeekday = selectionWeekday;
    this.dataSourceWeekday = dataSourceWeekday;
    this.displayedColumns = displayedColumns;
    this.colorTitle = colorTitle;
    this.fxFlexOffset = fxFlexOffset;
  }
}

@Component({
  selector: 'app-a-schedule-device-detail',
  templateUrl: './a-schedule-device-detail.component.html',
  styleUrls: ['./a-schedule-device-detail.component.css']
})
export class AScheduleDeviceDetailComponent implements OnInit {
  displayedColumns: string[] = ['select', 'position', 'schDevId', 'schName', 'action'];
  displayedWeekdayColumns: string[] = ['timeSlot', 'select'];
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  weekdays: Weekday[] = [];

  public weekdayData: WeekdayDataModel[] = [{ timeSlot: "00: 00 - 00: 14", index: 0 },
  { timeSlot: "00: 15 - 00: 29", index: 1 },
  { timeSlot: "00: 30 - 00: 44", index: 2 },
  { timeSlot: "00: 45 - 00: 59", index: 3 },
  { timeSlot: "01: 00 - 01: 14", index: 4 },
  { timeSlot: "01: 15 - 01: 29", index: 5 },
  { timeSlot: "01: 30 - 01: 44", index: 6 },
  { timeSlot: "01: 45 - 01: 59", index: 7 },
  { timeSlot: "02: 00 - 02: 14", index: 8 },
  { timeSlot: "02: 15 - 02: 29", index: 9 },
  { timeSlot: "02: 30 - 02: 44", index: 10 },
  { timeSlot: "02: 45 - 02: 59", index: 11 },
  { timeSlot: "03: 00 - 03: 14", index: 12 },
  { timeSlot: "03: 15 - 03: 29", index: 13 },
  { timeSlot: "03: 30 - 03: 44", index: 14 },
  { timeSlot: "03: 45 - 03: 59", index: 15 },
  { timeSlot: "04: 00 - 04: 14", index: 16 },
  { timeSlot: "04: 15 - 04: 29", index: 17 },
  { timeSlot: "04: 30 - 04: 44", index: 18 },
  { timeSlot: "04: 45 - 04: 59", index: 19 },
  { timeSlot: "05: 00 - 05: 14", index: 20 },
  { timeSlot: "05: 15 - 05: 29", index: 21 },
  { timeSlot: "05: 30 - 05: 44", index: 22 },
  { timeSlot: "05: 45 - 05: 59", index: 23 },
  { timeSlot: "06: 00 - 06: 14", index: 24 },
  { timeSlot: "06: 15 - 06: 29", index: 25 },
  { timeSlot: "06: 30 - 06: 44", index: 26 },
  { timeSlot: "06: 45 - 06: 59", index: 27 },
  { timeSlot: "07: 00 - 07: 14", index: 28 },
  { timeSlot: "07: 15 - 07: 29", index: 29 },
  { timeSlot: "07: 30 - 07: 44", index: 30 },
  { timeSlot: "07: 45 - 07: 59", index: 31 },
  { timeSlot: "08: 00 - 08: 14", index: 32 },
  { timeSlot: "08: 15 - 08: 29", index: 33 },
  { timeSlot: "08: 30 - 08: 44", index: 34 },
  { timeSlot: "08: 45 - 08: 59", index: 35 },
  { timeSlot: "09: 00 - 09: 14", index: 36 },
  { timeSlot: "09: 15 - 09: 29", index: 37 },
  { timeSlot: "09: 30 - 09: 44", index: 38 },
  { timeSlot: "09: 45 - 09: 59", index: 39 },
  { timeSlot: "10: 00 - 10: 14", index: 40 },
  { timeSlot: "10: 15 - 10: 29", index: 41 },
  { timeSlot: "10: 30 - 10: 44", index: 42 },
  { timeSlot: "10: 45 - 10: 59", index: 43 },
  { timeSlot: "11: 00 - 11: 14", index: 44 },
  { timeSlot: "11: 15 - 11: 29", index: 45 },
  { timeSlot: "11: 30 - 11: 44", index: 46 },
  { timeSlot: "11: 45 - 11: 59", index: 47 },
  { timeSlot: "12: 00 - 12: 14", index: 48 },
  { timeSlot: "12: 15 - 12: 29", index: 49 },
  { timeSlot: "12: 30 - 12: 44", index: 50 },
  { timeSlot: "12: 45 - 12: 59", index: 51 },
  { timeSlot: "13: 00 - 13: 14", index: 52 },
  { timeSlot: "13: 15 - 13: 29", index: 53 },
  { timeSlot: "13: 30 - 13: 44", index: 54 },
  { timeSlot: "13: 45 - 13: 59", index: 55 },
  { timeSlot: "14: 00 - 14: 14", index: 56 },
  { timeSlot: "14: 15 - 14: 29", index: 57 },
  { timeSlot: "14: 30 - 14: 44", index: 58 },
  { timeSlot: "14: 45 - 14: 59", index: 59 },
  { timeSlot: "15: 00 - 15: 14", index: 60 },
  { timeSlot: "15: 15 - 15: 29", index: 61 },
  { timeSlot: "15: 30 - 15: 44", index: 62 },
  { timeSlot: "15: 45 - 15: 59", index: 63 },
  { timeSlot: "16: 00 - 16: 14", index: 64 },
  { timeSlot: "16: 15 - 16: 29", index: 65 },
  { timeSlot: "16: 30 - 16: 44", index: 66 },
  { timeSlot: "16: 45 - 16: 59", index: 67 },
  { timeSlot: "17: 00 - 17: 14", index: 68 },
  { timeSlot: "17: 15 - 17: 29", index: 69 },
  { timeSlot: "17: 30 - 17: 44", index: 70 },
  { timeSlot: "17: 45 - 17: 59", index: 71 },
  { timeSlot: "18: 00 - 18: 14", index: 72 },
  { timeSlot: "18: 15 - 18: 29", index: 73 },
  { timeSlot: "18: 30 - 18: 44", index: 74 },
  { timeSlot: "18: 45 - 18: 59", index: 75 },
  { timeSlot: "19: 00 - 19: 14", index: 76 },
  { timeSlot: "19: 15 - 19: 29", index: 77 },
  { timeSlot: "19: 30 - 19: 44", index: 78 },
  { timeSlot: "19: 45 - 19: 59", index: 79 },
  { timeSlot: "20: 00 - 20: 14", index: 80 },
  { timeSlot: "20: 15 - 20: 29", index: 81 },
  { timeSlot: "20: 30 - 20: 44", index: 82 },
  { timeSlot: "20: 45 - 20: 59", index: 83 },
  { timeSlot: "21: 00 - 21: 14", index: 84 },
  { timeSlot: "21: 15 - 21: 29", index: 85 },
  { timeSlot: "21: 30 - 21: 44", index: 86 },
  { timeSlot: "21: 45 - 21: 59", index: 87 },
  { timeSlot: "22: 00 - 22: 14", index: 88 },
  { timeSlot: "22: 15 - 22: 29", index: 89 },
  { timeSlot: "22: 30 - 22: 44", index: 90 },
  { timeSlot: "22: 45 - 22: 59", index: 91 },
  { timeSlot: "23: 00 - 23: 14", index: 92 },
  { timeSlot: "23: 15 - 23: 29", index: 93 },
  { timeSlot: "23: 30 - 23: 44", index: 94 },
  { timeSlot: "23: 45 - 23: 59", index: 95 }
  ];

  dataSource = new MatTableDataSource<any>();
  dataSourceMonday = new MatTableDataSource<any>();
  dataSourceTuesday = new MatTableDataSource<any>();
  dataSourceWednesday = new MatTableDataSource<any>();
  dataSourceThursday = new MatTableDataSource<any>();
  dataSourceFridayday = new MatTableDataSource<any>();
  dataSourceSaturday = new MatTableDataSource<any>();
  dataSourceSunday = new MatTableDataSource<any>();
  selection = new SelectionModel<any>(true, []);
  selectionMonday = new SelectionModel<any>(true, []);
  selectionTuesday = new SelectionModel<any>(true, []);
  selectionWednesday = new SelectionModel<any>(true, []);
  selectionThursday = new SelectionModel<any>(true, []);
  selectionFridayday = new SelectionModel<any>(true, []);
  selectionSaturday = new SelectionModel<any>(true, []);
  selectionSunday = new SelectionModel<any>(true, []);

  preventAbuse = false;
  page = 0;
  keyword: string = '';
  totalRow: number = 0;
  totalPage: number = 0;
  pageSizeOptions: number[] = [10, 25, 50, 100];
  pageSize = this.pageSizeOptions[0];

  action!: string;
  aScheduleDeviceDetailForm!: FormGroup;
  title!: string;
  @ViewChild('dialog') dialogTemplate!: TemplateRef<any>;
  constructor(private spinner: NgxSpinnerService,private pagin: PaginatorCustomService, private notificationService: NotificationService, private translateService: TranslateService,
    private dataService: DataService, private dialog: MatDialog, private formBuilder: FormBuilder) {
    this.aScheduleDeviceDetailForm = this.formBuilder.group({
      schDevId: ['', Validators.compose([Validators.required])],
      schName: ['', Validators.compose([Validators.required, Validators.maxLength(200)])],
      mon: '',
      tue: '',
      wed: '',
      thu: '',
      fri: '',
      sat: '',
      sun: ''
    });

    this.dataSourceMonday = new MatTableDataSource(this.weekdayData);
    this.dataSourceTuesday = new MatTableDataSource(this.weekdayData);
    this.dataSourceWednesday = new MatTableDataSource(this.weekdayData);
    this.dataSourceThursday = new MatTableDataSource(this.weekdayData);
    this.dataSourceFridayday = new MatTableDataSource(this.weekdayData);
    this.dataSourceSaturday = new MatTableDataSource(this.weekdayData);
    this.dataSourceSunday = new MatTableDataSource(this.weekdayData);

    this.dataSourceMonday.sort = this.sort;
    this.dataSourceTuesday.sort = this.sort;
    this.dataSourceWednesday.sort = this.sort;
    this.dataSourceThursday.sort = this.sort;
    this.dataSourceFridayday.sort = this.sort;
    this.dataSourceSaturday.sort = this.sort;
    this.dataSourceSunday.sort = this.sort;

    let weekdayTitleMonday = '';
    let weekdayTitleTuesday = '';
    let weekdayTitleWednesday = '';
    let weekdayTitleThursday = '';
    let weekdayTitleFridayday = '';
    let weekdayTitleSaturday = '';
    let weekdayTitleSunday = '';

    this.translateService.get('aScheduleDeviceDetailModule.mondayCol').subscribe(data => weekdayTitleMonday = data);
    this.translateService.get('aScheduleDeviceDetailModule.tuesdayCol').subscribe(data => weekdayTitleTuesday = data);
    this.translateService.get('aScheduleDeviceDetailModule.wednesdayCol').subscribe(data => weekdayTitleWednesday = data);
    this.translateService.get('aScheduleDeviceDetailModule.thursdayCol').subscribe(data => weekdayTitleThursday = data);
    this.translateService.get('aScheduleDeviceDetailModule.fridayCol').subscribe(data => weekdayTitleFridayday = data);
    this.translateService.get('aScheduleDeviceDetailModule.saturdayCol').subscribe(data => weekdayTitleSaturday = data);
    this.translateService.get('aScheduleDeviceDetailModule.sundayCol').subscribe(data => weekdayTitleSunday = data);

    let ws2 = new Weekday(weekdayTitleMonday, this.selectionMonday, this.dataSourceMonday, this.displayedWeekdayColumns, '#34395e', 0);
    let ws3 = new Weekday(weekdayTitleTuesday, this.selectionTuesday, this.dataSourceTuesday, this.displayedWeekdayColumns, '#34395e', 2);
    let ws4 = new Weekday(weekdayTitleWednesday, this.selectionWednesday, this.dataSourceWednesday, this.displayedWeekdayColumns, '#34395e', 2);
    let ws5 = new Weekday(weekdayTitleThursday, this.selectionThursday, this.dataSourceThursday, this.displayedWeekdayColumns, '#34395e', 2);
    let ws6 = new Weekday(weekdayTitleFridayday, this.selectionFridayday, this.dataSourceFridayday, this.displayedWeekdayColumns, '#34395e', 2);
    let ws7 = new Weekday(weekdayTitleSaturday, this.selectionSaturday, this.dataSourceSaturday, this.displayedWeekdayColumns, '#fc544b', 2);
    let wscn = new Weekday(weekdayTitleSunday, this.selectionSunday, this.dataSourceSunday, this.displayedWeekdayColumns, '#fc544b', 2);
    this.weekdays.push(ws2);
    this.weekdays.push(ws3);
    this.weekdays.push(ws4);
    this.weekdays.push(ws5);
    this.weekdays.push(ws6);
    this.weekdays.push(ws7);
    this.weekdays.push(wscn);
  }

  ngOnInit(): void {
    this.loadAScheduleDevDetailListPaging();
    this.loadAllAScheduleDevDetail();
  }
  ngAfterViewInit() {
    this.paginator._intl.itemsPerPageLabel = this.pagin.setLable;
    this.paginator._intl.firstPageLabel = this.pagin.firstButton;
    this.paginator._intl.nextPageLabel = this.pagin.nextButton;
    this.paginator._intl.lastPageLabel = this.pagin.lastButton;
    this.paginator._intl.previousPageLabel = this.pagin.preButton;
  }
  isAllSelected() {
    const numSelected = this.selection.selected.length;
    if (this.dataSource.data.length < 3) {
      const numRows = this.dataSource.data.length;
      return numSelected === numRows;
    } else {
      const numRows = this.dataSource.data.length - 3;
      return numSelected === numRows;
    }
  }
  isAllWeekdaySelected(weekdayItem: Weekday) {
    const numSelected = weekdayItem.selectionWeekday.selected.length;
    const numRows = weekdayItem.dataSourceWeekday.data.length;
    return numSelected === numRows;
  }
  masterToggle() {
    if (this.isAllSelected()) {
      this.selection.clear();
      return;
    }
    this.selection.select(...this.dataSource.data.filter(x => x.schDevId != 63 && x.schDevId != 0 && x.schDevId != 1));
  }
  masterTogglWeekday(weekdayItem: Weekday) {
    if (this.isAllWeekdaySelected(weekdayItem)) {
      weekdayItem.selectionWeekday.clear();
      return;
    }
    weekdayItem.selectionWeekday.select(...weekdayItem.dataSourceWeekday.data);
  }
  checkboxLabel(row?: any): string {
    if (!row) {
      return `${this.isAllSelected() ? 'deselect' : 'select'} all`;
    }
    return `${this.selection.isSelected(row) ? 'deselect' : 'select'} row ${row.position + 1}`;
  }
  loadAScheduleDevDetailListPaging() {
    this.preventAbuse = true;
    this.dataService.get('aScheduleDeviceDetail/getlistpaging?page=' + this.page + "&pageSize=" + this.pageSize + "&keyword=" + this.keyword).subscribe((data: any) => {
      this.dataSource = new MatTableDataSource(data.items);
      this.dataSource.sort = this.sort;
      this.totalRow = data.totalCount;
      this.preventAbuse = false;

    }, (err: any) => {
      this.translateService.get('messageSystem.loadFail').subscribe(data => MessageConstants.GET_FAILSE_MSG = data);
      this.notificationService.printErrorMessage(MessageConstants.GET_FAILSE_MSG);
      this.preventAbuse = false;
    });
  }
  schDevDetailIdListNotCreated: any = [];
  loadAllAScheduleDevDetail() {
    this.schDevDetailIdListNotCreated = [];
    this.preventAbuse = true;
    this.dataService.get('aScheduleDeviceDetail/getall').subscribe((data: any) => {
      this.preventAbuse = false;
      let schDevDetailIdListCreated: any = [];
      data.forEach((item: any) => {
        schDevDetailIdListCreated.push(item.schDevId);
      });
      for (let i = 0; i < 59; i++) {
        if (schDevDetailIdListCreated.indexOf(i) == -1) {
          this.schDevDetailIdListNotCreated.push(i);
        }
      }
    }, (err: any) => {
      this.translateService.get('messageSystem.loadFail').subscribe(data => MessageConstants.GET_FAILSE_MSG = data);
      this.notificationService.printErrorMessage(MessageConstants.GET_FAILSE_MSG);
      this.preventAbuse = false;
    });
  }
  applyFilter(event: Event) {
    this.keyword = (event.target as HTMLInputElement).value;
    this.page = 0;
    this.loadAScheduleDevDetailListPaging();
    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }
  onChangePage(pe: PageEvent) {
    this.page = pe.pageIndex;
    this.pageSize = pe.pageSize;
    this.loadAScheduleDevDetailListPaging();
  }
  removeData() {
    let roleChecked: any[] = [];
    this.selection.selected.forEach((value: any) => {
      let id = value.schDevId;
      roleChecked.push(id);
    });
    this.translateService.get('messageSystem.confirmDelete').subscribe(data => MessageConstants.CONFIRM_DELETE_MSG = data);
    this.notificationService.printConfirmationDialog(MessageConstants.CONFIRM_DELETE_MSG, () => this.deleteItemConfirm(JSON.stringify(roleChecked)));
  }
  deleteItemConfirm(id: string) {
    this.preventAbuse = true;
    this.spinner.show();
    this.dataService.delete('aScheduleDeviceDetail/deletemulti', 'checkedList', id).subscribe((res: any) => {
      this.translateService.get('messageSystem.deleteSuccess').subscribe(data => MessageConstants.DELETED_OK_MSG = data);
      this.notificationService.printSuccessMessage(MessageConstants.DELETED_OK_MSG);
      this.selection.clear();
      this.loadAScheduleDevDetailListPaging();
      this.spinner.hide();
    }, (err: any) => {
      this.preventAbuse = false;
      this.translateService.get('messageSystem.deletefail').subscribe(data => MessageConstants.DELETE_FAILSE_MSG = data);
      this.notificationService.printErrorMessage(MessageConstants.DELETE_FAILSE_MSG);
      this.spinner.hide();
    });
  }
  openDialog(action: string, item?: any, config?: MatDialogConfig) {
    this.resetChecked();
    this.action = action;
    if (action == 'create') {
      this.loadAllAScheduleDevDetail();
      this.translateService.get('aScheduleDeviceDetailModule.createMessage').subscribe(data => this.title = data);
    }
    else {
      this.translateService.get('aScheduleDeviceDetailModule.editMessage').subscribe(data => this.title = data);
      this.dataService.get('aScheduleDeviceDetail/getbyid/' + item.schDevId).subscribe((data: any) => {
        let item = data;
        this.schDevDetailIdListNotCreated.push(item.schDevId);
        this.aScheduleDeviceDetailForm.controls['schDevId'].setValue(item.schDevId);
        this.aScheduleDeviceDetailForm.controls['schName'].setValue(item.schName);
        this.aScheduleDeviceDetailForm.controls['mon'].setValue(item.mon);
        this.aScheduleDeviceDetailForm.controls['tue'].setValue(item.tue);
        this.aScheduleDeviceDetailForm.controls['wed'].setValue(item.wed);
        this.aScheduleDeviceDetailForm.controls['thu'].setValue(item.thu);
        this.aScheduleDeviceDetailForm.controls['fri'].setValue(item.fri);
        this.aScheduleDeviceDetailForm.controls['sat'].setValue(item.sat);
        this.aScheduleDeviceDetailForm.controls['sun'].setValue(item.sun);

        let thu2: number[] = [];
        let thu3: number[] = [];
        let thu4: number[] = [];
        let thu5: number[] = [];
        let thu6: number[] = [];
        let thu7: number[] = [];
        let chunhat: number[] = [];
        if (item.mon != '') {
          thu2 = item.mon.split(',').map((x: number) => +x);
        }
        if (item.tue != '') {
          thu3 = item.tue.split(',').map((x: number) => +x);
        }
        if (item.wed != '') {
          thu4 = item.wed.split(',').map((x: number) => +x);
        }
        if (item.thu != '') {
          thu5 = item.thu.split(',').map((x: number) => +x);
        }
        if (item.fri != '') {
          thu6 = item.fri.split(',').map((x: number) => +x);
        }
        if (item.sat != '') {
          thu7 = item.sat.split(',').map((x: number) => +x);
        }
        if (item.sun != '') {
          chunhat = item.sun.split(',').map((x: number) => +x);
        }
        this.dataSourceMonday.data.forEach((item: any) => {
          if (thu2.indexOf(item.index) != -1) {
            this.selectionMonday.select(item);
          }
        });
        this.dataSourceTuesday.data.forEach((item: any) => {
          if (thu3.indexOf(item.index) != -1) {
            this.selectionTuesday.select(item);
          }
        });
        this.dataSourceWednesday.data.forEach((item: any) => {
          if (thu4.indexOf(item.index) != -1) {
            this.selectionWednesday.select(item);
          }
        });
        this.dataSourceThursday.data.forEach((item: any) => {
          if (thu5.indexOf(item.index) != -1) {
            this.selectionThursday.select(item);
          }
        });
        this.dataSourceFridayday.data.forEach((item: any) => {
          if (thu6.indexOf(item.index) != -1) {
            this.selectionFridayday.select(item);
          }
        });
        this.dataSourceSaturday.data.forEach((item: any) => {
          if (thu7.indexOf(item.index) != -1) {
            this.selectionSaturday.select(item);
          }
        });
        this.dataSourceSunday.data.forEach((item: any) => {
          if (chunhat.indexOf(item.index) != -1) {
            this.selectionSunday.select(item);
          }
        });
        this.preventAbuse = false;
      }, (err: any) => {
        this.translateService.get('messageSystem.loadFail').subscribe(data => MessageConstants.GET_FAILSE_MSG = data);
        this.notificationService.printErrorMessage(MessageConstants.GET_FAILSE_MSG);
        this.preventAbuse = false;
      });
    }
    const dialogRef = this.dialog.open(this.dialogTemplate, {
      width: '1250px', autoFocus: false
    });
    dialogRef.disableClose = true;
    dialogRef.afterClosed().subscribe(result => {
      this.onReset();
    });
  }
  resetChecked() {
    this.dataSourceMonday.data.forEach((item: any) => {
      this.selectionMonday.deselect(item);
    });
    this.dataSourceTuesday.data.forEach((item: any) => {
      this.selectionTuesday.deselect(item);
    });
    this.dataSourceWednesday.data.forEach((item: any) => {
      this.selectionWednesday.deselect(item);
    });
    this.dataSourceThursday.data.forEach((item: any) => {
      this.selectionThursday.deselect(item);
    });
    this.dataSourceFridayday.data.forEach((item: any) => {
      this.selectionFridayday.deselect(item);
    });
    this.dataSourceSaturday.data.forEach((item: any) => {
      this.selectionSaturday.deselect(item);
    });
    this.dataSourceSunday.data.forEach((item: any) => {
      this.selectionSunday.deselect(item);
    });
  }
  onReset() {
    this.action == '';
    this.dialog.closeAll();
    this.aScheduleDeviceDetailForm.reset();
  }
  get getValidForm(): { [key: string]: AbstractControl } {
    return this.aScheduleDeviceDetailForm.controls;
  }
  addData() {
    if (this.aScheduleDeviceDetailForm.invalid) {
      return;
    }
    // this.preventAbuse = true;
    let thu2: number[] = [];
    this.selectionMonday.selected.forEach((item: any) => {
      let id = item.index;
      thu2.push(id);
    });
    let thu3: number[] = [];
    this.selectionTuesday.selected.forEach((item: any) => {
      let id = item.index;
      thu3.push(id);
    });
    let thu4: number[] = [];
    this.selectionWednesday.selected.forEach((item: any) => {
      let id = item.index;
      thu4.push(id);
    });
    let thu5: number[] = [];
    this.selectionThursday.selected.forEach((item: any) => {
      let id = item.index;
      thu5.push(id);
    });
    let thu6: number[] = [];
    this.selectionFridayday.selected.forEach((item: any) => {
      let id = item.index;
      thu6.push(id);
    });
    let thu7: number[] = [];
    this.selectionSaturday.selected.forEach((item: any) => {
      let id = item.index;
      thu7.push(id);
    });
    let chunhat: number[] = [];
    this.selectionSunday.selected.forEach((item: any) => {
      let id = item.index;
      chunhat.push(id);
    });
    this.aScheduleDeviceDetailForm.controls['mon'].setValue(thu2.toString());
    this.aScheduleDeviceDetailForm.controls['tue'].setValue(thu3.toString());
    this.aScheduleDeviceDetailForm.controls['wed'].setValue(thu4.toString());
    this.aScheduleDeviceDetailForm.controls['thu'].setValue(thu5.toString());
    this.aScheduleDeviceDetailForm.controls['fri'].setValue(thu6.toString());
    this.aScheduleDeviceDetailForm.controls['sat'].setValue(thu7.toString());
    this.aScheduleDeviceDetailForm.controls['sun'].setValue(chunhat.toString());
    let aSheduleDeviceDetail =
    {
      schDevId: this.aScheduleDeviceDetailForm.controls['schDevId'].value,
      schName: this.aScheduleDeviceDetailForm.controls['schName'].value,
      mon: this.aScheduleDeviceDetailForm.controls['mon'].value,
      tue: this.aScheduleDeviceDetailForm.controls['tue'].value,
      wed: this.aScheduleDeviceDetailForm.controls['wed'].value,
      thu: this.aScheduleDeviceDetailForm.controls['thu'].value,
      fri: this.aScheduleDeviceDetailForm.controls['fri'].value,
      sat: this.aScheduleDeviceDetailForm.controls['sat'].value,
      sun: this.aScheduleDeviceDetailForm.controls['sun'].value,
    };
    if (this.action == 'create') {
      this.spinner.show();
      this.dataService.post('aScheduleDeviceDetail/create', aSheduleDeviceDetail).subscribe((data: any) => {
        let message!: string;
        this.translateService.get('messageSystem.createSuccess').subscribe(mes => message = mes);
        this.notificationService.printSuccessMessage(message);
        this.loadAScheduleDevDetailListPaging();
        this.spinner.hide();
      }, (err: any) => {
        this.preventAbuse = false;
        let error!: string;
        this.translateService.get('messageSystem.createFail').subscribe(mes => error = mes);
        this.notificationService.printErrorMessage(error);
        this.spinner.hide();
      });
    } else if (this.action == 'edit') {
      this.spinner.show();
      this.dataService.put('aScheduleDeviceDetail/update', aSheduleDeviceDetail).subscribe((data: any) => {
        let message!: string;
        this.translateService.get('messageSystem.updateSuccess').subscribe(mes => message = mes);
        this.notificationService.printSuccessMessage(message);
        this.loadAScheduleDevDetailListPaging();
        this.spinner.hide();
      }, (err: any) => {
        this.preventAbuse = false;
        let error!: string;
        this.translateService.get('messageSystem.updateFail').subscribe(mes => error = mes);
        this.notificationService.printErrorMessage(error);
        this.spinner.hide();
      });
    }
  }
}

