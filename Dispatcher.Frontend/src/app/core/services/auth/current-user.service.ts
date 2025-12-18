import { computed, inject, Injectable } from '@angular/core';
import { AuthFacadeService } from './auth-facade.service';
import { UserRole } from './current-user.dto';

@Injectable({ providedIn: 'root' })
export class CurrentUserService {
  private auth = inject(AuthFacadeService);

  /** Signal that UI can read (readonly) */
  readonly currentUser = computed(() => this.auth.currentUser());

  readonly isAuthenticated = computed(() => this.auth.isAuthenticated());
  readonly isAdmin = computed(() => this.auth.isAdmin());
  readonly isDispatcher = computed(() => this.auth.isDispatcher());
  readonly isClient = computed(() => this.auth.isClient());
  readonly isDriver = computed(() => this.auth.isDriver());

  /**
   * Get current snapshot (non-reactive)
   * Use this when you need immediate value without reactivity
   */
  get snapshot() {
    return this.auth.currentUser(); // ← Call with ()
  }

  /**
   * Get default route based on user role
   */
  getDefaultRoute(): string {
    const user = this.snapshot;
    if (!user) return '/auth/login';

    switch (user.role) {
      case UserRole.Admin:
        return '/admin';
      case UserRole.Dispatcher:
        return '/dispatcher';
      case UserRole.Driver:
        return '/driver';
      case UserRole.Client:
        return '/client';
      default:
        return '/';
    }
  }
}
