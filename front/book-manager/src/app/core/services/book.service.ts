import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { Book, CreateBook, UpdateBook } from '../models/book.model';
import { ApiResponse } from '../models/api-response.model';

@Injectable({ providedIn: 'root' })
export class BookService {
  private readonly endpoint = '/books';

  constructor(private http: HttpClient) {}

  getAll(): Observable<Book[]> {
    return this.http
      .get<ApiResponse<Book[]>>(this.endpoint)
      .pipe(map(res => res.data ?? []));
  }

  getById(id: number): Observable<Book> {
    return this.http
      .get<ApiResponse<Book>>(`${this.endpoint}/${id}`)
      .pipe(map(res => res.data!));
  }

  create(payload: CreateBook): Observable<Book> {
    return this.http
      .post<ApiResponse<Book>>(this.endpoint, payload)
      .pipe(map(res => res.data!));
  }

  update(id: number, payload: UpdateBook): Observable<Book> {
    return this.http
      .put<ApiResponse<Book>>(`${this.endpoint}/${id}`, payload)
      .pipe(map(res => res.data!));
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.endpoint}/${id}`);
  }
}
