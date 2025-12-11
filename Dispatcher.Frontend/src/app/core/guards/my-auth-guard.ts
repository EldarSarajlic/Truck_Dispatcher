// src/app/core/guards/auth.guard.ts
import {inject} from '@angular/core';
import {ActivatedRouteSnapshot, CanActivateFn, Router} from '@angular/router';
import {CurrentUserService} from '../services/auth/current-user.service';
import {UserRole} from '../services/auth/current-user.dto';

export const myAuthGuard: CanActivateFn = (route: ActivatedRouteSnapshot) => {
  const currentUser = inject(CurrentUserService);
  const router = inject(Router);

  const requireAuth = route.data['requireAuth'] === true;
  const requireAdmin = route.data['requireAdmin'] === true;
  const requireDispatcher = route.data['requireDispatcher'] === true;
  const requireDriver = route.data['requireDriver'] === true;
  const requireClient = route.data['requireClient'] === true;

  const isAuth = currentUser.isAuthenticated();

  // 1) ako ruta traži auth, a user nije logiran → login
  if (requireAuth && !isAuth) {
    router.navigate(['/auth/login']);
    return false;
  }

  // Ako ne traži auth → pusti (javne rute)
  if (!requireAuth) {
    return true;
  }

  // 2) role check – admin > manager > employee
  const user = currentUser.snapshot;
  if (!user) {
    router.navigate(['/auth/login']);
    return false;
  }

  if (requireAdmin && user.role !== UserRole.Admin) {
    router.navigate([currentUser.getDefaultRoute()]);
    return false;
  }

  if (requireDispatcher && user.role !== UserRole.Dispatcher) {
    router.navigate([currentUser.getDefaultRoute()]);
    return false;
  }

  if (requireClient && user.role !== UserRole.Client) {
    router.navigate([currentUser.getDefaultRoute()]);
    return false;
  }

  if (requireDriver && user.role !== UserRole.Driver) {
    router.navigate([currentUser.getDefaultRoute()]);
    return false;
  }

  return true;
};

export interface MyAuthRouteData {
  requireAuth?: boolean;
  requireAdmin?: boolean;
  requireDispatcher?: boolean;
  requireClient?: boolean;
  requireDriver?: boolean;
}

export function myAuthData(data: MyAuthRouteData): { auth: MyAuthRouteData } {
  return { auth: data };
}
