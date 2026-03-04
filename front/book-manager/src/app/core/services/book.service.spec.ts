import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';

import { BookService } from './book.service';
import { Book, CreateBook, UpdateBook } from '../models/book.model';
import { ApiResponse } from '../models/api-response.model';

describe('BookService', () => {
  let service: BookService;
  let httpMock: HttpTestingController;

  const mockBook: Book = {
    id: 1,
    title: 'The Lord of the Rings',
    isbn: '978-0-618-64015-7',
    publishedYear: 1954,
    authorId: 1,
    authorName: 'J.R.R. Tolkien',
    genreId: 1,
    genreName: 'Fantasy'
  };

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [BookService]
    });
    service = TestBed.inject(BookService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('getAll() should return list of books', () => {
    const response: ApiResponse<Book[]> = { success: true, data: [mockBook] };
    service.getAll().subscribe(books => {
      expect(books.length).toBe(1);
      expect(books[0].title).toBe('The Lord of the Rings');
    });
    const req = httpMock.expectOne(r => r.url.endsWith('/books'));
    expect(req.request.method).toBe('GET');
    req.flush(response);
  });

  it('getById() should return a single book', () => {
    const response: ApiResponse<Book> = { success: true, data: mockBook };
    service.getById(1).subscribe(book => {
      expect(book.id).toBe(1);
    });
    const req = httpMock.expectOne(r => r.url.endsWith('/books/1'));
    expect(req.request.method).toBe('GET');
    req.flush(response);
  });

  it('create() should POST and return new book', () => {
    const dto: CreateBook = {
      title: 'The Lord of the Rings',
      isbn: '978-0-618-64015-7',
      publishedYear: 1954,
      authorId: 1,
      genreId: 1
    };
    const response: ApiResponse<Book> = { success: true, data: mockBook };
    service.create(dto).subscribe(book => {
      expect(book.title).toBe('The Lord of the Rings');
    });
    const req = httpMock.expectOne(r => r.url.endsWith('/books'));
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(dto);
    req.flush(response);
  });

  it('update() should PUT and return updated book', () => {
    const dto: UpdateBook = { title: 'Updated Title', isbn: '978-0-618-64015-7', publishedYear: 1955, authorId: 1, genreId: 1 };
    const updated: Book = { ...mockBook, title: 'Updated Title' };
    const response: ApiResponse<Book> = { success: true, data: updated };
    service.update(1, dto).subscribe(book => {
      expect(book.title).toBe('Updated Title');
    });
    const req = httpMock.expectOne(r => r.url.endsWith('/books/1'));
    expect(req.request.method).toBe('PUT');
    req.flush(response);
  });

  it('delete() should DELETE the book', () => {
    const response: ApiResponse<void> = { success: true, data: undefined };
    service.delete(1).subscribe(result => {
      expect(result).toBeUndefined();
    });
    const req = httpMock.expectOne(r => r.url.endsWith('/books/1'));
    expect(req.request.method).toBe('DELETE');
    req.flush(response);
  });
});
