import { MatOptionSelectionChange } from '@angular/material/core';
import { TreeAZoneTDevice } from '../../../core/domain/treeAZoneTDevice.model';
import { FlatTreeControl } from '@angular/cdk/tree';
import { MatTreeFlatDataSource, MatTreeFlattener } from '@angular/material/tree';
import { SelectionModel } from '@angular/cdk/collections';
import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators, ValidatorFn } from '@angular/forms';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { TranslateService } from '@ngx-translate/core';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { MessageConstants } from 'src/app/core/common/message.constants';
import { DataService } from 'src/app/core/services/data.service';
import { NotificationService } from 'src/app/core/services/notification.service';
import { PaginatorCustomService } from 'src/app/core/services/paginator-custom.service';
import { numbers } from '@material/textfield';
import { NgxSpinnerService } from 'ngx-spinner';
/** Flat node with expandable and level information */
interface FlatNodeTemplate {
  expandable: boolean;
  name: string;
  level: number;
  id: number;
}
@Component({
  selector: 'app-t-groupaccess',
  templateUrl: './t-groupaccess.component.html',
  styleUrls: ['./t-groupaccess.component.css']
})
export class TGroupaccessComponent implements OnInit {

  displayedColumns: string[] = ['select', 'position', 'gaName', 'gaStatus', 'action',];
  dataSource = new MatTableDataSource<any>();
  preventAbuse = false;
  page = 0;
  keyword: string = '';
  totalRow: number = 0;
  totalPage: number = 0;
  gaParents: any;
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  @ViewChild('dialog') dialogTemplate!: TemplateRef<any>;
  @ViewChild('dialogTGAD') dialogTemplateTGAD!: TemplateRef<any>;
  selection = new SelectionModel<any>(true, []);
  groupAccessForm!: FormGroup;
  action: string = '';
  title: string = '';
  pageSizeOptions: number[] = [10, 25, 50, 100];
  pageSize = this.pageSizeOptions[0];
  isAllChecked = false;
  parentName: any;
  filteredOptions!: Observable<any[]>;
  users: any;
  GroupAccessParentControl = new FormControl();


  constructor(private spinner: NgxSpinnerService,private dataService: DataService, public dialog: MatDialog,
    private notificationService: NotificationService, private pagin: PaginatorCustomService,
    private translateService: TranslateService, private formBuilder: FormBuilder) {

    this.groupAccessForm = this.formBuilder.group({
      gaId: '',
      gaName: ['', Validators.compose([Validators.required, Validators.maxLength(50)])],
      gaStatus: '',
    })
    this.filteredOptions = this.GroupAccessParentControl.valueChanges.pipe(
      startWith(''),
      map((value: any) => (typeof value === 'string' ? value : value)),
    );
  }
  ngOnInit(): void {
    this.loadTGroupAccessListPaging();
    this.loadTreeAZoneTDevice();
  }

  //open dialog add or edit
  openDialog(action: string, item?: any, config?: MatDialogConfig) {
    this.action = action;
    if (action == 'create') {
      this.translateService.get('tGroupAccessModule.createMessage').subscribe(data => this.title = data);
      this.groupAccessForm.controls['gaStatus'].setValue(true);
    }
    else {
      this.translateService.get('tGroupAccessModule.editMessage').subscribe(data => this.title = data);
      this.groupAccessForm.controls['gaId'].setValue(item.gaId);
      let itemFilter = this.dataSource.filteredData.filter((x: any) => x.gaId == item.gaId)[0];
      this.groupAccessForm.controls['gaName'].setValue(itemFilter.gaName);
      this.groupAccessForm.controls['gaStatus'].setValue(itemFilter.gaStatus);
    }
    const dialogRef = this.dialog.open(this.dialogTemplate, { width: '650px' });
    dialogRef.disableClose = true;
    dialogRef.afterClosed().subscribe(result => {
      this.onReset();
    })
  }
  onReset() {
    this.action == '';
    this.dialog.closeAll();
    this.groupAccessForm.reset();
  }
  loadTGroupAccessListPaging() {
    this.preventAbuse = true;
    this.dataService.get('TGroupAccess/getlistpaging?page=' + this.page + "&pageSize=" + this.pageSize + "&keyword=" + this.keyword).subscribe((data: any) => {
      this.dataSource = new MatTableDataSource(data.items);
      this.dataSource.sort = this.sort;
      this.totalRow = data.totalCount;
      this.preventAbuse = false;
    }, err => {
      this.preventAbuse = false;
      this.translateService.get('messageSystem.loadFail').subscribe(data => MessageConstants.GET_FAILSE_MSG = data);
      this.notificationService.printErrorMessage(MessageConstants.GET_FAILSE_MSG);
    });
  }

  //search
  applyFilter(event: Event) {
    //const filterValue = (event.target as HTMLInputElement).value;
    this.keyword = (event.target as HTMLInputElement).value;
    this.page = 0;
    this.loadTGroupAccessListPaging();
    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }
  //select all record
  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.data.length;
    return numSelected === numRows;
  }

  //toggle select
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

  get getValidForm(): { [key: string]: AbstractControl } {
    return this.groupAccessForm.controls;
  }

  //Save change data
  addData() {
    if (this.groupAccessForm.invalid) {
      return;
    }
    this.preventAbuse = true;
    if (this.action == 'create') {
      this.spinner.show();
      let groupaccess = { gaName: this.groupAccessForm.controls['gaName'].value, gaStatus: this.groupAccessForm.controls['gaStatus'].value };
      this.dataService.post('TGroupAccess/create', groupaccess).subscribe(data => {
        let message!: string;
        this.translateService.get('messageSystem.createSuccess').subscribe(mes => message = mes);
        this.notificationService.printSuccessMessage(message);
        this.loadTGroupAccessListPaging();
        this.dialog.closeAll();
        this.onReset();
        this.spinner.hide();
      }, err => {
        this.preventAbuse = false;
        let error!: string;
        this.translateService.get('messageSystem.createFail').subscribe(mes => error = mes);
        this.notificationService.printErrorMessage(err);
        this.spinner.hide();
      });
    }
    else if (this.action == 'edit') {
      this.spinner.show();
      let groupaccess = { gaName: this.groupAccessForm.controls['gaName'].value, gaId: this.groupAccessForm.controls['gaId'].value, gaStatus: this.groupAccessForm.controls['gaStatus'].value };
      this.dataService.put('TGroupAccess/update', groupaccess).subscribe(data => {
        let message!: string;
        this.translateService.get('messageSystem.updateSuccess').subscribe(mes => message = mes);
        this.notificationService.printSuccessMessage(message);
        this.loadTGroupAccessListPaging();
        this.dialog.closeAll();
        this.onReset();
        this.spinner.hide();
      }, err => {
        this.preventAbuse = false;
        let error!: string;
        this.translateService.get('messageSystem.updateFail').subscribe(mes => error = mes);
        this.notificationService.printErrorMessage(err);
        this.spinner.hide();
      })
    }
    this.onReset();
  }

  //remove muilti data
  removeData() {
    let groupaccessChecked: any[] = [];
    this.selection.selected.forEach((value: any) => {
      let id = value.gaId;
      groupaccessChecked.push(id);
    });
    this.translateService.get('messageSystem.confirmDelete').subscribe(data => MessageConstants.CONFIRM_DELETE_MSG = data);
    this.notificationService.printConfirmationDialog(MessageConstants.CONFIRM_DELETE_MSG, () => this.deleteItemConfirm(JSON.stringify(groupaccessChecked)));
  }

  deleteItemConfirm(gaId: string) {
    this.preventAbuse = true;
    this.spinner.show();
    this.dataService.delete('TGroupAccess/deletemulti', 'checkedList', gaId).subscribe(response => {
      this.translateService.get('messageSystem.deleteSuccess').subscribe(data => MessageConstants.DELETED_OK_MSG = data);
      this.notificationService.printSuccessMessage(MessageConstants.DELETED_OK_MSG);
      this.selection.clear();
      this.loadTGroupAccessListPaging();
      this.spinner.hide();
    }, err => {
      this.preventAbuse = false;
      this.translateService.get('messageSystem.deletefail').subscribe(data => MessageConstants.DELETE_FAILSE_MSG = data);
      this.notificationService.printErrorMessage(MessageConstants.DELETE_FAILSE_MSG);
      this.spinner.hide();
    });
  }


  ngAfterViewInit() {
    this.paginator._intl.itemsPerPageLabel = this.pagin.setLable;
    this.paginator._intl.firstPageLabel = this.pagin.firstButton;
    this.paginator._intl.nextPageLabel = this.pagin.nextButton;
    this.paginator._intl.lastPageLabel = this.pagin.lastButton;
    this.paginator._intl.previousPageLabel = this.pagin.preButton;
  }
  //change paging
  onChangePage(pe: PageEvent) {
    this.page = pe.pageIndex;
    this.pageSize = pe.pageSize;
    this.loadTGroupAccessListPaging();
  }

  openDialogTGAD() {
    this.loadAllTGroupAccess();
  }
  onCloseDialogTGADt() {
    this.dialog.closeAll();
    this.tGroupAccessControl.reset();
  }
  resetTreeChecked() {
    this.treeControl.dataNodes.forEach((element: any) => {
      this.checklistSelection.deselect(element);
    });
  }
  tGroupAccess: any;
  optionsTGroupAccess!: Observable<any[]>;
  tGroupAccessControl = new FormControl('',
    { validators: [this.autocompleteObjectValidator(), Validators.required] });
  preventAbuse2: boolean = false;
  loadAllTGroupAccess() {
    this.preventAbuse2 = true;
    this.dataService.get('tgroupaccess/getall').subscribe((data: any) => {
      this.preventAbuse2 = false;
      this.tGroupAccess = data;
      this.optionsTGroupAccess = this.tGroupAccessControl.valueChanges.pipe(
        startWith(''),
        map((value: any) => (value == null ? value : typeof value === 'string' ? value : value.gaName)),
        map(name => (name ? this._filterTGroupAccess(name) : this.tGroupAccess.slice())),
      );
      const dialogRef = this.dialog.open(this.dialogTemplateTGAD, { width: '650px', autoFocus: false });
      dialogRef.disableClose = true
      this.treeControl.expandAll();
      this.resetTreeChecked();
      dialogRef.afterClosed().subscribe(result => {
        this.onCloseDialogTGADt();
      });
    }, (err: any) => {
      this.translateService.get('messageSystem.loadFail').subscribe(data => MessageConstants.GET_FAILSE_MSG = data);
      this.notificationService.printErrorMessage(MessageConstants.GET_FAILSE_MSG);
      this.preventAbuse2 = false;
    });
  }
  autocompleteObjectValidator(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
      if (typeof control.value === 'string') {
        return { 'invalidAutocompleteObject': { value: control.value } }
      }
      return null  /* valid option selected */
    }
  }
  private _filterTGroupAccess(value: any): any[] {
    const filterValue = value?.toLowerCase();
    return this.tGroupAccess.filter((option: any) => option?.gaName.toLowerCase().includes(filterValue));
  }
  displayFnTdevType(ga: any): string {
    return ga && ga.gaName ? ga.gaName : '';
  }
  gaId: number = 0;
  onchangeTGroupAccess(event: MatOptionSelectionChange, item: any) {
    this.resetTreeChecked();
    if (event.source.selected) {
      this.gaId = item.gaId;
      this.preventAbuse2 = true;
      this.dataService.get('TGroupAccessDetail/getbygaid/' + this.gaId).subscribe((data: any) => {
        let devIds = data;
        this.treeControl.dataNodes.filter(x => x.level == 2).forEach((element: any) => {
          if (devIds.indexOf(element.id) >= 0) {
            this.checklistSelection.select(element);
            this.checkAllParentsSelection(element);
          }
        });
        this.preventAbuse2 = false;
      }, (err: any) => {
        this.translateService.get('messageSystem.loadFail').subscribe(data => MessageConstants.GET_FAILSE_MSG = data);
        this.notificationService.printErrorMessage(MessageConstants.GET_FAILSE_MSG);
        this.preventAbuse2 = false;
      });
    }
  }
  saveTGAD() {
    if (this.tGroupAccessControl.invalid) {
      return;
    }
    let deviceSelecteds: any = [];
    this.treeControl.dataNodes.filter(x => x.level == 2).forEach((element: any) => {
      if (this.checklistSelection.selected.filter(x => x.level == 2).indexOf(element) >= 0) {
        let deviceSelectedItem = {
          devId: element.id,
          selected: true
        }
        deviceSelecteds.push(deviceSelectedItem);
      } else {
        let deviceSelectedItem = {
          devId: element.id,
          selected: false
        }
        deviceSelecteds.push(deviceSelectedItem);
      }
    });
    let tgadMapping = {
      gaId: this.gaId,
      deviceItemMappings: deviceSelecteds
    };
    this.preventAbuse2 = true;
    this.spinner.show();
    this.dataService.put('TGroupAccessDetail/settgrouaccessdetail', tgadMapping).subscribe((data: any) => {
      let message!: string;
      this.translateService.get('messageSystem.updateSuccess').subscribe(mes => message = mes);
      this.notificationService.printSuccessMessage(message);
      this.preventAbuse2 = false;
      this.spinner.hide();
    }, (err: any) => {
      this.preventAbuse2 = false;
      let error!: string;
      this.translateService.get('messageSystem.updateFail').subscribe(mes => error = mes);
      this.notificationService.printErrorMessage(error);
      this.spinner.hide();
    });
  }
  loadTreeAZoneTDevice() {
    this.preventAbuse2 = true;
    this.dataService.get('TGroupAccessDetail/gettreeazonetdevice').subscribe((data: any) => {
      this.preventAbuse2 = false;
      let parentTreeTitle: string = '';
      this.translateService.get('tGroupAcessDetailModule.parentTreeTitle').subscribe(data => parentTreeTitle = data);
      let tree: TreeAZoneTDevice[] = [new TreeAZoneTDevice(0, parentTreeTitle, data)]
      this.dataTreeSource.data = tree
    }, (err: any) => {
      this.translateService.get('messageSystem.loadFail').subscribe(data => MessageConstants.GET_FAILSE_MSG = data);
      this.notificationService.printErrorMessage(MessageConstants.GET_FAILSE_MSG);
      this.preventAbuse2 = false;
    });
  }
  private _transformer = (node: TreeAZoneTDevice, level: number) => {
    return {
      expandable: !!node.childrens && node.childrens.length > 0,
      name: node.name,
      level: level,
      id: node.id,
    };
  };

  treeControl = new FlatTreeControl<FlatNodeTemplate>(
    node => node.level,
    node => node.expandable,
  );
  treeFlattener = new MatTreeFlattener(
    this._transformer,
    node => node.level,
    node => node.expandable,
    node => node.childrens,
  );
  dataTreeSource = new MatTreeFlatDataSource(this.treeControl, this.treeFlattener);

  /** The selection for checklist */
  checklistSelection = new SelectionModel<FlatNodeTemplate>(true /* multiple */);
  hasChild = (_: number, node: FlatNodeTemplate) => node.expandable;

  /** tất cả children checked => checked parent */
  descendantsAllSelected(node: FlatNodeTemplate): boolean {
    const descendants = this.treeControl.getDescendants(node);
    return descendants.every(child => this.checklistSelection.isSelected(child));
  }

  /** children ckecked (ngoại trừ tất cả children checked) => [indeterminate] parent */
  descendantsPartiallySelected(node: FlatNodeTemplate): boolean {
    const descendants = this.treeControl.getDescendants(node);
    const result = descendants.some(child => this.checklistSelection.isSelected(child));
    return result && !this.descendantsAllSelected(node);
  }

  /** checked parent => checked tất cả children */
  todoItemSelectionToggle(node: FlatNodeTemplate): void {
    this.checklistSelection.toggle(node);
    const descendants = this.treeControl.getDescendants(node);
    this.checklistSelection.isSelected(node)
      ? this.checklistSelection.select(...descendants)
      : this.checklistSelection.deselect(...descendants);
  }

  /** thay đổi check của children */
  todoLeafItemSelectionToggle(node: FlatNodeTemplate): void {
    this.checklistSelection.toggle(node);
    this.checkAllParentsSelection(node);
  }

  /* vòng lặp tìm các parent và kiểm tra điều kiện để checked parent */
  checkAllParentsSelection(node: FlatNodeTemplate): void {
    let parent: FlatNodeTemplate | null = this.getParentNode(node);
    while (parent) {
      this.checkRootNodeSelection(parent);
      parent = this.getParentNode(parent);
    }
  }

  /* Tìm parent của children */
  getParentNode(node: FlatNodeTemplate): FlatNodeTemplate | null {
    const currentLevel = this.getLevel(node);
    if (currentLevel < 1) {
      return null;
    }
    const startIndex = this.treeControl.dataNodes.indexOf(node) - 1;
    for (let i = startIndex; i >= 0; i--) {
      const currentNode = this.treeControl.dataNodes[i];
      if (this.getLevel(currentNode) < currentLevel) {
        return currentNode;
      }
    }
    return null;
  }

  /** check để checked parent hoặc không */
  checkRootNodeSelection(node: FlatNodeTemplate): void {
    const nodeSelected = this.checklistSelection.isSelected(node);
    const descendants = this.treeControl.getDescendants(node);
    const descAllSelected =
      descendants.length > 0 &&
      descendants.every(child => {
        return this.checklistSelection.isSelected(child);
      });
    if (nodeSelected && !descAllSelected)/** parent đang checked và tất cả children không checked thì bỏ checked parent */ {
      this.checklistSelection.deselect(node);
    } else if (!nodeSelected && descAllSelected) /** parent không checked và tất cả children đang checked thì checked parent */ {
      this.checklistSelection.select(node);
    } else {
    }
  }

  /** get level của 1 node truyền vào */
  getLevel = (node: FlatNodeTemplate) => node.level;


}
