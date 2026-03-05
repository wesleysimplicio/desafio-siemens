import { Component, OnInit, OnDestroy, ChangeDetectionStrategy } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { Author } from '../../../../core/models/author.model';
import { loadAuthors, deleteAuthor, clearAuthorError } from '../../store/author.actions';
import { selectAllAuthors, selectAuthorLoading, selectAuthorError } from '../../store/author.selectors';

@Component({
  standalone: false,
  selector: 'app-author-list',
  templateUrl: './author-list.component.html',
  styleUrls: ['./author-list.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
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
    this.store.dispatch(loadAuthors());
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

  trackById(index: number, item: Author): number {
    return item.id;
  }

  ngOnDestroy(): void {
    this.store.dispatch(clearAuthorError());
  }
}
