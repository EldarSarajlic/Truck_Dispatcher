import { Injectable }                          from '@angular/core';
import { HttpClient, HttpParams }              from '@angular/common/http';
import { Observable }                          from 'rxjs';
import { environment }                         from '../../../environments/environment';
import { InventoryItemDto, ListInventoryRequest, ListInventoryResponse } from './inventory-api.model';

@Injectable({ providedIn: 'root' })
export class InventoryApiService {
  private readonly base = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getItems(req: ListInventoryRequest): Observable<ListInventoryResponse> {
    let params = new HttpParams()
      .set('Paging.Page',     String(req.paging?.page     ?? 1))
      .set('Paging.PageSize', String(req.paging?.pageSize ?? 10));

    if (req.search)   params = params.set('Search',   req.search);
    if (req.category) params = params.set('Category', req.category);

    return this.http.get<ListInventoryResponse>(`${this.base}/Inventory`, { params });
  }
}
