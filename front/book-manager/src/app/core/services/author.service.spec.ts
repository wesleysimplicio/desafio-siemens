import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';

import { AuthorService } from './author.service';
import { Author, CreateAuthor, UpdateAuthor } from '../models/author.model';
import { ApiResponse } from '../models/api-response.model';

describe('AuthorService', () => {
  let service: AuthorService;
  let httpMock: HttpTestingController;

  const mockAuthor: Author = { id: 1, name: 'J.R.R. Tolkien', bio: 'English author' };

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [AuthorService]
    });
    service = TestBed.inject(AuthorService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('getAll() should return list of authors', () => {
    const response: ApiResponse<Author[]> = { success: true, data: [mockAuthor] };
    service.getAll().subscribe(authors => {
      expect(authors.length).toBe(1);
      expect(authors[0].name).toBe('J.R.R. Tolkien');
    });
    const req = httpMock.expectOne(r => r.url.endsWith('/authors'));
    expect(req.request.method).toBe('GET');
    req.flush(response);
  });

  it('getById() should return a single author', () => {
    const response: ApiResponse<Author> = { success: true, data: mockAuthor };
    service.getById(1).subscribe(author => {
      expect(author.id).toBe(1);
    });
    const req = httpMock.expectOne(r => r.url.endsWith('/authors/1'));
    expect(req.request.method).toBe('GET');
    req.flush(response);
  });

  it('create() should POST and return new author', () => {
    const dto: CreateAuthor = { name: 'J.R.R. Tolkien', bio: 'English author' };
    const response: ApiResponse<Author> = { success: true, data: mockAuthor };
    service.create(dto).subscribe(author => {
      expect(author.name).toBe('J.R.R. Tolkien');
    });
    const req = httpMock.expectOne(r => r.url.endsWith('/authors'));
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(dto);
    req.flush(response);
  });

  it('update() should PUT and return updated author', () => {
    const dto: UpdateAuthor = { name: 'Updated Name', bio: 'Updated bio' };
    const updated: Author = { id: 1, name: 'Updated Name', bio: 'Updated bio' };
    const response: ApiResponse<Author> = { success: true, data: updated };
    service.update(1, dto).subscribe(author => {
      expect(author.name).toBe('Updated Name');
    });
    const req = httpMock.expectOne(r => r.url.endsWith('/authors/1'));
    expect(req.request.method).toBe('PUT');
    req.flush(response);
  });

  it('delete() should DELETE the author', () => {
    const response: ApiResponse<void> = { success: true, data: undefined };
    service.delete(1).subscribe(result => {
      expect(result).toBeUndefined();
    });
    const req = httpMock.expectOne(r => r.url.endsWith('/authors/1'));
    expect(req.request.method).toBe('DELETE');
    req.flush(response);
  });
});
