import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Actions, ofType } from '@ngrx/effects';
import { Store } from '@ngrx/store';
import { Observable, Subject } from 'rxjs';
import { take, takeUntil } from 'rxjs/operators';
import {
  createGenre, createGenreSuccess, createGenreFailure,
  updateGenre, updateGenreSuccess, updateGenreFailure,
  loadGenres, clearGenreError
} from '../../store/genre.actions';
import { selectAllGenres, selectGenreError, selectGenreLoading, selectGenreLoaded } from '../../store/genre.selectors';

@Component({
  standalone: false,
  selector: 'app-genre-form',
  templateUrl: './genre-form.component.html',
  styleUrls: ['./genre-form.component.scss']
})
export class GenreFormComponent implements OnInit, OnDestroy {
  form: FormGroup;
  isEditMode = false;
  genreId: number | null = null;
  error$!: Observable<string | null>;
  loading$!: Observable<boolean>;
  private destroy$ = new Subject<void>();

  constructor(
    private fb: FormBuilder,
    private store: Store,
    private route: ActivatedRoute,
    private router: Router,
    private actions$: Actions
  ) {
    this.error$ = this.store.select(selectGenreError);
    this.loading$ = this.store.select(selectGenreLoading);
    this.form = this.fb.group({
      name: ['', [Validators.required, Validators.maxLength(100)]]
    });
  }

  ngOnInit(): void {
    this.genreId = this.route.snapshot.paramMap.get('id')
      ? Number(this.route.snapshot.paramMap.get('id'))
      : null;
    this.isEditMode = this.genreId !== null;

    if (this.isEditMode) {
      this.store.select(selectGenreLoaded).pipe(take(1)).subscribe(loaded => {
        if (!loaded) this.store.dispatch(loadGenres());
      });
      this.store.select(selectAllGenres)
        .pipe(takeUntil(this.destroy$))
        .subscribe(genres => {
          const genre = genres.find(g => g.id === this.genreId);
          if (genre) {
            this.form.patchValue({ name: genre.name });
          }
        });
    }
  }

  onSubmit(): void {
    if (this.form.invalid) return;

    const payload = { name: this.form.value.name.trim() };

    if (this.isEditMode && this.genreId) {
      this.store.dispatch(updateGenre({ id: this.genreId, payload }));
    } else {
      this.store.dispatch(createGenre({ payload }));
    }

    this.actions$.pipe(
      ofType(createGenreSuccess, updateGenreSuccess, createGenreFailure, updateGenreFailure),
      take(1),
      takeUntil(this.destroy$)
    ).subscribe(action => {
      if (action.type === createGenreSuccess.type || action.type === updateGenreSuccess.type) {
        this.router.navigate(['/genres']);
      }
    });
  }

  dismissError(): void {
    this.store.dispatch(clearGenreError());
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
