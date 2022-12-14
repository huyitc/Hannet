import { DatePipe } from '@angular/common';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { TranslateService } from '@ngx-translate/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { MessageConstants } from 'src/app/core/common/message.constants';
import { DataService } from 'src/app/core/services/data.service';
import { DeviceTypeService } from 'src/app/core/services/device-type.service';
import { NotificationService } from 'src/app/core/services/notification.service';
import { PaginatorCustomService } from 'src/app/core/services/paginator-custom.service';

@Component({
  selector: 'app-check-in-by-day',
  templateUrl: './check-in-by-day.component.html',
  styleUrls: ['./check-in-by-day.component.css']
})
export class CheckInByDayComponent implements OnInit {
  displayedColumns: string[] = ['position','personName','aliasID','personID','date', 'checkinTime','placeID','place','title','type','avatar','deviceID','deviceName'];
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  dataSource = new MatTableDataSource<any>();
  preventAbuse = false;
  day=this.datepipe.transform(new Date(),'yyyy-MM-dd');
  placeId:any="";
  zones:any;
  dataList:any=[];
  showList:boolean = false;
  pageSizeOptions: number[] = [10, 25, 50, 100];
  pageSize = this.pageSizeOptions[0];
  page = 0;
  constructor(private spinner: NgxSpinnerService,private dataService: DataService, public dialog: MatDialog,
    private notificationService: NotificationService, private pagin: PaginatorCustomService,
    private translateService: TranslateService, private _deviceTypeService: DeviceTypeService,private datepipe: DatePipe) 
    { 
      
    }

  ngOnInit(): void {
    this.getZones();
    this.checkIn();
  }

  getZones() {
    this.dataService.get('AZone/getall').subscribe(
      (data: any) => {
        this.zones = data;
      },
      (err) => { }
    );
  }

  checkIn(){
    if(this.placeId == ""){
      let err!: string;
      this.translateService.get('checkInByDate.placeIdCodeRequired').subscribe(mes => err = mes);
      this.notificationService.printErrorMessage(err);
      this.preventAbuse = false;
      return;
    }
    let bod = {
      placeID:this.placeId,
      date:this.day,
    }
    this.preventAbuse = true;
    this.spinner.show();
    this.dataService.post('GetCheckin/getall',bod).subscribe((res:any)=>{
      if (res.data.length == 0) {
        this.showList = true;
        this.dataSource = res.data;
        this.dataSource.sort = this.sort;
        this.translateService.get('messageSystem.staticsNull').subscribe(data => MessageConstants.STATISTIC_NULL = data);
        this.notificationService.printErrorMessage(MessageConstants.STATISTIC_NULL);
        this.spinner.hide();
      }
      else{
       this.showList = true;
        this.dataSource = res.data;
        this.dataSource.sort = this.sort;
        this.spinner.hide();
        }
        this.preventAbuse = false;
    },(err:any)=>{
      this.translateService.get('messageSystem.loadFail').subscribe(data=>MessageConstants.GET_FAILSE_MSG=data);
      this.notificationService.printErrorMessage(MessageConstants.GET_FAILSE_MSG);
      this.spinner.hide();
    });
    this.spinner.hide();
  }

  onReset(){
    this.day = this.datepipe.transform(new Date(),'yyyy-MM-dd');
    this.placeId = "";
  }
}
