import { createEntityAdapter, EntityAdapter, EntityState } from '@ngrx/entity';
import { createReducer, on } from '@ngrx/store';
import { Book } from '../../../core/models/book.model';
import * as BookActions from './book.actions';

export interface BookState extends EntityState<Book> {
  loading: boolean;
  loaded: boolean;
  error: string | null;
}

export const adapter: EntityAdapter<Book> = createEntityAdapter<Book>();

export const initialState: BookState = adapter.getInitialState({
  loading: false,
  loaded: false,
  error: null
});

export const bookReducer = createReducer(
  initialState,

  on(BookActions.loadBooks, state => ({ ...state, loading: !state.loaded, error: null })),
  on(BookActions.loadBooksSuccess, (state, { books }) =>
    adapter.setAll(books, { ...state, loading: false, loaded: true })),
  on(BookActions.loadBooksFailure, (state, { error }) =>
    ({ ...state, loading: false, error })),

  on(BookActions.createBook, state => ({ ...state, loading: true, error: null })),
  on(BookActions.createBookSuccess, (state, { book }) =>
    adapter.addOne(book, { ...state, loading: false, loaded: false })),
  on(BookActions.createBookFailure, (state, { error }) =>
    ({ ...state, loading: false, error })),

  on(BookActions.updateBook, state => ({ ...state, loading: true, error: null })),
  on(BookActions.updateBookSuccess, (state, { book }) =>
    adapter.upsertOne(book, { ...state, loading: false, loaded: false })),
  on(BookActions.updateBookFailure, (state, { error }) =>
    ({ ...state, loading: false, error })),

  on(BookActions.deleteBook, state => ({ ...state, loading: true, error: null })),
  on(BookActions.deleteBookSuccess, (state, { id }) =>
    adapter.removeOne(id, { ...state, loading: false, loaded: false })),
  on(BookActions.deleteBookFailure, (state, { error }) =>
    ({ ...state, loading: false, error })),

  on(BookActions.clearBookError, state => ({ ...state, error: null }))
);
