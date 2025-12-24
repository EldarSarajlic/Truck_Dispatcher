import { Component, OnInit, inject } from '@angular/core';
import { DashboardApiService } from '../../../../api-services/dashboard/dashboard-api.service';
import {
  GetOrdersDashboardSummaryResponse
} from '../../../../api-services/dashboard/dashboard-api.model';
import { Chart, registerables } from 'chart.js';
import {
  GetOrdersDashboardChartsResponse
} from '../../../../api-services/dashboard/dashboard-api.model';
import {
  GetOrdersReportResponse
} from '../../../../api-services/dashboard/dashboard-api.model';

Chart.register(...registerables);


@Component({
  selector: 'app-order-stats',
  standalone: false,
  templateUrl: './order-stats.component.html',
  styleUrl: './order-stats.component.css',
})
export class OrderStatsComponent implements OnInit {

  private dashboardApi = inject(DashboardApiService);

  // ================= LOADING =================
  isLoadingSummary = false;

  // ================= CURRENT MONTH SUMMARY =================
  currentMonth = {
    totalRevenue: 0,   // year-to-date revenue
    revenue: 0,        // current month revenue
    totalOrders: 0,
    avgOrderValue: 0
  };

  // ================= DUMMY DATA (OSTAJE ZA SAD) =================
  selectedStats = {
    approved: 0,
    pending: 0,
    rejected: 0
  };

  months = [
    { value: 1, label: 'January' },
    { value: 2, label: 'February' },
    { value: 3, label: 'March' },
    { value: 4, label: 'April' },
    { value: 5, label: 'May' },
    { value: 6, label: 'June' },
    { value: 7, label: 'July' },
    { value: 8, label: 'August' },
    { value: 9, label: 'September' },
    { value: 10, label: 'October' },
    { value: 11, label: 'November' },
    { value: 12, label: 'December' }
  ];

  years = [2023, 2024, 2025];

  selectedMonth = new Date().getMonth() + 1;
  selectedYear = new Date().getFullYear();

  // ============================================================

  ngOnInit(): void {
    this.loadCurrentMonthSummary();
    this.loadCharts(this.selectedYear);
    this.loadOrdersReport();
  }

  /**
   * Loads current month KPI summary
   * Endpoint: GET /dashboard/orders/summary
   */
  loadCurrentMonthSummary(): void {
    this.isLoadingSummary = true;

    this.dashboardApi.getOrdersSummary().subscribe({
      next: (result) => {
        this.currentMonth = {
          totalRevenue: result.totalRevenue,
          revenue: result.monthlyRevenue,
          totalOrders: result.totalOrders,
          avgOrderValue: result.avgOrderValue
        };

        this.isLoadingSummary = false;
      },
      error: (err) => {
        console.error('Failed to load order summary', err);
        this.isLoadingSummary = false;
      }
    });
  }

ordersChart?: Chart;
revenueChart?: Chart;
isLoadingCharts = false;

loadCharts(year: number): void {
  this.isLoadingCharts = true;

  this.dashboardApi.getOrdersCharts(year).subscribe({
    next: (res) => {
      this.renderOrdersChart(res.ordersByMonth);
      this.renderRevenueChart(res.revenueByMonth);
      this.isLoadingCharts = false;
    },
    error: (err) => {
      console.error('Failed to load charts', err);
      this.isLoadingCharts = false;
    }
  });
}
private renderOrdersChart(data: number[]): void {
  if (this.ordersChart) {
    this.ordersChart.destroy();
  }

  this.ordersChart = new Chart('ordersByMonthChart', {
    type: 'bar',
    data: {
      labels: this.months.map(m => m.label),
      datasets: [{
        label: 'Orders',
        data
      }]
    }
  });
}

private renderRevenueChart(data: number[]): void {
  if (this.revenueChart) {
    this.revenueChart.destroy();
  }

  this.revenueChart = new Chart('revenueByMonthChart', {
    type: 'line',
    data: {
      labels: this.months.map(m => m.label),
      datasets: [{
        label: 'Revenue',
        data
      }]
    }
  });
}

// ================= MONTHLY / YEARLY REPORT =================
isLoadingReport = false;
ordersReport?: GetOrdersReportResponse;
reportError?: string;
activeItemsTab: 'selling' | 'profitable' | 'cancelled' = 'selling';

loadOrdersReport(): void {
  this.isLoadingReport = true;
  this.reportError = undefined;

this.dashboardApi.getOrdersReport(
  this.selectedYear,
  this.selectedMonth
).subscribe({
  next: (res) => {
    this.ordersReport = res;
    this.isLoadingReport = false;
  },
  error: (err) => {
    console.error('Failed to load orders report', err);
    this.isLoadingReport = false;
  }
});
}

  /**
   * Placeholder for future month/year filtering
   */
  loadStats(): void {
    // intentionally empty for now
    this.loadOrdersReport();
    this.loadCharts(this.selectedYear);
  }

  // ================= HELPERS =================
  get selectedMonthLabel(): string {
    const month = this.months.find(m => m.value === this.selectedMonth);
    return month ? month.label : '';
  }
}
