import { PaginatorCustomService } from '../../../core/services/paginator-custom.service';
import { FormGroup, FormBuilder, Validators, AbstractControl } from '@angular/forms';
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

@Component({
  selector: 'app-a-zone',
  templateUrl: './a-zone.component.html',
  styleUrls: ['./a-zone.component.css']
})
export class AZoneComponent implements OnInit {
  displayedColumns: string[] = ['select', 'position', 'zonName', 'zonDescription', 'zonStatus', 'action'];
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  dataSource = new MatTableDataSource<any>();
  preventAbuse = false;
  page = 0;
  keyword: string = '';
  totalRow: number = 0;
  totalPage: number = 0;
  pageSizeOptions: number[] = [10, 25, 50, 100];
  pageSize = this.pageSizeOptions[0];
  selection = new SelectionModel<any>(true, []);
  action!: string;
  aZoneForm!: FormGroup;
  title!: string;
  @ViewChild('dialog') dialogTemplate!: TemplateRef<any>;
  constructor(private spinner: NgxSpinnerService,private pagin: PaginatorCustomService,private notificationService: NotificationService, private translateService: TranslateService,
    private dataService: DataService, private dialog: MatDialog, private formBuilder: FormBuilder) {
    this.aZoneForm = this.formBuilder.group({
      zonId: 0,
      zonName: ['', Validators.compose([Validators.required, Validators.maxLength(200)])],
      zonDescription: ['', Validators.compose([Validators.required, Validators.maxLength(200)])],
      zonStatus: true
    });
  }

  ngOnInit(): void {
    this.loadAZoneListPaging();
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
    const numRows = this.dataSource.data.length;
    return numSelected === numRows;
  }
  masterToggle() {
    if (this.isAllSelected()) {
      this.selection.clear();
      return;
    }

    this.selection.select(...this.dataSource.data);

  }
  checkboxLabel(row?: any): string {
    if (!row) {
      return `${this.isAllSelected() ? 'deselect' : 'select'} all`;
    }
    return `${this.selection.isSelected(row) ? 'deselect' : 'select'} row ${row.position + 1}`;
  }
  loadAZoneListPaging() {
    this.preventAbuse = true;
    this.dataService.get('azone/getlistpaging?page=' + this.page + "&pageSize=" + this.pageSize + "&keyword=" + this.keyword).subscribe((data: any) => {
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
  applyFilter(event: Event) {
    this.keyword = (event.target as HTMLInputElement).value;
    this.page = 0;
    this.loadAZoneListPaging();
    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }
  onChangePage(pe: PageEvent) {
    this.page = pe.pageIndex;
    this.pageSize = pe.pageSize;
    this.loadAZoneListPaging();
  }
  removeData() {
    let roleChecked: any[] = [];
    console.log(this.selection.selected)
    this.selection.selected.forEach((value: any) => {
      let id = { id: value.zonId, placeId: value.placeId };
      roleChecked.push(id);
    });
    this.translateService.get('messageSystem.confirmDelete').subscribe(data => MessageConstants.CONFIRM_DELETE_MSG = data);
    this.notificationService.printConfirmationDialog(MessageConstants.CONFIRM_DELETE_MSG, () => this.deleteItemConfirm(JSON.stringify(roleChecked)));
  }

  deleteItemConfirm(id: string) {
    this.preventAbuse = true;
    this.spinner.show();
    this.dataService.delete('azone/deletemulti', 'checkedList', id).subscribe((res: any) => {
      this.translateService.get('messageSystem.deleteSuccess').subscribe(data => MessageConstants.DELETED_OK_MSG = data);
      this.notificationService.printSuccessMessage(MessageConstants.DELETED_OK_MSG);
      this.selection.clear();
      this.loadAZoneListPaging();
      this.spinner.hide();
    }, (err: any) => {
      this.preventAbuse = false;
      this.translateService.get('messageSystem.deletefail').subscribe(data => MessageConstants.DELETE_FAILSE_MSG = data);
      this.notificationService.printErrorMessage(MessageConstants.DELETE_FAILSE_MSG);
      this.spinner.hide();
    });
  }
  openDialog(action: string, item?: any, config?: MatDialogConfig) {
    this.action = action;
    if (action == 'create') {
      this.translateService.get('aZoneModule.createMessage').subscribe(data => this.title = data);
      this.aZoneForm.controls['zonStatus'].setValue(true);
    }
    else {
      this.translateService.get('aZoneModule.editMessage').subscribe(data => this.title = data);
      this.aZoneForm.controls['zonId'].setValue(item.zonId);
      let itemFilter = this.dataSource.filteredData.filter((x: any) => x.zonId == item.zonId)[0];
      this.aZoneForm.controls['zonName'].setValue(itemFilter.zonName);
      this.aZoneForm.controls['zonDescription'].setValue(itemFilter.zonDescription);
      this.aZoneForm.controls['zonStatus'].setValue(itemFilter.zonStatus);
    }
    const dialogRef = this.dialog.open(this.dialogTemplate, {
      width: '650px'
    });
    dialogRef.disableClose = true;
    dialogRef.afterClosed().subscribe(result => {
      this.onReset();
    });
  }

  onReset() {
    this.action == '';
    this.dialog.closeAll();
    this.aZoneForm.reset();
  }
  get getValidForm(): { [key: string]: AbstractControl } {
    return this.aZoneForm.controls;
  }
  addData() {
    if (this.aZoneForm.invalid) {
      return;
    }
    this.preventAbuse = true;
    if (this.action == 'create') {
      this.spinner.show();
      let aZone =
      {
        zonName: this.aZoneForm.controls['zonName'].value,
        zonDescription: this.aZoneForm.controls['zonDescription'].value,
        zonStatus: this.aZoneForm.controls['zonStatus'].value
      };
      this.dataService.post('aZone/create', aZone).subscribe((data: any) => {
        let message!: string;
        this.translateService.get('messageSystem.createSuccess').subscribe(mes => message = mes);
        this.notificationService.printSuccessMessage(message);
        this.loadAZoneListPaging();
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
      let aZone =
      {
        zonId:this.aZoneForm.controls['zonId'].value,
        zonName: this.aZoneForm.controls['zonName'].value,
        zonDescription: this.aZoneForm.controls['zonDescription'].value,
        zonStatus: this.aZoneForm.controls['zonStatus'].value
      };
      this.dataService.put('aZone/update', aZone).subscribe((data: any) => {
        let message!: string;
        this.translateService.get('messageSystem.updateSuccess').subscribe(mes => message = mes);
        this.notificationService.printSuccessMessage(message);
        this.loadAZoneListPaging();
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
