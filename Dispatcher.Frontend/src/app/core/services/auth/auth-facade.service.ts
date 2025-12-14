import { Injectable, inject, signal, computed } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, of, tap, catchError, map } from 'rxjs';
import { jwtDecode } from 'jwt-decode';

import { AuthApiService } from '../../../api-services/auth/auth-api.service';
import {
  LoginCommand,
  LoginCommandDto,
  LogoutCommand,
  RefreshTokenCommand,
  RefreshTokenCommandDto,
} from '../../../api-services/auth/auth-api.model';

import { AuthStorageService } from './auth-storage.service';
import { CurrentUserDto, UserRole } from './current-user.dto';
import { JwtPayloadDto } from './jwt-payload.dto';

@Injectable({ providedIn: 'root' })
export class AuthFacadeService {
  private api = inject(AuthApiService);
  private storage = inject(AuthStorageService);
  private router = inject(Router);

  // ========================================
  // REACTIVE STATE: Current User
  // ========================================

  private _currentUser = signal<CurrentUserDto | null>(null);

  /** Readonly signal for UI – read as auth.currentUser() */
  readonly currentUser = this._currentUser.asReadonly();

  /** Computed signals based on current user */
  readonly isAuthenticated = computed(() => !!this._currentUser());
  readonly isAdmin = computed(() => this._currentUser()?.role === UserRole.Admin);
  readonly isDispatcher = computed(() => this._currentUser()?.role === UserRole.Dispatcher);
  readonly isDriver = computed(() => this._currentUser()?.role === UserRole.Driver);
  readonly isClient = computed(() => this._currentUser()?.role === UserRole.Client);

  constructor() {
    // Try to restore user from existing token on app startup
    this.initializeFromToken();
  }

  // ========================================
  // PUBLIC API
  // ========================================

  login(payload: LoginCommand): Observable<void> {
    return this.api.login(payload).pipe(
      tap((response: LoginCommandDto) => {
        this.storage.saveLogin(response);
        this.decodeAndSetUser(response.accessToken);
      }),
      map(() => void 0)
    );
  }

  logout(): Observable<void> {
    const refreshToken = this.storage.getRefreshToken();
    this.clearUserState();

    if (!refreshToken) {
      return of(void 0);
    }

    const payload: LogoutCommand = { refreshToken };
    return this.api.logout(payload).pipe(
      catchError(() => of(void 0))
    );
  }

  refresh(payload: RefreshTokenCommand): Observable<RefreshTokenCommandDto> {
    return this.api.refresh(payload).pipe(
      tap((response: RefreshTokenCommandDto) => {
        this.storage.saveRefresh(response);
        this.decodeAndSetUser(response.accessToken);
      })
    );
  }

  redirectToLogin(): void {
    this.clearUserState();
    this.router.navigate(['/auth/login']);
  }

  getAccessToken(): string | null {
    return this.storage.getAccessToken();
  }

  getRefreshToken(): string | null {
    return this.storage.getRefreshToken();
  }

  // ========================================
  // PRIVATE HELPERS
  // ========================================

  private initializeFromToken(): void {
    const token = this.storage.getAccessToken();
    if (token) {
      this.decodeAndSetUser(token);
    }
  }

  private decodeAndSetUser(token: string): void {
    try {
      const payload = jwtDecode<JwtPayloadDto>(token);

      let role: UserRole;
      switch (payload.role) {
        case 'Admin':
          role = UserRole.Admin;
          break;
        case 'Dispatcher':
          role = UserRole.Dispatcher;
          break;
        case 'Driver':
          role = UserRole.Driver;
          break;
        case 'Client':
        default:
          role = UserRole.Client;
          break;
      }

      const user: CurrentUserDto = {
        userId: Number(payload.sub),
        email: payload.email,
        role: role,
        tokenVersion: Number(payload.ver),
      };

      this._currentUser.set(user);
    } catch (error) {
      console.error('Failed to decode JWT token:', error);
      this._currentUser.set(null);
    }
  }

  private clearUserState(): void {
    this._currentUser.set(null);
    this.storage.clear();
  }
}
