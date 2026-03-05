import { createFeatureSelector, createSelector } from '@ngrx/store';
import { adapter, BookState } from './book.reducer';

export const selectBookState = createFeatureSelector<BookState>('books');

const { selectAll, selectEntities } = adapter.getSelectors();

export const selectAllBooks = createSelector(selectBookState, selectAll);
export const selectBookEntities = createSelector(selectBookState, selectEntities);
export const selectBookLoading = createSelector(selectBookState, s => s.loading);
export const selectBookLoaded = createSelector(selectBookState, s => s.loaded);
export const selectBookError = createSelector(selectBookState, s => s.error);
export const selectBookById = (id: number) =>
  createSelector(selectBookEntities, entities => entities[id]);
