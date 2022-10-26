import { PaginatorCustomService } from './../../../core/services/paginator-custom.service';
import { NotificationService } from 'src/app/core/services/notification.service';
import { TranslateService } from '@ngx-translate/core';
import { DataService } from 'src/app/core/services/data.service';
import { SelectionModel } from '@angular/cdk/collections';
import { COMMA, ENTER } from '@angular/cdk/keycodes';
import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { AbstractControl, FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { MessageConstants } from 'src/app/core/common/message.constants';
import { FlatTreeControl } from '@angular/cdk/tree';
import { MatTreeFlatDataSource, MatTreeFlattener } from '@angular/material/tree';
import { MenuNode } from 'src/app/core/domain/menuNode';
import { MenuFlatNode } from 'src/app/core/domain/menuFlatNode';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { SystemConstants } from 'src/app/core/common/system.constants';

const user = JSON.parse(localStorage.getItem(SystemConstants.CURRENT_USER)!);

@Component({
  selector: 'app-menu',
  templateUrl: './app-menu.component.html',
  styleUrls: ['./app-menu.component.css']
})
export class AppMenuComponent implements OnInit {

  action: string = '';
  title: string = '';
  displayedColumns: string[] = ['select', 'position', 'menuName', 'createdDate', 'createdBy', 'action',];
  dataSource = new MatTableDataSource<any>();
  preventAbuse = false;
  page = 0;
  keyword: string = '';
  totalRow: number = 0;
  totalPage: number = 0;
  menuParents: any;
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  // @ViewChild(MatSort) sort!: MatSort;
  @ViewChild('dialog') dialogTemplate!: TemplateRef<any>;
  @ViewChild('menuUser') menuUserTemplate!: TemplateRef<any>;

  pageSizeOptions: number[] = [5, 10, 25, 100];
  pageSize = this.pageSizeOptions[0];
  isAllChecked = false;
  selection = new SelectionModel<any>(true, []);
  menuForm!: FormGroup;
  separatorKeysCodes: number[] = [ENTER, COMMA];

  private _transformer = (node: MenuNode, level: number) => {
    return {
      expandable: !!node.childrens && node.childrens.length > 0,
      name: node.menuName,
      level: level,
      id: node.id
    };
  };
  treeControl = new FlatTreeControl<MenuFlatNode>(
    node => node.level,
    node => node.expandable,
  );

  treeFlattener = new MatTreeFlattener(
    this._transformer,
    node => node.level,
    node => node.expandable,
    node => node.childrens
  );
  treeMenus = new MatTreeFlatDataSource(this.treeControl, this.treeFlattener);
  checklistSelection = new SelectionModel<MenuFlatNode>(true /* multiple */);
  parentName: any;
  parentIdControl = new FormControl();
  filteredOptions!: Observable<any[]>;
  users: any;
  userMenuControl = new FormControl();
  userFiltereds!: Observable<any[]>;
  menuUsers:any;

  constructor(private dataService: DataService, private translateService: TranslateService, private notificationService: NotificationService, private pagin: PaginatorCustomService,
    private dialog: MatDialog, private formBuilder: FormBuilder) {
    this.menuForm = this.formBuilder.group({
      id: 0,
      menuName: ['', Validators.compose([Validators.required, Validators.maxLength(50)])],
      parentId: [''],
      createdDate: '',
      createdBy: '',
      updatedDate: '',
      updatedBy: '',
      isDeleted: '',
      icon:['',Validators.compose([Validators.maxLength(100)])],
      link:['',Validators.compose([Validators.maxLength(100)])],
      activeLink:['',Validators.compose([Validators.maxLength(100)])]
    });
    this.filteredOptions = this.parentIdControl.valueChanges.pipe(
      startWith(''),
      map(value => (typeof value === 'string' ? value : value)),
      map(name => (name ? this._filter(name) : this.menuParents.slice())),
    );
    this.userFiltereds = this.userMenuControl.valueChanges.pipe(
      startWith(''),
      map(value => (typeof value === 'string' ? value : value)),
      map(name => (name ? this._userFilter(name) : this.users.slice())));

  }

  ngOnInit(): void {
    this.loadAllMenus();
    this.loadParentMenus();
    this.loadUsers();
    this.loadTreeMenus();
  }

  private _filter(value: any): any[] {
    const filterValue = value.menuName?.toLowerCase();
    return this.menuParents.filter((option: any) => option.menuName?.toLowerCase().includes(filterValue));
  }

  private _userFilter(value: any): any[] {
    const filterValue = value.userName?.toLowerCase();
    return this.users.filter((option: any) => option.userName?.toLowerCase().includes(filterValue));
  }


  loadAllMenus() {
    this.preventAbuse = true;
    this.dataService.get('appmenus/getlistpaging?page=' + this.page + "&pageSize=" + this.pageSize + "&keyword=" + this.keyword).subscribe((data: any) => {
      this.dataSource = new MatTableDataSource(data.items);
      this.totalRow = data.totalCount;
      this.preventAbuse = false;
    }, err => {
      this.translateService.get('messageSystem.loadFail').subscribe(data => MessageConstants.GET_FAILSE_MSG = data);
      this.notificationService.printErrorMessage(MessageConstants.GET_FAILSE_MSG);
      this.preventAbuse = false;
    });
  }

  loadParentMenus() {
    this.dataService.get('appmenus/getall').subscribe((data: any) => {
      this.menuParents = data;
    }, err => {
    });
  }

  loadUsers() {
    this.dataService.get('appusers/getall').subscribe((data: any) => {
      this.users = data;
    }, err => {

    });
  }

  loadTreeMenus(){
    this.dataService.get('appmenus/getmenutree').subscribe((data: any) => {
      this.treeMenus.data = data;
    }, err => {

    });
  }

  openDialog(action: string, item?: any, config?: MatDialogConfig) {
    this.action = action;
    if (action == 'create') {
      this.translateService.get('appMenuModule.createMessage').subscribe(data => this.title = data);
    }
    else {
      this.translateService.get('appMenuModule.editMessage').subscribe(data => this.title = data);
      this.menuForm.controls['id'].setValue(item.id);
      let itemFilter = this.dataSource.filteredData.filter((x: any) => x.id == item.id)[0];
      this.menuForm.controls['menuName'].setValue(itemFilter.menuName);
      this.menuForm.controls['parentId'].setValue(itemFilter.parentId);
      this.menuForm.controls['createdDate'].setValue(itemFilter.createdDate);
      this.menuForm.controls['createdBy'].setValue(itemFilter.createdBy);
      this.menuForm.controls['updatedDate'].setValue(itemFilter.updatedDate);
      this.menuForm.controls['updatedBy'].setValue(itemFilter.updatedBy);
      this.menuForm.controls['icon'].setValue(itemFilter.icon);
      this.menuForm.controls['link'].setValue(itemFilter.link);
      this.menuForm.controls['activeLink'].setValue(itemFilter.activeLink);
      let parentItem = this.menuParents.filter((x: any) => x.id == item.parentId)[0];
      this.parentIdControl.setValue(parentItem?.menuName);
    }
    const dialogRef = this.dialog.open(this.dialogTemplate, {
      width: '750px',
    });
    dialogRef.afterClosed().subscribe(result => {
      this.onReset();
    });

  }

  openAddUserDialog() {
    const dialogRef = this.dialog.open(this.menuUserTemplate, {
      width: '750px',autoFocus:false
    });
    dialogRef.afterClosed().subscribe(result => {
      this.resetUpdateMenuUser();
    });

  }
  removeData() {
    let roleChecked: any[] = [];
    this.selection.selected.forEach((value: any) => {
      let id = value.id;
      roleChecked.push(id);
    });
    this.translateService.get('messageSystem.confirmDelete').subscribe(data => MessageConstants.CONFIRM_DELETE_MSG = data);
    this.notificationService.printConfirmationDialog(MessageConstants.CONFIRM_DELETE_MSG, () => this.deleteItemConfirm(JSON.stringify(roleChecked)));
  }

  deleteItemConfirm(id: string) {
    this.preventAbuse = true;
    this.dataService.delete('appmenus/deletemulti', 'checkedList', id).subscribe(response => {
      this.translateService.get('messageSystem.deleteSuccess').subscribe(data => MessageConstants.DELETED_OK_MSG = data);
      this.notificationService.printSuccessMessage(MessageConstants.DELETED_OK_MSG);
      this.preventAbuse = false;
      this.selection.clear();
      this.loadAllMenus();
      this.loadParentMenus();
    }, err => {
      this.translateService.get('messageSystem.deletefail').subscribe(data => MessageConstants.DELETE_FAILSE_MSG = data);
      this.notificationService.printErrorMessage(MessageConstants.DELETE_FAILSE_MSG);
      this.preventAbuse = false;
    });
  }
  applyFilter(event: Event) {
    this.keyword = (event.target as HTMLInputElement).value;
    this.page = 0;
    this.loadAllMenus();
    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  addData() {
    if(this.menuForm.invalid){
      return;
    }
    this.preventAbuse = true;
    if (this.action == 'create') {
      let role = { menuName: this.menuForm.controls['menuName'].value, parentId: this.menuForm.controls['parentId'].value,icon: this.menuForm.controls['icon'].value,link: this.menuForm.controls['link'].value,activeLink: this.menuForm.controls['activeLink'].value,createdBy: user.id }
      this.dataService.post('appmenus/create', role).subscribe(data => {
        let message!: string;
        this.translateService.get('messageSystem.createSuccess').subscribe(mes => message = mes);
        this.notificationService.printSuccessMessage(message);
        this.preventAbuse = false;
        this.loadAllMenus();
        this.loadParentMenus();
        this.onReset();

      }, err => {
        let error!: string;
        this.translateService.get('messageSystem.createFail').subscribe(mes => error = mes);
        this.notificationService.printErrorMessage(err);
        this.preventAbuse = false;
      });
    }
    else if (this.action == 'edit') {
      let role = { menuName: this.menuForm.controls['menuName'].value, parentId: this.menuForm.controls['parentId'].value, id: this.menuForm.controls['id'].value,icon: this.menuForm.controls['icon'].value,link: this.menuForm.controls['link'].value, activeLink: this.menuForm.controls['activeLink'].value, updatedBy: user.id, createdDate: this.menuForm.controls['createdDate'].value, createdBy: this.menuForm.controls['createdBy'].value }
      this.dataService.put('appmenus/update', role).subscribe(data => {
        let message!: string;
        this.translateService.get('messageSystem.updateSuccess').subscribe(mes => message = mes);
        this.notificationService.printSuccessMessage(message);
        this.preventAbuse = false;
        this.loadAllMenus();
        this.loadParentMenus();
        this.onReset();

      }, err => {
        let error!: string;
        this.translateService.get('messageSystem.updateFail').subscribe(mes => error = mes);
        this.notificationService.printErrorMessage(err);
        this.preventAbuse = false;
      });
    }

  }

  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.data.length;
    return numSelected === numRows;
  }

  /** Selects all rows if they are not all selected; otherwise clear selection. */
  masterToggle() {
    if (this.isAllSelected()) {
      this.selection.clear();
      return;
    }

    this.selection.select(...this.dataSource.data);

  }

  /** The label for the checkbox on the passed row */
  checkboxLabel(row?: any): string {
    if (!row) {
      return `${this.isAllSelected() ? 'deselect' : 'select'} all`;
    }
    return `${this.selection.isSelected(row) ? 'deselect' : 'select'} row ${row.position + 1}`;
  }

  onChangePage(pe: PageEvent) {
    this.page = pe.pageIndex;
    this.pageSize = pe.pageSize;
    this.loadAllMenus();
  }

  descendantsAllSelected(node: MenuFlatNode): boolean {
    const descendants = this.treeControl.getDescendants(node);
    const descAllSelected =
      descendants.length > 0 &&
      descendants.every((child: any) => {
        return this.checklistSelection.isSelected(child);
      });
    return descAllSelected;
  }

  todoItemSelectionToggle(node: MenuFlatNode): void {
    this.checklistSelection.toggle(node);
    const descendants = this.treeControl.getDescendants(node);
    this.checklistSelection.isSelected(node)
      ? this.checklistSelection.select(...descendants)
      : this.checklistSelection.deselect(...descendants);

    // Force update for the parent
    descendants.forEach(child => this.checklistSelection.isSelected(child));
    this.checkAllParentsSelection(node);
  }

  /* Checks all the parents when a leaf node is selected/unselected */
  checkAllParentsSelection(node: MenuFlatNode): void {
    let parent: MenuFlatNode | null = this.getParentNode(node);
    while (parent) {
      this.checkRootNodeSelection(parent);
      parent = this.getParentNode(parent);
    }
  }

  getLevel = (node: MenuFlatNode) => node.level;

  /* Get the parent node of a node */
  getParentNode(node: MenuFlatNode): MenuFlatNode | null {
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

  /** Check root node checked state and change it accordingly */
  checkRootNodeSelection(node: MenuFlatNode): void {
    const nodeSelected = this.checklistSelection.isSelected(node);
    const descendants = this.treeControl.getDescendants(node);
    const descAllSelected =
      descendants.length > 0 &&
      descendants.every(child => {
        return this.checklistSelection.isSelected(child);
      });
    if (nodeSelected && !descAllSelected) {
      this.checklistSelection.deselect(node);
    } else if (!nodeSelected && descAllSelected) {
      this.checklistSelection.select(node);
    }
  }

  descendantsPartiallySelected(node: MenuFlatNode): boolean {
    const descendants = this.treeControl.getDescendants(node);
    const result = descendants.some(child => this.checklistSelection.isSelected(child));
    return result && !this.descendantsAllSelected(node);
  }

  /** Toggle a leaf to-do item selection. Check all the parents to see if they changed */
  todoLeafItemSelectionToggle(node: MenuFlatNode): void {
    this.checklistSelection.toggle(node);
    this.checkAllParentsSelection(node);
  }

  hasChild = (_: number, node: MenuFlatNode) => node.expandable;

  get getValidForm(): { [key: string]: AbstractControl } {
    return this.menuForm.controls;
  }

  ngAfterViewInit() {
    this.paginator._intl.itemsPerPageLabel = this.pagin.setLable;
    this.paginator._intl.firstPageLabel = this.pagin.firstButton;
    this.paginator._intl.nextPageLabel = this.pagin.nextButton;
    this.paginator._intl.lastPageLabel = this.pagin.lastButton;
    this.paginator._intl.previousPageLabel = this.pagin.preButton;
  }

  onReset() {
    this.action == '';
    this.dialog.closeAll();
    this.menuForm.reset();
    this.parentIdControl.reset();

  }

  getParentId(event: any) {
    this.menuForm.controls['parentId'].setValue(event.id);
  }

  getMenuUser(option:any){
    this.dataService.get('appmenus/getmenuuser?id='+option.id).subscribe((data: any) => {
      this.menuUsers = data;
      this.setCheckedMenuNode();
    }, err => {
    });
  }

  resetSelection() {
    this.treeControl.dataNodes.forEach((element: any) => {
      this.checklistSelection.deselect(element);
    })
  }

  setCheckedMenuNode() {
    //reset checklistSelection
    this.resetSelection();

    //check nếu có tồn tại quyền trong group thì add vào checklistSelection
    this.menuUsers.forEach((value: any) => {
      let select = this.treeControl.dataNodes.filter((x: any) => x.id == value.id)[0];
      //this.todoLeafItemSelectionToggle(select);
      this.checklistSelection.toggle(select);

    });

    this.menuUsers.forEach((element: any) => {
      let select = this.treeControl.dataNodes.filter((x: any) => x.id == element.id)[0];
      this.checkAllParentsSelection(select);
    });
  }

  updateUserMenu(){
    this.preventAbuse=true;
    let userMenu={userId:this.getUserId(this.userMenuControl.value),appMenus:this.getMenuSelected()};
    this.dataService.post('appmenuusers/create', userMenu).subscribe(data=>{
      let message!: string;
      this.translateService.get('messageSystem.updateSuccess').subscribe(mes => message = mes);
      this.notificationService.printSuccessMessage(message);
      this.preventAbuse = false;
    }, err => {
      let error!: string;
      this.translateService.get('messageSystem.updateFail').subscribe(mes => error = mes);
      this.notificationService.printErrorMessage(err);
      this.preventAbuse = false;
    });
  }

  getUserId(userName:string):string{
    let user= this.users.filter((x:any)=>x.userName.toLowerCase()==userName.toLocaleLowerCase())[0];
    return user?.id;
  }
  getMenuSelected():any[]{
    let strMenus: any[] = [];
    if (this.checklistSelection.selected.length > 0) {
      this.checklistSelection.selected.forEach((value: any) => {
        let menuId = { id: '' }
        menuId.id = value.id;
        strMenus.push(menuId);
      })
    }
    return strMenus;
  }

  resetUpdateMenuUser(){
    this.userMenuControl.reset();
    this.checklistSelection.clear();
  }
}
