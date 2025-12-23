import { Component } from '@angular/core';

type VehiclesTab = 'trucks' | 'trailers' | 'status';

@Component({
  selector: 'app-admin-vehicles',
  standalone: false,
  templateUrl: './admin-vehicles.component.html',
  styleUrls: ['./admin-vehicles.component.scss'],
})
export class AdminVehiclesComponent {
  activeTab: VehiclesTab = 'trucks';

  setActive(tab: VehiclesTab) {
    this.activeTab = tab;
  }

  showComingSoon(featureName: string) {
    window.alert(`${featureName} — coming soon`);
  }
}