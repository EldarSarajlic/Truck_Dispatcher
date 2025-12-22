import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  GetAdminDashboardOverviewResponse
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
}
