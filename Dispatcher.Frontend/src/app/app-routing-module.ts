import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {myAuthData, myAuthGuard} from './core/guards/my-auth-guard';
import { NotFoundComponent } from './modules/not-found/not-found.component';

const routes: Routes = [
  {
    path: 'admin',
    canActivate: [myAuthGuard],
    data: myAuthData({ requireAuth: true, requireAdmin: true }),
    loadChildren: () =>
      import('./modules/admin/admin-module').then(m => m.AdminModule)
  },
  {
    path: 'auth',
    loadChildren: () =>
      import('./modules/auth/auth-module').then(m => m.AuthModule)
  },
  {
    path: 'client',
    canActivate: [myAuthGuard],
    data: myAuthData({ requireAuth: true }),// bilo ko logiran
    loadChildren: () =>
      import('./modules/client/client-module').then(m => m.ClientModule)
  },
  {
    path: '',
    loadChildren: () =>
      import('./modules/auth/auth-module').then(m => m.AuthModule)
  },
  {
    path: 'settings',
    canActivate: [myAuthGuard],
    data: myAuthData({ requireAuth: true }),
    loadChildren: () =>
      import('./modules/settings/settings-module').then(m => m.SettingsModule)
  },
  // named route so child modules can redirect here
  { path: 'not-found', component: NotFoundComponent },
  // fallback 404
  { path: '**', component: NotFoundComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
