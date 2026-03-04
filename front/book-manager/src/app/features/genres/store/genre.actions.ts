import { createAction, props } from '@ngrx/store';
import { Genre, CreateGenre, UpdateGenre } from '../../../core/models/genre.model';

// Load All
export const loadGenres = createAction('[Genres] Load Genres');
export const loadGenresSuccess = createAction('[Genres] Load Genres Success', props<{ genres: Genre[] }>());
export const loadGenresFailure = createAction('[Genres] Load Genres Failure', props<{ error: string }>());

// Create
export const createGenre = createAction('[Genres] Create Genre', props<{ payload: CreateGenre }>());
export const createGenreSuccess = createAction('[Genres] Create Genre Success', props<{ genre: Genre }>());
export const createGenreFailure = createAction('[Genres] Create Genre Failure', props<{ error: string }>());

// Update
export const updateGenre = createAction('[Genres] Update Genre', props<{ id: number; payload: UpdateGenre }>());
export const updateGenreSuccess = createAction('[Genres] Update Genre Success', props<{ genre: Genre }>());
export const updateGenreFailure = createAction('[Genres] Update Genre Failure', props<{ error: string }>());

// Delete
export const deleteGenre = createAction('[Genres] Delete Genre', props<{ id: number }>());
export const deleteGenreSuccess = createAction('[Genres] Delete Genre Success', props<{ id: number }>());
export const deleteGenreFailure = createAction('[Genres] Delete Genre Failure', props<{ error: string }>());

// UI
export const clearGenreError = createAction('[Genres] Clear Error');
