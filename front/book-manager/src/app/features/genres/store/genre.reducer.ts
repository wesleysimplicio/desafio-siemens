import { createEntityAdapter, EntityAdapter, EntityState } from '@ngrx/entity';
import { createReducer, on } from '@ngrx/store';
import { Genre } from '../../../core/models/genre.model';
import * as GenreActions from './genre.actions';

export interface GenreState extends EntityState<Genre> {
  loading: boolean;
  error: string | null;
}

export const adapter: EntityAdapter<Genre> = createEntityAdapter<Genre>();

export const initialState: GenreState = adapter.getInitialState({
  loading: false,
  error: null
});

export const genreReducer = createReducer(
  initialState,

  on(GenreActions.loadGenres, state => ({ ...state, loading: true, error: null })),
  on(GenreActions.loadGenresSuccess, (state, { genres }) =>
    adapter.setAll(genres, { ...state, loading: false })),
  on(GenreActions.loadGenresFailure, (state, { error }) =>
    ({ ...state, loading: false, error })),

  on(GenreActions.createGenre, state => ({ ...state, loading: true, error: null })),
  on(GenreActions.createGenreSuccess, (state, { genre }) =>
    adapter.addOne(genre, { ...state, loading: false })),
  on(GenreActions.createGenreFailure, (state, { error }) =>
    ({ ...state, loading: false, error })),

  on(GenreActions.updateGenre, state => ({ ...state, loading: true, error: null })),
  on(GenreActions.updateGenreSuccess, (state, { genre }) =>
    adapter.upsertOne(genre, { ...state, loading: false })),
  on(GenreActions.updateGenreFailure, (state, { error }) =>
    ({ ...state, loading: false, error })),

  on(GenreActions.deleteGenre, state => ({ ...state, loading: true, error: null })),
  on(GenreActions.deleteGenreSuccess, (state, { id }) =>
    adapter.removeOne(id, { ...state, loading: false })),
  on(GenreActions.deleteGenreFailure, (state, { error }) =>
    ({ ...state, loading: false, error })),

  on(GenreActions.clearGenreError, state => ({ ...state, error: null }))
);
