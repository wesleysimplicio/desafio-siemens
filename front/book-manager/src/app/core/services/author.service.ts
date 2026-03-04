import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { Author, CreateAuthor, UpdateAuthor } from '../models/author.model';
import { ApiResponse } from '../models/api-response.model';

@Injectable({ providedIn: 'root' })
export class AuthorService {
  private readonly endpoint = '/authors';

  constructor(private http: HttpClient) {}

  getAll(): Observable<Author[]> {
    return this.http
      .get<ApiResponse<Author[]>>(this.endpoint)
      .pipe(map(res => res.data ?? []));
  }

  getById(id: number): Observable<Author> {
    return this.http
      .get<ApiResponse<Author>>(`${this.endpoint}/${id}`)
      .pipe(map(res => res.data!));
  }

  create(payload: CreateAuthor): Observable<Author> {
    return this.http
      .post<ApiResponse<Author>>(this.endpoint, payload)
      .pipe(map(res => res.data!));
  }

  update(id: number, payload: UpdateAuthor): Observable<Author> {
    return this.http
      .put<ApiResponse<Author>>(`${this.endpoint}/${id}`, payload)
      .pipe(map(res => res.data!));
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.endpoint}/${id}`);
  }
}
