import { UserRoleModule } from 'src/app/core/common/userRole.pipe';
import { TranslateModule } from '@ngx-translate/core';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FlexLayoutModule } from '@angular/flex-layout';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatChipsModule } from '@angular/material/chips';
import { MatDialogModule } from '@angular/material/dialog';
import { MatDividerModule } from '@angular/material/divider';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatOptionModule } from '@angular/material/core';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSelectModule } from '@angular/material/select';
import { MatTableModule } from '@angular/material/table';
import { MatTooltipModule } from '@angular/material/tooltip';
import { UserPipeModule } from 'src/app/core/common/userPipe.pipe';
import { NgProgressHttpModule } from 'ngx-progressbar/http';
import { MatTreeModule } from '@angular/material/tree';
import {MatProgressBarModule} from '@angular/material/progress-bar'
import {MatTabsModule} from '@angular/material/tabs';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatTableModule,
    MatDividerModule,
    MatPaginatorModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatCheckboxModule,
    MatDialogModule,
    MatSelectModule,
    MatOptionModule,
    MatAutocompleteModule,
    MatTooltipModule,
    ReactiveFormsModule,
    FormsModule,
    FlexLayoutModule,
    UserPipeModule,
    MatChipsModule,
    TranslateModule,
    UserRoleModule,
    MatProgressBarModule,
    MatTabsModule
  ],
  exports:[
    MatCardModule,
    MatButtonModule,
    MatTableModule,
    MatDividerModule,
    MatPaginatorModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatCheckboxModule,
    MatDialogModule,
    MatSelectModule,
    MatOptionModule,
    MatAutocompleteModule,
    MatTooltipModule,
    ReactiveFormsModule,
    FormsModule,
    FlexLayoutModule,
    UserPipeModule,
    MatChipsModule,
    TranslateModule,
    NgProgressHttpModule,
    MatTreeModule,
    UserRoleModule,
    MatProgressBarModule,
    MatTabsModule
  ]
})
export class MaterialModule { }
