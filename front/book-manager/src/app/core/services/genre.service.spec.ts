import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';

import { GenreService } from './genre.service';
import { Genre, CreateGenre, UpdateGenre } from '../models/genre.model';
import { ApiResponse } from '../models/api-response.model';

describe('GenreService', () => {
  let service: GenreService;
  let httpMock: HttpTestingController;

  const mockGenre: Genre = { id: 1, name: 'Fiction' };
  const apiUrl = '/genres';

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [GenreService]
    });
    service = TestBed.inject(GenreService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('getAll() should return list of genres', () => {
    const response: ApiResponse<Genre[]> = { success: true, data: [mockGenre] };
    service.getAll().subscribe(genres => {
      expect(genres.length).toBe(1);
      expect(genres[0].name).toBe('Fiction');
    });
    const req = httpMock.expectOne(r => r.url.endsWith('/genres'));
    expect(req.request.method).toBe('GET');
    req.flush(response);
  });

  it('getById() should return a single genre', () => {
    const response: ApiResponse<Genre> = { success: true, data: mockGenre };
    service.getById(1).subscribe(genre => {
      expect(genre.id).toBe(1);
    });
    const req = httpMock.expectOne(r => r.url.endsWith('/genres/1'));
    expect(req.request.method).toBe('GET');
    req.flush(response);
  });

  it('create() should POST and return new genre', () => {
    const dto: CreateGenre = { name: 'Fiction' };
    const response: ApiResponse<Genre> = { success: true, data: mockGenre };
    service.create(dto).subscribe(genre => {
      expect(genre.name).toBe('Fiction');
    });
    const req = httpMock.expectOne(r => r.url.endsWith('/genres'));
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(dto);
    req.flush(response);
  });

  it('update() should PUT and return updated genre', () => {
    const dto: UpdateGenre = { name: 'Updated Fiction' };
    const updated: Genre = { id: 1, name: 'Updated Fiction' };
    const response: ApiResponse<Genre> = { success: true, data: updated };
    service.update(1, dto).subscribe(genre => {
      expect(genre.name).toBe('Updated Fiction');
    });
    const req = httpMock.expectOne(r => r.url.endsWith('/genres/1'));
    expect(req.request.method).toBe('PUT');
    req.flush(response);
  });

  it('delete() should DELETE the genre', () => {
    let completed = false;
    service.delete(1).subscribe({ complete: () => (completed = true) });
    const req = httpMock.expectOne(r => r.url.endsWith('/genres/1'));
    expect(req.request.method).toBe('DELETE');
    req.flush(null, { status: 204, statusText: 'No Content' });
    expect(completed).toBeTrue();
  });
});
