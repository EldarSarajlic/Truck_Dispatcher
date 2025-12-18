import { Component, inject, OnInit, Renderer2 } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { BaseComponent } from '../../../core/components/base-classes/base-component';
import { AuthFacadeService } from '../../../core/services/auth/auth-facade.service';
import { LoginCommand } from '../../../api-services/auth/auth-api.model';
import { CurrentUserService } from '../../../core/services/auth/current-user.service';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent extends BaseComponent implements OnInit {
  private fb = inject(FormBuilder);
  private auth = inject(AuthFacadeService);
  private router = inject(Router);
  private currentUser = inject(CurrentUserService);
  private renderer = inject(Renderer2);

  hidePassword = true;
  emailFocused = false;
  passwordFocused = false;
  currentTheme: 'light' | 'dark' = 'light';

  form = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required]],
    rememberMe: [false],
  });

  ngOnInit(): void {
    // Load saved theme from localStorage or default to 'light'
    const savedTheme = localStorage.getItem('theme') as 'light' | 'dark' | null;
    this.currentTheme = savedTheme || 'light';
    this.applyTheme(this.currentTheme);
  }

  onSubmit(): void {
    if (this.form.invalid || this.isLoading) return;

    this.startLoading();

    const payload: LoginCommand = {
      email: this.form.value.email ?? '',
      password: this.form.value.password ?? '',
      fingerprint: null,
    };

    this.auth.login(payload).subscribe({
      next: () => {
        this.stopLoading();
        const target = this.currentUser.getDefaultRoute();
        this.router.navigate([target]);
      },
      error: (err) => {
        this.stopLoading('Invalid credentials. Please try again.');
        console.error('Login error:', err);
      },
    });
  }

  togglePasswordVisibility(): void {
    this.hidePassword = !this.hidePassword;
  }

  private applyTheme(theme: 'light' | 'dark'): void {
    const htmlElement = document.documentElement;
    this.renderer.setAttribute(htmlElement, 'data-theme', theme);
  }
}
