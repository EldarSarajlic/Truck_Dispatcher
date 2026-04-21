import { Component } from '@angular/core';
import { NavItem } from '../../shared/components/sidebar/sidebar.component';

@Component({
  selector: 'app-admin-layout',
  standalone: false,
  templateUrl: './admin-layout.component.html',
  styleUrl: './admin-layout.component.scss'
})
export class AdminLayoutComponent {
  readonly adminNavItems: NavItem[] = [
    { label: 'ADMIN_PANEL.MENU.DASHBOARD', icon: 'ph-duotone ph-layout',        route: '/admin/dashboard' },
    { label: 'ADMIN_PANEL.MENU.USERS',     icon: 'ph-duotone ph-users-three',   route: '/admin/users'     },
    { label: 'ADMIN_PANEL.MENU.VEHICLES',  icon: 'ph-duotone ph-truck',         route: '/admin/vehicles'  },
    { label: 'ADMIN_PANEL.MENU.TRAILERS',  icon: 'ph-duotone ph-truck-trailer', route: '/admin/trailers'  },
    { label: 'ADMIN_PANEL.MENU.INVENTORY', icon: 'ph-duotone ph-package',       route: '/admin/inventory' },
  ];
}
