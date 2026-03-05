import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {FitPaginatorBarComponent} from './components/fit-paginator-bar/fit-paginator-bar.component';
import {materialModules} from './material-modules';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {TranslateModule} from '@ngx-translate/core';
import { FitConfirmDialogComponent } from './components/fit-confirm-dialog/fit-confirm-dialog.component';
import {DialogHelperService} from './services/dialog-helper.service';
import { FitLoadingBarComponent } from './components/fit-loading-bar/fit-loading-bar.component';
import { FitTableSkeletonComponent } from './components/fit-table-skeleton/fit-table-skeleton.component';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { RouterModule } from '@angular/router';        


@NgModule({
  declarations: [
    FitPaginatorBarComponent,
    FitConfirmDialogComponent,
    FitLoadingBarComponent,
    FitTableSkeletonComponent,
    SidebarComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    TranslateModule,
     RouterModule,       
    ...materialModules
  ],
  providers: [
    DialogHelperService
  ],
  exports:[
    FitPaginatorBarComponent,
    CommonModule,
    ReactiveFormsModule,
    TranslateModule,
    FormsModule,
    FitLoadingBarComponent,
    FitTableSkeletonComponent,
    RouterModule,       
    SidebarComponent,   
    materialModules
  ]
})
export class SharedModule { }
