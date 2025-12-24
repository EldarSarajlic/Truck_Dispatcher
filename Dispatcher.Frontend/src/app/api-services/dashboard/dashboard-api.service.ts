import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  GetAdminDashboardOverviewResponse,
  GetOrdersDashboardChartsResponse,
  GetOrdersDashboardSummaryResponse,
  GetOrdersReportResponse
} from './dashboard-api.model';

@Injectable({
  providedIn: 'root'
})
export class DashboardApiService {
  private readonly baseUrl = `${environment.apiUrl}/dashboard`;
  private http = inject(HttpClient);

  /**
   * GET /Dashboard/overview
   * Get admin dashboard overview.
   */
  getOverview(): Observable<GetAdminDashboardOverviewResponse> {
    return this.http.get<GetAdminDashboardOverviewResponse>(
      `${this.baseUrl}/overview`
    );
  }
  /**
   * GET /dashboard/orders/summary
   * Get orders dashboard current month summary.
   */
  getOrdersSummary(): Observable<GetOrdersDashboardSummaryResponse> {
    return this.http.get<GetOrdersDashboardSummaryResponse>(
      `${this.baseUrl}/orders/summary`
    );
  }

getOrdersCharts(year: number) {
  return this.http.get<GetOrdersDashboardChartsResponse>(
    `${this.baseUrl}/orders/charts`,
    { params: { year } }
  );
}
getOrdersReport(year: number, month?: number) {
  const params: any = { year };

  if (month) {
    params.month = month;
  }

  return this.http.get<GetOrdersReportResponse>(
    `${this.baseUrl}/orders/report`,
    { params }
  );
}

}
