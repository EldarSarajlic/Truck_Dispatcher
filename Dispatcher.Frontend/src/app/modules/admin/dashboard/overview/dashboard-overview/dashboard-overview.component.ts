import { Component, OnInit, OnDestroy, inject, signal, computed } from '@angular/core';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

import { DashboardApiService } from '../../../../../api-services/dashboard/dashboard-api.service';
import {
  GetAdminDashboardOverviewResponse,
  RecentOrderItem,
} from '../../../../../api-services/dashboard/dashboard-api.model';
import { MetricCard } from '../../../../shared/components/metric-card/metric-card.component';

@Component({
  selector:    'app-dashboard-overview',
  standalone:  false,
  templateUrl: './dashboard-overview.component.html',
  styleUrl:    './dashboard-overview.component.scss',
})
export class DashboardOverviewComponent implements OnInit, OnDestroy {
  readonly dashboardStats = signal<GetAdminDashboardOverviewResponse | null>(null);
  readonly isLoading      = signal(true);
  readonly hasError       = signal(false);
  readonly selectedOrder  = signal<RecentOrderItem | null>(null);

  readonly metricCards = computed<MetricCard[]>(() => {
    const s = this.dashboardStats();
    return [
      {
        label:   'DASHBOARD.PENDING_ORDERS',
        value:   s?.pendingOrders ?? 0,
        variant: 'amber',
        icon:    'ph-duotone ph-file-text',
        pill:    'DASHBOARD.NEEDS_REVIEW',
      },
      {
        label:   'DASHBOARD.TOTAL_ORDERS',
        value:   s?.totalOrders ?? 0,
        variant: 'green',
        icon:    'ph-duotone ph-check-circle',
        pill:    'DASHBOARD.ALL_TIME',
      },
      {
        label:   'DASHBOARD.TOTAL_REVENUE',
        value:   s?.totalSales ?? 0,
        variant: 'blue',
        icon:    'ph-duotone ph-currency-eur',
        pill:    'DASHBOARD.LIFETIME',
        prefix:  '€',
      },
      {
        label:   'DASHBOARD.TOTAL_USERS',
        value:   s?.totalUsers ?? 0,
        variant: 'violet',
        icon:    'ph-duotone ph-users-three',
        pill:    'DASHBOARD.REGISTERED',
      },
    ];
  });

  private readonly destroyed$ = new Subject<void>();

  private readonly api = inject(DashboardApiService);

  ngOnInit(): void {
    this.loadOverview();
  }

  ngOnDestroy(): void {
    // Signal to all takeUntil() operators that they should complete.
    this.destroyed$.next();
    this.destroyed$.complete();
  }

  private loadOverview(): void {
    this.isLoading.set(true);
    this.hasError.set(false);

    this.api.getOverview()
      .pipe(takeUntil(this.destroyed$))  // auto-unsubscribes when component destroys
      .subscribe({
        next: (res) => {
          this.dashboardStats.set(res);
          this.isLoading.set(false);
        },
        error: () => {
          this.hasError.set(true);
          this.isLoading.set(false);
        },
      });
  }

  // ── Public template methods ────────────────────────────────────────────
  // Lookup table pattern: object literal instead of if/else chain.
  statusClass(status: string): string {
    return ({
      'Pending':    'status--pending',
      'Approved':   'status--approved',
      'InProgress': 'status--in-progress',
      'Completed':  'status--completed',
      'Cancelled':  'status--cancelled',
    } as Record<string, string>)[status] ?? '';
  }

  statusKey(status: string): string {
    return ({
      'Pending':    'ORDERS.STATUS.PENDING',
      'Approved':   'ORDERS.STATUS.APPROVED',
      'InProgress': 'ORDERS.STATUS.IN_PROGRESS',
      'Completed':  'ORDERS.STATUS.COMPLETED',
      'Cancelled':  'ORDERS.STATUS.CANCELLED',
    } as Record<string, string>)[status] ?? 'ORDERS.STATUS.UNKNOWN';
  }

  openOrderModal(order: RecentOrderItem): void {
    this.selectedOrder.set(order);
  }

  closeOrderModal(): void {
    this.selectedOrder.set(null);
  }
}