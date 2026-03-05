import { inject, Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { Store } from '@ngrx/store';
import { of } from 'rxjs';
import { map, mergeMap, exhaustMap, catchError, withLatestFrom, filter } from 'rxjs/operators';
import { AuthorService } from '../../../core/services/author.service';
import * as AuthorActions from './author.actions';
import { selectAuthorLoaded } from './author.selectors';

@Injectable()
export class AuthorEffects {
  private actions$ = inject(Actions);
  private authorService = inject(AuthorService);
  private store = inject(Store);

  loadAuthors$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AuthorActions.loadAuthors),
      withLatestFrom(this.store.select(selectAuthorLoaded)),
      filter(([, loaded]) => !loaded),
      exhaustMap(() =>
        this.authorService.getAll().pipe(
          map(authors => AuthorActions.loadAuthorsSuccess({ authors })),
          catchError(error => of(AuthorActions.loadAuthorsFailure({ error: error.message })))
        )
      )
    )
  );

  createAuthor$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AuthorActions.createAuthor),
      mergeMap(({ payload }) =>
        this.authorService.create(payload).pipe(
          map(author => AuthorActions.createAuthorSuccess({ author })),
          catchError(error => of(AuthorActions.createAuthorFailure({ error: error.message })))
        )
      )
    )
  );

  updateAuthor$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AuthorActions.updateAuthor),
      mergeMap(({ id, payload }) =>
        this.authorService.update(id, payload).pipe(
          map(author => AuthorActions.updateAuthorSuccess({ author })),
          catchError(error => of(AuthorActions.updateAuthorFailure({ error: error.message })))
        )
      )
    )
  );

  deleteAuthor$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AuthorActions.deleteAuthor),
      mergeMap(({ id }) =>
        this.authorService.delete(id).pipe(
          map(() => AuthorActions.deleteAuthorSuccess({ id })),
          catchError(error => of(AuthorActions.deleteAuthorFailure({ error: error.message })))
        )
      )
    )
  );

}
