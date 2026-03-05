import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminLayoutComponent } from './admin-layout/admin-layout.component';
import { AdminUsersComponent } from './users/admin-users.component';
import { AdminVehiclesComponent } from './vehicles/admin-vehicles.component';
import { DashboardLayoutComponent } from './dashboard/dashboard-layout/dashboard-layout.component';
import { DashboardOverviewComponent } from './dashboard/overview/dashboard-overview/dashboard-overview.component';
import { OrderStatsComponent } from './dashboard/order-stats/order-stats.component';
import { TrucksComponent } from './vehicles/trucks/trucks.component';
import { TrailersComponent } from './vehicles/trailers/trailers.component';
const routes: Routes = [
  {
    path: '',
    component: AdminLayoutComponent,
    children: [

      {
    path: 'dashboard',
    component: DashboardLayoutComponent,
    children: [
    { path: '', component: DashboardOverviewComponent },
    {path:'orders',component: OrderStatsComponent}
    ],
    },



    {
     path: 'vehicles',
      component: AdminVehiclesComponent,
       children: [
        { path: 'trucks', component: TrucksComponent },
        { path: '', redirectTo: 'trucks', pathMatch: 'full' },
        {path: 'trailers', component: TrailersComponent},
  ],
},

      {
        path: 'users',
        component: AdminUsersComponent
      },
      // default admin route → /admin/dashboard
      {
        path: '',
        redirectTo: 'dashboard',
        pathMatch: 'full',
      },

      // unmatched admin routes → 404
      { path: '**', redirectTo: '/not-found' },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AdminRoutingModule {}
