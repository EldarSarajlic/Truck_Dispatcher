import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { environment } from '../../../environments/environment';
import { VehicleStatusDto } from '../../core/models/vehicle-status.model';

type PagedResult<T> = {
  items: T[];
  totalCount?: number;
};

@Injectable({ providedIn: 'root' })
export class VehicleStatusService {
  private readonly baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getAll(): Observable<VehicleStatusDto[]> {
    const params = new HttpParams()
      .set('Paging.Page', '1')
      .set('Paging.PageSize', '100');

    return this.http
      .get<PagedResult<VehicleStatusDto>>(`${this.baseUrl}/VehicleStatuses`, { params })
      .pipe(map((r) => r.items ?? []));
  }
}