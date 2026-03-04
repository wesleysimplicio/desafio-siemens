import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { map, mergeMap, catchError } from 'rxjs/operators';
import { BookService } from '../../../core/services/book.service';
import * as BookActions from './book.actions';

@Injectable()
export class BookEffects {
  loadBooks$ = createEffect(() =>
    this.actions$.pipe(
      ofType(BookActions.loadBooks),
      mergeMap(() =>
        this.bookService.getAll().pipe(
          map(books => BookActions.loadBooksSuccess({ books })),
          catchError(error => of(BookActions.loadBooksFailure({ error: error.message })))
        )
      )
    )
  );

  createBook$ = createEffect(() =>
    this.actions$.pipe(
      ofType(BookActions.createBook),
      mergeMap(({ payload }) =>
        this.bookService.create(payload).pipe(
          map(book => BookActions.createBookSuccess({ book })),
          catchError(error => of(BookActions.createBookFailure({ error: error.message })))
        )
      )
    )
  );

  updateBook$ = createEffect(() =>
    this.actions$.pipe(
      ofType(BookActions.updateBook),
      mergeMap(({ id, payload }) =>
        this.bookService.update(id, payload).pipe(
          map(book => BookActions.updateBookSuccess({ book })),
          catchError(error => of(BookActions.updateBookFailure({ error: error.message })))
        )
      )
    )
  );

  deleteBook$ = createEffect(() =>
    this.actions$.pipe(
      ofType(BookActions.deleteBook),
      mergeMap(({ id }) =>
        this.bookService.delete(id).pipe(
          map(() => BookActions.deleteBookSuccess({ id })),
          catchError(error => of(BookActions.deleteBookFailure({ error: error.message })))
        )
      )
    )
  );

  constructor(
    private actions$: Actions,
    private bookService: BookService
  ) {}
}
