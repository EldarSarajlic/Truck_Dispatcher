import { Component, OnInit } from '@angular/core';

interface DashboardStats {
  totalUsers: number;
  usersChange: string;

  activeOrders: number;
  ordersChange: number;
  ordersChangeText: string;

  totalVehicles: number;
  readyVehicles: number;
  maintenanceVehicles: number;

  revenue: number;
  revenueGrowth: number;

  alerts: string[];
}

@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.scss'],
  standalone: false
})
export class AdminDashboardComponent implements OnInit {

  isLoading = true;
  error?: string;

  dashboardStats?: DashboardStats;

  ngOnInit(): void {
    // mock dok nema API
    setTimeout(() => {
      this.dashboardStats = {
        totalUsers: 156,
        usersChange: '+12 this month',

        activeOrders: 89,
        ordersChange: -5,
        ordersChangeText: '-5 today',

        totalVehicles: 45,
        readyVehicles: 42,
        maintenanceVehicles: 3,

        revenue: 458320,
        revenueGrowth: 15.3,

        alerts: [
          '3 vehicles need service',
          '2 insurance policies expiring',
          '5 pending order approvals'
        ]
      };

      this.isLoading = false;
    }, 600);
  }
}
