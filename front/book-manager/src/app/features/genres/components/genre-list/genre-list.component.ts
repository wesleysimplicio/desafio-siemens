import { Component, OnInit, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { take } from 'rxjs/operators';
import { Genre } from '../../../../core/models/genre.model';
import { loadGenres, deleteGenre, clearGenreError } from '../../store/genre.actions';
import { selectAllGenres, selectGenreLoading, selectGenreError, selectGenreLoaded } from '../../store/genre.selectors';

@Component({
  standalone: false,
  selector: 'app-genre-list',
  templateUrl: './genre-list.component.html',
  styleUrls: ['./genre-list.component.scss']
})
export class GenreListComponent implements OnInit, OnDestroy {
  genres$: Observable<Genre[]>;
  loading$: Observable<boolean>;
  error$: Observable<string | null>;

  confirmDeleteId: number | null = null;

  constructor(private store: Store) {
    this.genres$ = this.store.select(selectAllGenres);
    this.loading$ = this.store.select(selectGenreLoading);
    this.error$ = this.store.select(selectGenreError);
  }

  ngOnInit(): void {
    this.store.select(selectGenreLoaded).pipe(take(1)).subscribe(loaded => {
      if (!loaded) this.store.dispatch(loadGenres());
    });
  }

  onDelete(id: number): void {
    this.confirmDeleteId = id;
  }

  confirmDelete(): void {
    if (this.confirmDeleteId !== null) {
      this.store.dispatch(deleteGenre({ id: this.confirmDeleteId }));
      this.confirmDeleteId = null;
    }
  }

  cancelDelete(): void {
    this.confirmDeleteId = null;
  }

  dismissError(): void {
    this.store.dispatch(clearGenreError());
  }

  ngOnDestroy(): void {
    this.store.dispatch(clearGenreError());
  }
}
