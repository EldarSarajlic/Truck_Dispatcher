import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { CityDto, CountryDto } from './locations-api.model';

@Injectable({ providedIn: 'root' })
export class LocationsApiService {
  private readonly baseUrl = `${environment.apiUrl}/api/locations`;
  private readonly http    = inject(HttpClient);

  getCountries(): Observable<CountryDto[]> {
    return this.http.get<CountryDto[]>(`${this.baseUrl}/countries`);
  }

  getCitiesByCountry(countryId: number): Observable<CityDto[]> {
    return this.http.get<CityDto[]>(`${this.baseUrl}/countries/${countryId}/cities`);
  }
}
