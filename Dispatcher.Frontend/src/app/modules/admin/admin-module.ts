import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AdminRoutingModule } from './admin-routing-module';
import { SharedModule } from '../shared/shared-module';
import { AdminLayoutComponent } from './admin-layout/admin-layout.component';
import { AdminUsersComponent } from './users/admin-users.component';
import { AdminVehiclesComponent } from './vehicles/admin-vehicles.component';
import { DashboardLayoutComponent } from './dashboard/dashboard-layout/dashboard-layout.component';
import { DashboardOverviewComponent } from './dashboard/overview/dashboard-overview/dashboard-overview.component';
import { OrderStatsComponent } from './dashboard/order-stats/order-stats.component';
import { TrucksComponent } from './vehicles/trucks/trucks.component';
import { TruckFormModalComponent } from './vehicles/trucks/truck-form-modal/truck-form-modal.component';
import { TrailersComponent } from './vehicles/trailers/trailers.component';
import { TrailerFormModalComponent } from './vehicles/trailers/trailer-form-modal/trailer-form-modal.component';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
@NgModule({
  declarations: [
    AdminLayoutComponent,
    AdminUsersComponent,
    AdminVehiclesComponent,
    DashboardLayoutComponent,
    DashboardOverviewComponent,
    OrderStatsComponent,
    TrucksComponent,
    TruckFormModalComponent,
    TrailersComponent,
    TrailerFormModalComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    AdminRoutingModule,
    SharedModule,
     MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule,
  ],
})
export class AdminModule {}