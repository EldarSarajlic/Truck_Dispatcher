import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import {
  ListTrailersRequest,
  ListTrailersResponse,
  ListTrailerQueryDto,
  GetTrailerByIdQueryDto,
  CreateTrailerCommand,
  UpdateTrailerCommand,
  ChangeTrailerStatusCommand
} from './trailers-api.model';
import { buildHttpParams } from '../../../core/models/build-http-params';

@Injectable({
  providedIn: 'root'
})
export class TrailersService {
  private readonly baseUrl = `${environment.apiUrl}/Trailers`;
  private http = inject(HttpClient);

  /**
   * GET /Trailers
   * List trailers with paging, search and status filter
   */
  list(request?: ListTrailersRequest): Observable<ListTrailersResponse> {
    const params = request ? buildHttpParams(request as any) : undefined;
    return this.http.get<ListTrailersResponse>(this.baseUrl, { params });
  }

  /**
   * GET /Trailers/{id}
   * Get trailer by ID
   */
  getById(id: number): Observable<GetTrailerByIdQueryDto> {
    return this.http.get<GetTrailerByIdQueryDto>(`${this.baseUrl}/${id}`);
  }

  /**
   * POST /Trailers
   * Create a new trailer
   * @returns ID of created trailer
   */
  create(payload: CreateTrailerCommand): Observable<number> {
    return this.http.post<number>(this.baseUrl, payload);
  }

  /**
   * PUT /Trailers/{id}
   * Update existing trailer
   */
  update(id: number, payload: UpdateTrailerCommand): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, payload);
  }

  /**
   * PATCH /Trailers/{id}/status
   * Change trailer status
   */
  changeStatus(
    id: number,
    payload: ChangeTrailerStatusCommand
  ): Observable<void> {
    return this.http.patch<void>(`${this.baseUrl}/${id}/status`, payload);
  }

  /**
   * DELETE /Trailers/{id}
   * Delete trailer
   */
  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
