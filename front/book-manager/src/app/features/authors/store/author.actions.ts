import { createAction, props } from '@ngrx/store';
import { Author, CreateAuthor, UpdateAuthor } from '../../../core/models/author.model';

export const loadAuthors = createAction('[Authors] Load Authors');
export const loadAuthorsSuccess = createAction('[Authors] Load Authors Success', props<{ authors: Author[] }>());
export const loadAuthorsFailure = createAction('[Authors] Load Authors Failure', props<{ error: string }>());

export const createAuthor = createAction('[Authors] Create Author', props<{ payload: CreateAuthor }>());
export const createAuthorSuccess = createAction('[Authors] Create Author Success', props<{ author: Author }>());
export const createAuthorFailure = createAction('[Authors] Create Author Failure', props<{ error: string }>());

export const updateAuthor = createAction('[Authors] Update Author', props<{ id: number; payload: UpdateAuthor }>());
export const updateAuthorSuccess = createAction('[Authors] Update Author Success', props<{ author: Author }>());
export const updateAuthorFailure = createAction('[Authors] Update Author Failure', props<{ error: string }>());

export const deleteAuthor = createAction('[Authors] Delete Author', props<{ id: number }>());
export const deleteAuthorSuccess = createAction('[Authors] Delete Author Success', props<{ id: number }>());
export const deleteAuthorFailure = createAction('[Authors] Delete Author Failure', props<{ error: string }>());

export const clearAuthorError = createAction('[Authors] Clear Error');
