import { Component } from '@angular/core';

@Component({
  selector: 'app-order-stats',
  standalone: false,
  templateUrl: './order-stats.component.html',
  styleUrl: './order-stats.component.css',
})
export class OrderStatsComponent {

  currentMonth = {
    totalRevenue: 0,
    revenue: 0,
    totalOrders: 0,
    avgOrderValue: 0
  };

  selectedStats = {
    approved: 0,
    pending: 0,
    rejected: 0
  };

  months = [
    { value: 1, label: 'January' },
    { value: 2, label: 'February' },
    // ...
  ];

  years = [2023, 2024, 2025];

  selectedMonth = new Date().getMonth() + 1;
  selectedYear = new Date().getFullYear();

  loadStats() {
    // UI only for now
  }

  // ✅ OVO JE FALILO
  get selectedMonthLabel(): string {
    const month = this.months.find(m => m.value === this.selectedMonth);
    return month ? month.label : '';
  }
}
