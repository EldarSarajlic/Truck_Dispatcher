import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { CurrentUserService } from '../services/auth/current-user.service';

/**
 * Used on the root '' path.
 * Always redirects: authenticated users go to their dashboard, others go to login.
 */
export const rootRedirectGuard: CanActivateFn = () => {
  const currentUser = inject(CurrentUserService);
  const router = inject(Router);

  if (currentUser.isAuthenticated()) {
    return router.createUrlTree([currentUser.getDefaultRoute()]);
  }
  return router.createUrlTree(['/auth/login']);
};

/**
 * Used on auth routes (login, forgot-password).
 * Prevents already-authenticated users from accessing login — sends them to their dashboard.
 */
export const redirectIfAuthenticatedGuard: CanActivateFn = () => {
  const currentUser = inject(CurrentUserService);
  const router = inject(Router);

  if (currentUser.isAuthenticated()) {
    return router.createUrlTree([currentUser.getDefaultRoute()]);
  }
  return true;
};
