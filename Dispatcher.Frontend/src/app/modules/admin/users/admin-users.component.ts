import { Component, OnInit, OnDestroy, HostListener, inject, signal } from '@angular/core';
import { AbstractControl, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { Subject } from 'rxjs';
import { takeUntil, debounceTime, distinctUntilChanged } from 'rxjs/operators';

import { ListUsersRequest, ListUsersQueryDto } from '../../../api-services/users/users-api.model';
import { UsersApiService } from '../../../api-services/users/users-api.service';
import { AuthApiService } from '../../../api-services/auth/auth-api.service';
import { LocationsApiService } from '../../../api-services/locations/locations-api.service';
import { CountryDto } from '../../../api-services/locations/locations-api.model';
import { FormField } from '../../shared/components/form-modal/form-modal.component';
import { ToasterService } from '../../../core/services/toaster.service';

// X = one digit; all other chars are literal separators
const PHONE_FORMAT: Record<string, string> = {
  AD: '+XXX XXX XXX',            AE: '+XXX XX XXX XXXX',
  AL: '+XXX XX XXX XXXX',        AM: '+XXX XX XXX XXX',
  AT: '+XX XXX XXXXXXX',         AU: '+XX XXX XXX XXX',
  AZ: '+XXX XX XXX XXXX',        BA: '+XXX XX XXX XXX',
  BE: '+XX XXX XX XX XX',        BG: '+XXX XX XXX XXXX',
  BR: '+XX XX X XXXX XXXX',      BY: '+XXX XX XXX XXXX',
  CA: '+X XXX XXX XXXX',         CH: '+XX XX XXX XXXX',
  CY: '+XXX XX XXX XXX',         CZ: '+XXX XXX XXX XXX',
  DE: '+XX XXX XXXX XXXX',       DK: '+XX XX XX XX XX',
  EE: '+XXX XXXX XXXX',          EG: '+XX XXX XXX XXXX',
  ES: '+XX XXX XXX XXX',         FI: '+XXX XX XXX XXXX',
  FR: '+XX X XX XX XX XX',       GB: '+XX XXXX XXX XXX',
  GE: '+XXX XXX XX XX XX',       GR: '+XX XXX XXX XXXX',
  HR: '+XXX XX XXX XXXX',        HU: '+XX XX XXX XXXX',
  ID: '+XX XXX XXXX XXXX',       IE: '+XXX XX XXX XXXX',
  IL: '+XXX XX XXX XXXX',        IN: '+XX XXXXX XXXXX',
  IQ: '+XXX XXX XXX XXXX',       IT: '+XX XXX XXX XXXX',
  JP: '+XX XX XXXX XXXX',        KZ: '+X XXX XXX XXXX',
  LT: '+XXX XXX XXXXX',          LU: '+XXX XXX XXX XXX',
  LV: '+XXX XXXX XXXX',          MA: '+XXX XXX XXX XXX',
  MD: '+XXX XXX XX XXX',         ME: '+XXX XX XXX XXX',
  MK: '+XXX XX XXX XXX',         MT: '+XXX XXXX XXXX',
  NL: '+XX X XXXXXXXX',          NO: '+XX XXX XX XXX',
  NZ: '+XX XX XXX XXXX',         PL: '+XX XXX XXX XXX',
  PT: '+XXX XXX XXX XXX',        RO: '+XX XXX XXX XXX',
  RS: '+XXX XX XXX XXXX',        RU: '+X XXX XXX XXXX',
  SA: '+XXX XX XXX XXXX',        SE: '+XX XX XXX XX XX',
  SI: '+XXX XX XXX XXX',         SK: '+XXX XXX XXX XXX',
  TR: '+XX XXX XXX XXXX',        UA: '+XXX XX XXX XXXX',
  US: '+X XXX XXX XXXX',         XK: '+XXX XX XXX XXX',
};

function passwordMatchValidator(group: AbstractControl): ValidationErrors | null {
  const pw  = group.get('password')?.value;
  const cpw = group.get('confirmPassword')?.value;
  if (pw && cpw && pw !== cpw) {
    group.get('confirmPassword')?.setErrors({ passwordMismatch: true });
    return { passwordMismatch: true };
  }
  return null;
}

@Component({
  selector: 'app-admin-users',
  standalone: false,
  templateUrl: './admin-users.component.html',
  styleUrl: './admin-users.component.scss',
})
export class AdminUsersComponent implements OnInit, OnDestroy {

  // ── 1. Signals (state) ────────────────────────────────────────────────────
  readonly users        = signal<ListUsersQueryDto[]>([]);
  readonly isLoading    = signal(true);
  readonly hasError     = signal(false);
  readonly totalItems   = signal(0);
  readonly totalPages   = signal(0);
  readonly currentPage  = signal(1);
  readonly pageSize     = signal(5);
  readonly selectedRole = signal('');

  readonly registerModalOpen      = signal(false);
  readonly isRegisterSubmitting   = signal(false);
  readonly registerSubmitError    = signal<string | null>(null);

  // ── 2. Local mutable state ────────────────────────────────────────────────
  searchTerm = '';

  readonly roles = ['Admin', 'Dispatcher', 'Driver', 'Client'];
  roleDropdownOpen = false;

  // ── 3. Register form definition (signal so options can update dynamically) ─
  readonly registerFields = signal<FormField[]>([
    {
      name: 'firstName', label: 'First Name', type: 'text',
      placeholder: 'Enter first name', required: true,
      validators: [Validators.maxLength(100)], halfWidth: true,
    },
    {
      name: 'lastName', label: 'Last Name', type: 'text',
      placeholder: 'Enter last name', required: true,
      validators: [Validators.maxLength(100)], halfWidth: true,
    },
    {
      name: 'email', label: 'Email', type: 'email',
      placeholder: 'user@example.com', required: true,
      validators: [Validators.email],
    },
    {
      name: 'country', label: 'Country', type: 'select',
      placeholder: 'Select country...', required: true,
      options: [],
      onValueChange: (value, form) => this.onCountryChange(Number(value), form),
    },
    {
      name: 'phoneNumber', label: 'Phone Number', type: 'tel',
      placeholder: 'Select a country first',
    },
    {
      name: 'city', label: 'City', type: 'select',
      placeholder: 'Select city...', required: true,
      options: [],
    },
    {
      name: 'role', label: 'Role', type: 'select',
      placeholder: 'Select role...', required: true,
      options: [
        { label: 'Admin',      value: 3 },
        { label: 'Dispatcher', value: 2 },
        { label: 'Driver',     value: 1 },
        { label: 'Client',     value: 0 },
      ],
    },
    {
      name: 'dateOfBirth', label: 'Date of Birth', type: 'date',
      required: true,
    },
    {
      name: 'password', label: 'Password', type: 'password',
      placeholder: 'Min. 6 characters', required: true,
      validators: [Validators.minLength(6)],
    },
    {
      name: 'confirmPassword', label: 'Confirm Password', type: 'password',
      placeholder: 'Repeat password', required: true,
    },
  ]);

  readonly registerGroupValidators: ValidatorFn[] = [passwordMatchValidator];

  // ── 4. Private cleanup handles ────────────────────────────────────────────
  private readonly destroyed$    = new Subject<void>();
  private readonly searchSubject = new Subject<string>();

  // ── 5. Dependencies ───────────────────────────────────────────────────────
  private readonly userService      = inject(UsersApiService);
  private readonly authService      = inject(AuthApiService);
  private readonly locationsService = inject(LocationsApiService);
  private readonly toast            = inject(ToasterService);

  // ── 6. Cached country list ────────────────────────────────────────────────
  private countries: CountryDto[] = [];

  constructor() {
    this.searchSubject.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      takeUntil(this.destroyed$),
    ).subscribe(term => this.loadUsers(1, term));
  }

  // ── 7. Lifecycle hooks ────────────────────────────────────────────────────
  ngOnInit(): void {
    this.loadUsers();
    this.loadCountries();
  }

  ngOnDestroy(): void {
    this.destroyed$.next();
    this.destroyed$.complete();
  }

  // ── 8. Data loading ───────────────────────────────────────────────────────
  loadUsers(page = 1, search?: string): void {
    this.isLoading.set(true);
    this.hasError.set(false);

    const request = new ListUsersRequest();
    request.paging.page     = page;
    request.paging.pageSize = 5;
    if (search !== undefined) request.search = search;
    request.role = this.selectedRole() || null;

    this.userService.getUsers(request)
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
        next: res => {
          this.users.set(res.items);
          this.totalItems.set(res.totalItems);
          this.totalPages.set(res.totalPages);
          this.currentPage.set(res.currentPage);
          this.pageSize.set(res.pageSize);
          this.isLoading.set(false);
        },
        error: () => {
          this.hasError.set(true);
          this.isLoading.set(false);
        },
      });
  }

  private loadCountries(): void {
    this.locationsService.getCountries()
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
        next: countries => {
          this.countries = countries;
          this.patchField('country', {
            options: countries.map(c => ({ label: c.name, value: c.id })),
          });
        },
      });
  }

  // ── 9. Public template methods ────────────────────────────────────────────
  @HostListener('document:click')
  closeDropdowns(): void { this.roleDropdownOpen = false; }

  toggleRoleDropdown(e: Event): void {
    e.stopPropagation();
    this.roleDropdownOpen = !this.roleDropdownOpen;
  }

  selectRole(role: string, e: Event): void {
    e.stopPropagation();
    this.selectedRole.set(role);
    this.roleDropdownOpen = false;
    this.loadUsers(1, this.searchTerm);
  }

  clearRole(e: Event): void {
    e.stopPropagation();
    this.selectedRole.set('');
    this.roleDropdownOpen = false;
    this.loadUsers(1, this.searchTerm);
  }

  onSearchInput(): void            { this.searchSubject.next(this.searchTerm); }
  onSearch(): void                 { this.loadUsers(1, this.searchTerm); }
  onPageChange(page: number): void { this.loadUsers(page, this.searchTerm); }

  // ── 10. Register modal ─────────────────────────────────────────────────────
  openRegisterModal(): void  { this.registerModalOpen.set(true); }
  closeRegisterModal(): void { this.registerModalOpen.set(false); }

  onFormSubmitted(values: Record<string, any>): void {
    this.isRegisterSubmitting.set(true);
    this.registerSubmitError.set(null);

    this.authService.register({
      firstName:       values['firstName'].trim(),
      lastName:        values['lastName'].trim(),
      displayName:     `${values['firstName'].trim()} ${values['lastName'].trim()}`,
      email:           values['email'].trim(),
      password:        values['password'],
      confirmPassword: values['confirmPassword'],
      phoneNumber:     values['phoneNumber']?.trim() || null,
      dateOfBirth:     values['dateOfBirth'],
      role:            Number(values['role']),
      cityId:          values['city'] ? Number(values['city']) : null,
    }).pipe(takeUntil(this.destroyed$))
      .subscribe({
        next: () => {
          this.isRegisterSubmitting.set(false);
          this.registerModalOpen.set(false);
          this.loadUsers(1, this.searchTerm);
          this.toast.success('User registered successfully.');
        },
        error: (err) => {
          this.isRegisterSubmitting.set(false);
          const message = err?.error?.message ?? err?.error?.title ?? 'Registration failed. Please try again.';
          this.registerSubmitError.set(message);
          this.toast.error(message);
        },
      });
  }

  // ── 11. Country change handler ────────────────────────────────────────────
  private onCountryChange(countryId: number, form: any): void {
    const country = this.countries.find(c => c.id === countryId);
    if (country) {
      const fmt = PHONE_FORMAT[country.countryCode];
      // Pre-fill phone code digits so formatter can seat them correctly
      const seed = country.phoneCode.replace(/\D/g, '');
      const preFormatted = fmt ? this.applyMask(seed, fmt) : country.phoneCode + ' ';
      form.get('phoneNumber')?.setValue(preFormatted);
      this.patchField('phoneNumber', { phoneFormat: fmt, phonePrefix: seed });
    }

    // Reset city and show loading
    form.get('city')?.setValue('');
    this.patchField('city', { isLoadingOptions: true, options: [] });

    this.locationsService.getCitiesByCountry(countryId)
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
        next: cities => {
          this.patchField('city', {
            isLoadingOptions: false,
            options: cities.map(c => ({ label: c.name, value: c.id })),
          });
        },
        error: () => {
          this.patchField('city', { isLoadingOptions: false, options: [] });
        },
      });
  }

  // ── 12. Class helpers ─────────────────────────────────────────────────────
  roleClass(role: string): string {
    return ({
      'Admin':      'role--admin',
      'Dispatcher': 'role--dispatcher',
      'Driver':     'role--driver',
      'Client':     'role--client',
    } as Record<string, string>)[role] ?? '';
  }

  statusClass(isEnabled: boolean): string {
    return isEnabled ? 'status--active' : 'status--inactive';
  }

  // ── Private helpers ───────────────────────────────────────────────────────
  private applyMask(digits: string, format: string): string {
    let di = 0, result = '';
    for (const ch of format) {
      if (di >= digits.length) break;
      result += ch === 'X' ? digits[di++] : ch;
    }
    return result;
  }

  private patchField(name: string, updates: Partial<FormField>): void {
    this.registerFields.update(fields =>
      fields.map(f => f.name === name ? { ...f, ...updates } : f)
    );
  }
}
