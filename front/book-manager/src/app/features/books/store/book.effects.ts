import { inject, Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { Store } from '@ngrx/store';
import { of } from 'rxjs';
import { map, mergeMap, exhaustMap, catchError, withLatestFrom, filter } from 'rxjs/operators';
import { BookService } from '../../../core/services/book.service';
import * as BookActions from './book.actions';
import { selectBookLoaded } from './book.selectors';

@Injectable()
export class BookEffects {
  private actions$ = inject(Actions);
  private bookService = inject(BookService);
  private store = inject(Store);

  loadBooks$ = createEffect(() =>
    this.actions$.pipe(
      ofType(BookActions.loadBooks),
      withLatestFrom(this.store.select(selectBookLoaded)),
      filter(([, loaded]) => !loaded),
      exhaustMap(() =>
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

}
