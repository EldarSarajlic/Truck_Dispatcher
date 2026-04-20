import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PaginatorBarComponent } from './components/paginator-bar/paginator-bar.component';
import { materialModules } from './material-modules';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { ConfirmDialogComponent } from './components/confirm-dialog/confirm-dialog.component';
import { DialogHelperService } from './services/dialog-helper.service';
import { LoadingBarComponent } from './components/loading-bar/loading-bar.component';
import { TableSkeletonComponent } from './components/table-skeleton/table-skeleton.component';
import { MetricCardComponent } from './components/metric-card/metric-card.component';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { FormModalComponent } from './components/form-modal/form-modal.component';
import { DatePickerComponent } from './components/date-picker/date-picker.component';
import { WizardFormModalComponent } from './components/wizard-form-modal/wizard-form-modal.component';
import { RouterModule } from '@angular/router';


@NgModule({
  declarations: [
    PaginatorBarComponent,
    ConfirmDialogComponent,
    LoadingBarComponent,
    TableSkeletonComponent,
    MetricCardComponent,
    SidebarComponent,
    FormModalComponent,
    DatePickerComponent,
    WizardFormModalComponent,
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
  exports: [
    PaginatorBarComponent,
    CommonModule,
    ReactiveFormsModule,
    TranslateModule,
    FormsModule,
    LoadingBarComponent,
    TableSkeletonComponent,
    MetricCardComponent,
    RouterModule,
    SidebarComponent,
    FormModalComponent,
    DatePickerComponent,
    WizardFormModalComponent,
    materialModules
  ]
})
export class SharedModule { }
