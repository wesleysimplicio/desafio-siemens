import { createFeatureSelector, createSelector } from '@ngrx/store';
import { adapter, GenreState } from './genre.reducer';

export const selectGenreState = createFeatureSelector<GenreState>('genres');

const { selectAll, selectEntities } = adapter.getSelectors();

export const selectAllGenres = createSelector(selectGenreState, selectAll);
export const selectGenreEntities = createSelector(selectGenreState, selectEntities);
export const selectGenreLoading = createSelector(selectGenreState, s => s.loading);
export const selectGenreLoaded = createSelector(selectGenreState, s => s.loaded);
export const selectGenreError = createSelector(selectGenreState, s => s.error);
export const selectGenreById = (id: number) =>
  createSelector(selectGenreEntities, entities => entities[id]);
