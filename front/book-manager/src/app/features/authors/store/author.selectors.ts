import { createFeatureSelector, createSelector } from '@ngrx/store';
import { adapter, AuthorState } from './author.reducer';

export const selectAuthorState = createFeatureSelector<AuthorState>('authors');

const { selectAll, selectEntities } = adapter.getSelectors();

export const selectAllAuthors = createSelector(selectAuthorState, selectAll);
export const selectAuthorEntities = createSelector(selectAuthorState, selectEntities);
export const selectAuthorLoading = createSelector(selectAuthorState, s => s.loading);
export const selectAuthorError = createSelector(selectAuthorState, s => s.error);
export const selectAuthorById = (id: number) =>
  createSelector(selectAuthorEntities, entities => entities[id]);
