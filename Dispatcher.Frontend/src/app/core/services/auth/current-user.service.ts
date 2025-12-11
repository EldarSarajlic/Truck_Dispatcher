// src/app/core/services/auth/current-user.service.ts
import {computed, inject, Injectable} from '@angular/core';
import {AuthFacadeService} from './auth-facade.service';
import {UserRole} from './current-user.dto';

@Injectable({ providedIn: 'root' })
export class CurrentUserService {
  private auth = inject(AuthFacadeService);

  /** Signal koji UI može čitati (readonly) */
  currentUser = computed(() => this.auth.currentUser());

  isAuthenticated = computed(() => this.auth.isAuthenticated());
  isAdmin = computed(() => this.auth.isAdmin());
  isDispatcher = computed(() => this.auth.isDispatcher());
  isClient = computed(() => this.auth.isClient());
  isDriver = computed(() => this.auth.isDriver());

  get snapshot() {
    return this.auth.currentUser();
  }

  /** Pravilo: admin > ostali → client */
  getDefaultRoute(): string {
    const user = this.snapshot;
    if (!user) return '/auth/login';

    switch (user.role) {
      case UserRole.Admin: return '/admin';
      case UserRole.Dispatcher: return '/admin';
      case UserRole.Driver: return '/driver';
      case UserRole.Client: return '/client';
      default: return '/client';
    }
  }
}
