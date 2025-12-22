import { Component, OnInit, inject } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';

import { DashboardApiService } from '../../../../../api-services/dashboard/dashboard-api.service';
import {
  GetAdminDashboardOverviewResponse
} from '../../../../../api-services/dashboard/dashboard-api.model';

@Component({
  selector: 'app-dashboard-overview',
  standalone: false,
  templateUrl: './dashboard-overview.component.html',
})
export class DashboardOverviewComponent implements OnInit {

  private api = inject(DashboardApiService);

  isLoading = false;
  error?: string;

  dashboardStats?: GetAdminDashboardOverviewResponse;

  // ✅ MODAL STATE
  selectedOrder: GetAdminDashboardOverviewResponse['recentOrders'][number] | null = null;

  ngOnInit(): void {
    this.loadOverview();
  }

  private loadOverview(): void {
    this.isLoading = true;
    this.error = undefined;

    this.api.getOverview().subscribe({
      next: (response) => {
        this.dashboardStats = response;
        this.isLoading = false;
      },
      error: (err: unknown) => {
        if (err instanceof HttpErrorResponse) {
          this.error = err.error?.message ?? err.message;
        } else if (err instanceof Error) {
          this.error = err.message;
        } else {
          this.error = 'Failed to load dashboard overview';
        }

        this.isLoading = false;
        console.error('Dashboard overview error:', err);
      }
    });
  }

  // ✅ MODAL METHODS
  openOrderModal(order: GetAdminDashboardOverviewResponse['recentOrders'][number]): void {
    this.selectedOrder = order;
  }

  closeOrderModal(): void {
    this.selectedOrder = null;
  }
}
