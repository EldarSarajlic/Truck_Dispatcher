import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AdminLayoutComponent } from './admin-layout/admin-layout.component';
import { ProductsComponent } from './catalogs/products/products.component';
import { ProductsAddComponent } from './catalogs/products/products-add/products-add.component';
import { ProductsEditComponent } from './catalogs/products/products-edit/products-edit.component';
import { ProductCategoriesComponent } from './catalogs/product-categories/product-categories.component';
import {AdminOrdersComponent} from './orders/admin-orders.component';
import {AdminSettingsComponent} from './admin-settings/admin-settings.component';
import { AdminUsersComponent } from './users/admin-users.component';
import { AdminVehiclesComponent } from './vehicles/admin-vehicles.component';
import { DashboardLayoutComponent } from './dashboard/dashboard-layout/dashboard-layout.component';
import { DashboardOverviewComponent } from './dashboard/overview/dashboard-overview/dashboard-overview.component';
import { OrderStatsComponent } from './dashboard/order-stats/order-stats.component';
import { TrucksComponent } from './vehicles/trucks/trucks.component';
import { MapComponent } from './map/map.component';
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
  ],
},

      {
        path: 'users',
        component: AdminUsersComponent
      },
      {
        path: 'map',
        component: MapComponent
      },
      // PRODUCTS
      {
        path: 'products',
        component: ProductsComponent,
      },
      {
        path: 'products/add',
        component: ProductsAddComponent,
      },
      {
        path: 'products/:id/edit',
        component: ProductsEditComponent,
      },

      // PRODUCT CATEGORIES
      {
        path: 'product-categories',
        component: ProductCategoriesComponent,
      },

      {
        path: 'orders',
        component: AdminOrdersComponent,
      },

      {
        path: 'settings',
        component: AdminSettingsComponent,
      },


      // default admin route → /admin/dashboard
      {
        path: '',
        redirectTo: 'dashboard',
        pathMatch: 'full',
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AdminRoutingModule {}
