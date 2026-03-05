import { Component, OnInit, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { take } from 'rxjs/operators';
import { Author } from '../../../../core/models/author.model';
import { loadAuthors, deleteAuthor, clearAuthorError } from '../../store/author.actions';
import { selectAllAuthors, selectAuthorLoading, selectAuthorError, selectAuthorLoaded } from '../../store/author.selectors';

@Component({
  standalone: false,
  selector: 'app-author-list',
  templateUrl: './author-list.component.html',
  styleUrls: ['./author-list.component.scss']
})
export class AuthorListComponent implements OnInit, OnDestroy {
  authors$: Observable<Author[]>;
  loading$: Observable<boolean>;
  error$: Observable<string | null>;
  confirmDeleteId: number | null = null;

  constructor(private store: Store) {
    this.authors$ = this.store.select(selectAllAuthors);
    this.loading$ = this.store.select(selectAuthorLoading);
    this.error$ = this.store.select(selectAuthorError);
  }

  ngOnInit(): void {
    this.store.select(selectAuthorLoaded).pipe(take(1)).subscribe(loaded => {
      if (!loaded) this.store.dispatch(loadAuthors());
    });
  }

  onDelete(id: number): void {
    this.confirmDeleteId = id;
  }

  confirmDelete(): void {
    if (this.confirmDeleteId !== null) {
      this.store.dispatch(deleteAuthor({ id: this.confirmDeleteId }));
      this.confirmDeleteId = null;
    }
  }

  cancelDelete(): void {
    this.confirmDeleteId = null;
  }

  dismissError(): void {
    this.store.dispatch(clearAuthorError());
  }

  ngOnDestroy(): void {
    this.store.dispatch(clearAuthorError());
  }
}
