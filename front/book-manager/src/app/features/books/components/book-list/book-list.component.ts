import { Component, OnInit, OnDestroy, ChangeDetectionStrategy } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { Book } from '../../../../core/models/book.model';
import { loadBooks, deleteBook, clearBookError } from '../../store/book.actions';
import { selectAllBooks, selectBookLoading, selectBookError } from '../../store/book.selectors';

@Component({
  standalone: false,
  selector: 'app-book-list',
  templateUrl: './book-list.component.html',
  styleUrls: ['./book-list.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class BookListComponent implements OnInit, OnDestroy {
  books$: Observable<Book[]>;
  loading$: Observable<boolean>;
  error$: Observable<string | null>;
  confirmDeleteId: number | null = null;

  constructor(private store: Store) {
    this.books$ = this.store.select(selectAllBooks);
    this.loading$ = this.store.select(selectBookLoading);
    this.error$ = this.store.select(selectBookError);
  }

  ngOnInit(): void {
    this.store.dispatch(loadBooks());
  }

  onDelete(id: number): void {
    this.confirmDeleteId = id;
  }

  confirmDelete(): void {
    if (this.confirmDeleteId !== null) {
      this.store.dispatch(deleteBook({ id: this.confirmDeleteId }));
      this.confirmDeleteId = null;
    }
  }

  cancelDelete(): void {
    this.confirmDeleteId = null;
  }

  dismissError(): void {
    this.store.dispatch(clearBookError());
  }

  trackById(index: number, item: Book): number {
    return item.id;
  }

  ngOnDestroy(): void {
    this.store.dispatch(clearBookError());
  }
}
