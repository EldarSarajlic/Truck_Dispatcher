import { Component, inject, OnDestroy, OnInit, Renderer2, signal } from '@angular/core';
import { AbstractControl, FormBuilder, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { AuthFacadeService } from '../../../core/services/auth/auth-facade.service';

const passwordsMatchValidator: ValidatorFn = (group: AbstractControl): ValidationErrors | null => {
  const newPassword     = group.get('newPassword')?.value;
  const confirmPassword = group.get('confirmPassword')?.value;
  return newPassword && confirmPassword && newPassword !== confirmPassword
    ? { passwordsMismatch: true }
    : null;
};

@Component({
  selector: 'app-reset-password',
  standalone: false,
  templateUrl: './reset-password.component.html',
  styleUrl: './reset-password.component.scss',
})
export class ResetPasswordComponent implements OnInit, OnDestroy {
  private fb       = inject(FormBuilder);
  private route    = inject(ActivatedRoute);
  private auth     = inject(AuthFacadeService);
  private renderer = inject(Renderer2);

  // ── 1. Signals (state) ──────────────────────────────────────────────
  readonly isLoading    = signal(false);
  readonly isSubmitted  = signal(false);
  readonly hasError     = signal(false);
  readonly errorMessage = signal('');
  readonly invalidLink  = signal(false);
  readonly hideNew      = signal(true);
  readonly hideConfirm  = signal(true);

  // ── 2. Form ─────────────────────────────────────────────────────────
  form = this.fb.group(
    {
      newPassword:     ['', [Validators.required, Validators.minLength(8)]],
      confirmPassword: ['', [Validators.required]],
    },
    { validators: passwordsMatchValidator },
  );

  // ── 3. Private state ────────────────────────────────────────────────
  private token       = '';
  private destroyed$  = new Subject<void>();

  // ── 4. Lifecycle hooks ──────────────────────────────────────────────
  ngOnInit(): void {
    const savedTheme = localStorage.getItem('theme') as 'light' | 'dark' | null;
    this.renderer.setAttribute(document.documentElement, 'data-theme', savedTheme ?? 'light');

    const token = this.route.snapshot.queryParamMap.get('token');
    if (!token) {
      this.invalidLink.set(true);
      return;
    }
    this.token = token;
  }

  ngOnDestroy(): void {
    this.destroyed$.next();
    this.destroyed$.complete();
  }

  // ── 5. Public template methods ──────────────────────────────────────
  onSubmit(): void {
    if (this.form.invalid || this.isLoading()) return;

    this.isLoading.set(true);
    this.hasError.set(false);
    this.errorMessage.set('');

    const { newPassword, confirmPassword } = this.form.value;

    this.auth.resetPassword(this.token, newPassword!, confirmPassword!)
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
        next:  () => { this.isLoading.set(false); this.isSubmitted.set(true); },
        error: () => {
          this.isLoading.set(false);
          this.hasError.set(true);
          this.errorMessage.set('This reset link is invalid or has expired. Please request a new one.');
        },
      });
  }
}
