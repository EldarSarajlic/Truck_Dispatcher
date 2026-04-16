import { Component, inject, OnDestroy, OnInit, Renderer2, signal } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { AuthFacadeService } from '../../../core/services/auth/auth-facade.service';

@Component({
  selector: 'app-forgot-password',
  standalone: false,
  templateUrl: './forgot-password.component.html',
  styleUrl: './forgot-password.component.scss',
})
export class ForgotPasswordComponent implements OnInit, OnDestroy {
  private fb       = inject(FormBuilder);
  private renderer = inject(Renderer2);

  // ── 1. Signals (state) ──────────────────────────────────────────────
  readonly isLoading   = signal(false);
  readonly hasError    = signal(false);
  readonly isSubmitted = signal(false);
  readonly errorMessage = signal('');

  // ── 2. Form ─────────────────────────────────────────────────────────
  form = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
  });

  // ── 3. Private cleanup handles ──────────────────────────────────────
  private destroyed$ = new Subject<void>();

  private auth = inject(AuthFacadeService);

  // ── 4. Lifecycle hooks ──────────────────────────────────────────────
  ngOnInit(): void {
    const savedTheme = localStorage.getItem('theme') as 'light' | 'dark' | null;
    this.renderer.setAttribute(document.documentElement, 'data-theme', savedTheme ?? 'light');
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

    this.auth.forgotPassword(this.form.value.email!)
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
        next:  () => { this.isLoading.set(false); this.isSubmitted.set(true); },
        error: () => {
          this.isLoading.set(false);
          this.hasError.set(true);
          this.errorMessage.set('Something went wrong. Please try again.');
        },
      });
  }
}
