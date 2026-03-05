import { Component, Input, Output, EventEmitter, computed, inject } from '@angular/core';
import { AuthFacadeService } from '../../../../core/services/auth/auth-facade.service';
import { UserRole } from '../../../../core/services/auth/current-user.dto';
import { Router } from '@angular/router';

// This interface describes the shape of one nav item.
export interface NavItem {
  label: string;       // Display text, e.g. "Dashboard"
  icon:  string;       // Phosphor icon class, e.g. "ph-duotone ph-layout"
  route: string;       // Angular route, e.g. "/admin/dashboard"
}

@Component({
  selector: 'app-sidebar',
  standalone: false,         
  templateUrl: './sidebar.component.html',
  styleUrl:    './sidebar.component.scss',
})
export class SidebarComponent {
  private router = inject(Router)
  @Input() navItems: NavItem[] = [];
  @Input() languages: { code: string; name: string; flag: string }[] = [];
  @Input() currentLang: string = '';
  @Output() languageChange = new EventEmitter<string>();

  auth = inject(AuthFacadeService);

  readonly roleLabel = computed(() => {
    const role = this.auth.currentUser()?.role;
    return role !== undefined ? UserRole[role] : '';
  });

  readonly profileRoute = computed(() => {
    const role = this.auth.currentUser()?.role;
    return role !== undefined ? `/${UserRole[role].toLowerCase()}/profile` : '/';
  });

  readonly settingsRoute = '/settings';

  onLogout(): void {
    this.auth.logout().subscribe(() => {
      this.router.navigate(['/auth/login']);
    });
  }
}