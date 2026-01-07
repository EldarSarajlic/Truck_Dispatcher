import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { ListUsersRequest, ListUsersResponse } from './users-api.model';
import { Observable } from 'rxjs';
import { buildHttpParams } from '../../core/models/build-http-params';

@Injectable({
  providedIn: 'root',
})
export class UsersApiService {
  private http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/users`;
  
  getUsers(request?: ListUsersRequest): Observable<ListUsersResponse>{
    const params = request ? buildHttpParams(request as any) : undefined;
    return this.http.get<ListUsersResponse>(`${this.baseUrl}`, {params});
  }
}
