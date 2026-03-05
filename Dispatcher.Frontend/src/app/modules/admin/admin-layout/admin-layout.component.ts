import { Component, inject, Renderer2 } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { AuthFacadeService } from '../../../core/services/auth/auth-facade.service';
import { NavItem } from '../../shared/components/sidebar/sidebar.component';

@Component({
  selector: 'app-admin-layout',
  standalone: false,
  templateUrl: './admin-layout.component.html',
  styleUrl: './admin-layout.component.scss'
})
export class AdminLayoutComponent {
  private translate = inject(TranslateService);
  private renderer = inject(Renderer2);
  auth = inject(AuthFacadeService);

  currentLang: string;

  languages = [
    { code: 'bs', name: 'Bosanski', flag: '🇧🇦' },
    { code: 'en', name: 'English', flag: '🇬🇧' }
  ];
  readonly adminNavItems: NavItem[] = [
    { label: 'ADMIN_PANEL.MENU.DASHBOARD', icon: 'ph-duotone ph-layout',        route: '/admin/dashboard' },
    { label: 'ADMIN_PANEL.MENU.USERS',     icon: 'ph-duotone ph-users-three',   route: '/admin/users'     },
    { label: 'ADMIN_PANEL.MENU.VEHICLES',  icon: 'ph-duotone ph-truck',         route: '/admin/vehicles'  },
    { label: 'ADMIN_PANEL.MENU.TRAILERS',  icon: 'ph-duotone ph-truck-trailer', route: '/admin/trailers'  },
    { label: 'ADMIN_PANEL.MENU.INVENTORY', icon: 'ph-duotone ph-package',       route: '/admin/inventory' },
  ];

  constructor() {
    this.currentLang = this.translate.currentLang || 'bs';
  }

  switchLanguage(langCode: string): void {
    this.currentLang = langCode;
    this.translate.use(langCode).subscribe();
    localStorage.setItem('language', langCode);
  }

  getCurrentLanguage() {
    return this.languages.find(lang => lang.code === this.currentLang);
  }
}
