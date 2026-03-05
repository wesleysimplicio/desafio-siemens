import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Actions, ofType } from '@ngrx/effects';
import { Store } from '@ngrx/store';
import { Observable, Subject } from 'rxjs';
import { take, takeUntil, filter } from 'rxjs/operators';

import { Genre } from '../../../../core/models/genre.model';
import { Author } from '../../../../core/models/author.model';
import { Book } from '../../../../core/models/book.model';

import { loadGenres } from '../../../genres/store/genre.actions';
import { loadAuthors } from '../../../authors/store/author.actions';
import {
  createBook, createBookSuccess, createBookFailure,
  updateBook, updateBookSuccess, updateBookFailure,
  loadBooks
} from '../../store/book.actions';

import { selectAllGenres } from '../../../genres/store/genre.selectors';
import { selectAllAuthors } from '../../../authors/store/author.selectors';
import { selectBookById, selectBookLoading, selectBookError } from '../../store/book.selectors';

@Component({
  standalone: false,
  selector: 'app-book-form',
  templateUrl: './book-form.component.html',
  styleUrls: ['./book-form.component.scss']
})
export class BookFormComponent implements OnInit, OnDestroy {
  form!: FormGroup;
  isEdit = false;
  bookId: number | null = null;

  genres$!: Observable<Genre[]>;
  authors$!: Observable<Author[]>;
  loading$!: Observable<boolean>;
  error$!: Observable<string | null>;

  private destroy$ = new Subject<void>();

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private store: Store,
    private actions$: Actions
  ) {
    this.genres$ = this.store.select(selectAllGenres);
    this.authors$ = this.store.select(selectAllAuthors);
    this.loading$ = this.store.select(selectBookLoading);
    this.error$ = this.store.select(selectBookError);
  }

  ngOnInit(): void {
    this.store.dispatch(loadGenres());
    this.store.dispatch(loadAuthors());

    this.form = this.fb.group({
      title: ['', [Validators.required, Validators.maxLength(200)]],
      isbn: ['', [Validators.required, Validators.maxLength(20)]],
      publishedYear: [null, [Validators.required, Validators.min(1000), Validators.max(9999)]],
      authorId: [null, Validators.required],
      genreId: [null, Validators.required],
      description: ['', Validators.maxLength(1000)]
    });

    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      this.isEdit = true;
      this.bookId = +idParam;
      this.store.dispatch(loadBooks());
      this.store.select(selectBookById(this.bookId))
        .pipe(
          filter((book): book is Book => !!book),
          take(1),
          takeUntil(this.destroy$)
        )
        .subscribe(book => {
          this.form.patchValue({
            title: book.title,
            isbn: book.isbn,
            publishedYear: book.publishedYear,
            authorId: book.authorId,
            genreId: book.genreId,
            description: book.description ?? ''
          });
        });
    }
  }

  get title() { return this.form.get('title'); }
  get isbn() { return this.form.get('isbn'); }
  get publishedYear() { return this.form.get('publishedYear'); }
  get authorId() { return this.form.get('authorId'); }
  get genreId() { return this.form.get('genreId'); }
  get description() { return this.form.get('description'); }

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    const value = this.form.value;
    if (this.isEdit && this.bookId !== null) {
      this.store.dispatch(updateBook({ id: this.bookId, payload: value }));
    } else {
      this.store.dispatch(createBook({ payload: value }));
    }
    this.actions$.pipe(
      ofType(createBookSuccess, updateBookSuccess, createBookFailure, updateBookFailure),
      take(1),
      takeUntil(this.destroy$)
    ).subscribe(action => {
      if (action.type === createBookSuccess.type || action.type === updateBookSuccess.type) {
        this.router.navigate(['/books']);
      }
    });
  }

  onCancel(): void {
    this.router.navigate(['/books']);
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
