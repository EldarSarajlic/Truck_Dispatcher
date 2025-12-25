import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { CreateTruckRequest, TruckDto, UpdateTruckRequest } from '../../core/models/truck.model';

export type TruckQuery = {
  search?: string;
  status?: number | null; // VehicleStatusId (for T-2.3.5)
};

@Injectable({ providedIn: 'root' })
export class TruckService {
  private readonly baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  // GET /Trucks?Search=...&Status=...
  getAll(query?: TruckQuery): Observable<TruckDto[]> {
    let params = new HttpParams();

    if (query?.search) params = params.set('Search', query.search);
    if (query?.status !== null && query?.status !== undefined) {
      params = params.set('Status', String(query.status));
    }

    return this.http.get<TruckDto[]>(`${this.baseUrl}/Trucks`, { params });
  }

  // GET /Trucks/{id}
  getById(id: number): Observable<TruckDto> {
    return this.http.get<TruckDto>(`${this.baseUrl}/Trucks/${id}`);
  }

  // POST /Trucks
  create(payload: CreateTruckRequest): Observable<TruckDto> {
    return this.http.post<TruckDto>(`${this.baseUrl}/Trucks`, payload);
  }

  // PUT /Trucks/{id}
  update(id: number, payload: UpdateTruckRequest): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/Trucks/${id}`, payload);
  }

  // DELETE /Trucks/{id}
  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/Trucks/${id}`);
  }

  // POST /Trucks/{id}/enable
  enable(id: number): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/Trucks/${id}/enable`, {});
  }

  // POST /Trucks/{id}/disable
  disable(id: number): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/Trucks/${id}/disable`, {});
  }
}