import { createEntityAdapter, EntityAdapter, EntityState } from '@ngrx/entity';
import { createReducer, on } from '@ngrx/store';
import { Author } from '../../../core/models/author.model';
import * as AuthorActions from './author.actions';

export interface AuthorState extends EntityState<Author> {
  loading: boolean;
  loaded: boolean;
  error: string | null;
}

export const adapter: EntityAdapter<Author> = createEntityAdapter<Author>();

export const initialState: AuthorState = adapter.getInitialState({
  loading: false,
  loaded: false,
  error: null
});

export const authorReducer = createReducer(
  initialState,

  on(AuthorActions.loadAuthors, state => ({ ...state, loading: true, error: null })),
  on(AuthorActions.loadAuthorsSuccess, (state, { authors }) =>
    adapter.setAll(authors, { ...state, loading: false, loaded: true })),
  on(AuthorActions.loadAuthorsFailure, (state, { error }) =>
    ({ ...state, loading: false, error })),

  on(AuthorActions.createAuthor, state => ({ ...state, loading: true, error: null })),
  on(AuthorActions.createAuthorSuccess, (state, { author }) =>
    adapter.addOne(author, { ...state, loading: false })),
  on(AuthorActions.createAuthorFailure, (state, { error }) =>
    ({ ...state, loading: false, error })),

  on(AuthorActions.updateAuthor, state => ({ ...state, loading: true, error: null })),
  on(AuthorActions.updateAuthorSuccess, (state, { author }) =>
    adapter.upsertOne(author, { ...state, loading: false })),
  on(AuthorActions.updateAuthorFailure, (state, { error }) =>
    ({ ...state, loading: false, error })),

  on(AuthorActions.deleteAuthor, state => ({ ...state, loading: true, error: null })),
  on(AuthorActions.deleteAuthorSuccess, (state, { id }) =>
    adapter.removeOne(id, { ...state, loading: false })),
  on(AuthorActions.deleteAuthorFailure, (state, { error }) =>
    ({ ...state, loading: false, error })),

  on(AuthorActions.clearAuthorError, state => ({ ...state, error: null }))
);
