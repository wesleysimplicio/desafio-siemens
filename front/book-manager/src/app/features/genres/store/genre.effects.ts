import { inject, Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { map, mergeMap, catchError } from 'rxjs/operators';
import { GenreService } from '../../../core/services/genre.service';
import * as GenreActions from './genre.actions';

@Injectable()
export class GenreEffects {
  private actions$ = inject(Actions);
  private genreService = inject(GenreService);

  loadGenres$ = createEffect(() =>
    this.actions$.pipe(
      ofType(GenreActions.loadGenres),
      mergeMap(() =>
        this.genreService.getAll().pipe(
          map(genres => GenreActions.loadGenresSuccess({ genres })),
          catchError(error => of(GenreActions.loadGenresFailure({ error: error.message })))
        )
      )
    )
  );

  createGenre$ = createEffect(() =>
    this.actions$.pipe(
      ofType(GenreActions.createGenre),
      mergeMap(({ payload }) =>
        this.genreService.create(payload).pipe(
          map(genre => GenreActions.createGenreSuccess({ genre })),
          catchError(error => of(GenreActions.createGenreFailure({ error: error.message })))
        )
      )
    )
  );

  updateGenre$ = createEffect(() =>
    this.actions$.pipe(
      ofType(GenreActions.updateGenre),
      mergeMap(({ id, payload }) =>
        this.genreService.update(id, payload).pipe(
          map(genre => GenreActions.updateGenreSuccess({ genre })),
          catchError(error => of(GenreActions.updateGenreFailure({ error: error.message })))
        )
      )
    )
  );

  deleteGenre$ = createEffect(() =>
    this.actions$.pipe(
      ofType(GenreActions.deleteGenre),
      mergeMap(({ id }) =>
        this.genreService.delete(id).pipe(
          map(() => GenreActions.deleteGenreSuccess({ id })),
          catchError(error => of(GenreActions.deleteGenreFailure({ error: error.message })))
        )
      )
    )
  );

}
