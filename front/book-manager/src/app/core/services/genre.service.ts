import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { Genre, CreateGenre, UpdateGenre } from '../models/genre.model';
import { ApiResponse } from '../models/api-response.model';

@Injectable({ providedIn: 'root' })
export class GenreService {
  private readonly endpoint = '/genres';

  constructor(private http: HttpClient) {}

  getAll(): Observable<Genre[]> {
    return this.http
      .get<ApiResponse<Genre[]>>(this.endpoint)
      .pipe(map(res => res.data ?? []));
  }

  getById(id: number): Observable<Genre> {
    return this.http
      .get<ApiResponse<Genre>>(`${this.endpoint}/${id}`)
      .pipe(map(res => res.data!));
  }

  create(payload: CreateGenre): Observable<Genre> {
    return this.http
      .post<ApiResponse<Genre>>(this.endpoint, payload)
      .pipe(map(res => res.data!));
  }

  update(id: number, payload: UpdateGenre): Observable<Genre> {
    return this.http
      .put<ApiResponse<Genre>>(`${this.endpoint}/${id}`, payload)
      .pipe(map(res => res.data!));
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.endpoint}/${id}`);
  }
}
