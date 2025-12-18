import { Component, OnInit } from '@angular/core';

interface RecentOrder {
  reference: string;
  client: string;
  price: number;
  date: string;
}

interface DashboardStats {
  totalSales: number;
  totalOrders: number;
  totalUsers: number;
  pendingOrders: number;
  recentSales: RecentOrder[];
}

@Component({
  selector: 'app-dashboard-overview',
  standalone: false,
  templateUrl: './dashboard-overview.component.html',
})
export class DashboardOverviewComponent implements OnInit {

  isLoading = true;
  error?: string;
  dashboardStats?: DashboardStats;

  ngOnInit(): void {
    // MOCK DATA
    setTimeout(() => {
      this.dashboardStats = {
        totalSales: 1843200,
        totalOrders: 428,
        totalUsers: 156,
        pendingOrders: 12,
        recentSales: [
          { reference: 'ORD-2024-0912', client: 'LogiTrans d.o.o.', price: 2450, date: '2024-12-14' },
          { reference: 'ORD-2024-0911', client: 'Euro Freight GmbH', price: 3890, date: '2024-12-13' },
          { reference: 'ORD-2024-0910', client: 'Balkan Express', price: 1720, date: '2024-12-12' },
          { reference: 'ORD-2024-0909', client: 'NordCargo AS', price: 4100, date: '2024-12-11' },
        ],
      };

      this.isLoading = false;
    }, 600);
  }
}
