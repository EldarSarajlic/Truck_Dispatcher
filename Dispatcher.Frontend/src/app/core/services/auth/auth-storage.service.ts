import { Injectable } from '@angular/core';
import {
  LoginCommandDto,
  RefreshTokenCommandDto
} from '../../../api-services/auth/auth-api.model';

/**
 * Low-level service for managing auth tokens in localStorage or sessionStorage.
 * Storage type is determined at login time by the "Remember Me" flag.
 * Should not be used directly in components - use AuthFacadeService instead.
 */
@Injectable({
  providedIn: 'root'
})
export class AuthStorageService {
  private readonly ACCESS_TOKEN_KEY = 'accessToken';
  private readonly REFRESH_TOKEN_KEY = 'refreshToken';
  private readonly ACCESS_EXPIRES_KEY = 'accessTokenExpiresAtUtc';
  private readonly REFRESH_EXPIRES_KEY = 'refreshTokenExpiresAtUtc';

  /** Returns the storage that currently holds an access token, or null. */
  private get activeStorage(): Storage | null {
    if (localStorage.getItem(this.ACCESS_TOKEN_KEY)) return localStorage;
    if (sessionStorage.getItem(this.ACCESS_TOKEN_KEY)) return sessionStorage;
    return null;
  }

  /**
   * Save login response.
   * @param response - login response from server
   * @param rememberMe - true → localStorage (persists across sessions), false → sessionStorage (cleared on tab/browser close)
   */
  saveLogin(response: LoginCommandDto, rememberMe: boolean): void {
    const storage = rememberMe ? localStorage : sessionStorage;
    storage.setItem(this.ACCESS_TOKEN_KEY, response.accessToken);
    storage.setItem(this.REFRESH_TOKEN_KEY, response.refreshToken);
    storage.setItem(this.ACCESS_EXPIRES_KEY, response.expiresAtUtc);
  }

  /**
   * Save refresh response to whichever storage was used at login.
   */
  saveRefresh(response: RefreshTokenCommandDto): void {
    const storage = this.activeStorage ?? localStorage;
    storage.setItem(this.ACCESS_TOKEN_KEY, response.accessToken);
    storage.setItem(this.REFRESH_TOKEN_KEY, response.refreshToken);
    storage.setItem(this.ACCESS_EXPIRES_KEY, response.accessTokenExpiresAtUtc);
    storage.setItem(this.REFRESH_EXPIRES_KEY, response.refreshTokenExpiresAtUtc);
  }

  /**
   * Clear all auth data from both storages.
   */
  clear(): void {
    [localStorage, sessionStorage].forEach(s => {
      s.removeItem(this.ACCESS_TOKEN_KEY);
      s.removeItem(this.REFRESH_TOKEN_KEY);
      s.removeItem(this.ACCESS_EXPIRES_KEY);
      s.removeItem(this.REFRESH_EXPIRES_KEY);
    });
  }

  /**
   * Get access token (checks localStorage first, then sessionStorage).
   */
  getAccessToken(): string | null {
    return this.activeStorage?.getItem(this.ACCESS_TOKEN_KEY) ?? null;
  }

  /**
   * Get refresh token from whichever storage holds the session.
   */
  getRefreshToken(): string | null {
    return this.activeStorage?.getItem(this.REFRESH_TOKEN_KEY) ?? null;
  }

  /**
   * Check if user has access token.
   */
  hasToken(): boolean {
    return !!this.getAccessToken();
  }
}
