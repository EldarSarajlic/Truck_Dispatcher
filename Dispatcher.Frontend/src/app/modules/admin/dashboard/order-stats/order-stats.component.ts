import { Component, OnInit, inject, HostListener } from '@angular/core';
import { DashboardApiService } from '../../../../api-services/dashboard/dashboard-api.service';
import { MetricCard } from '../../../shared/components/metric-card/metric-card.component';
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
  styleUrl: './order-stats.component.scss',
})
export class OrderStatsComponent implements OnInit {

  private dashboardApi = inject(DashboardApiService);

  // ================= LOADING =================
  isLoadingSummary = false;
  summaryError?: string;
  chartsError?: string;

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

  years = [2023, 2024, 2025,2026];

  selectedMonth: number | null = new Date().getMonth() + 1;
  selectedYear = new Date().getFullYear();

  // ================= DROPDOWN STATE =================
  monthDropdownOpen      = false;
  yearDropdownOpen       = false;
  chartsYearDropdownOpen = false;

  @HostListener('document:click')
  closeDropdowns(): void {
    this.monthDropdownOpen      = false;
    this.yearDropdownOpen       = false;
    this.chartsYearDropdownOpen = false;
  }

  toggleMonthDropdown(e: Event): void {
    e.stopPropagation();
    this.monthDropdownOpen = !this.monthDropdownOpen;
    this.yearDropdownOpen  = false;
  }

  toggleYearDropdown(e: Event): void {
    e.stopPropagation();
    this.yearDropdownOpen       = !this.yearDropdownOpen;
    this.monthDropdownOpen      = false;
    this.chartsYearDropdownOpen = false;
  }

  toggleChartsYearDropdown(e: Event): void {
    e.stopPropagation();
    this.chartsYearDropdownOpen = !this.chartsYearDropdownOpen;
    this.monthDropdownOpen      = false;
    this.yearDropdownOpen       = false;
  }

  selectChartsYear(value: number, e: Event): void {
    e.stopPropagation();
    this.selectedYear           = value;
    this.chartsYearDropdownOpen = false;
    this.loadCharts(value);
  }

  selectMonth(value: number | null, e: Event): void {
    e.stopPropagation();
    this.selectedMonth     = value;
    this.monthDropdownOpen = false;
  }

  selectYear(value: number, e: Event): void {
    e.stopPropagation();
    this.selectedYear     = value;
    this.yearDropdownOpen = false;
  }

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
    this.summaryError = undefined;

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
        this.summaryError = 'Failed to load summary data.';
        this.isLoadingSummary = false;
      }
    });
  }

ordersChart?: Chart;
revenueChart?: Chart;
isLoadingCharts = false;

loadCharts(year: number): void {
  this.isLoadingCharts = true;
  this.chartsError = undefined;

  this.dashboardApi.getOrdersCharts(year).subscribe({
    next: (res) => {
      this.isLoadingCharts = false;
      setTimeout(() => {
        this.renderOrdersChart(res.ordersByMonth);
        this.renderRevenueChart(res.revenueByMonth);
      }, 0);
    },
    error: (err) => {
      console.error('Failed to load charts', err);
      this.chartsError = 'Failed to load chart data.';
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
  this.ordersReport = undefined;

this.dashboardApi.getOrdersReport(
  this.selectedYear,
  this.selectedMonth ?? undefined
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

  get metricCards(): MetricCard[] {
    const fmt = (n: number) => n.toLocaleString('de-DE', { maximumFractionDigits: 0 });
    return [
      {
        label:   'DASHBOARD.TOTAL_REVENUE',
        value:   fmt(this.currentMonth.totalRevenue),
        variant: 'green',
        icon:    'ph-duotone ph-currency-eur',
        pill:    'DASHBOARD.YEAR_TO_DATE',
        prefix:  '€',
      },
      {
        label:   'DASHBOARD.MONTHLY_REVENUE',
        value:   fmt(this.currentMonth.revenue),
        variant: 'blue',
        icon:    'ph-duotone ph-trend-up',
        pill:    'DASHBOARD.CURRENT_MONTH',
        prefix:  '€',
      },
      {
        label:   'DASHBOARD.TOTAL_ORDERS',
        value:   this.currentMonth.totalOrders,
        variant: 'violet',
        icon:    'ph-duotone ph-clipboard-text',
        pill:    'DASHBOARD.ALL_TIME',
      },
      {
        label:   'DASHBOARD.AVG_ORDER_VALUE',
        value:   fmt(this.currentMonth.avgOrderValue),
        variant: 'amber',
        icon:    'ph-duotone ph-receipt',
        pill:    'DASHBOARD.PER_ORDER',
        prefix:  '€',
      },
    ];
  }
}
