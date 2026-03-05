import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Actions, ofType } from '@ngrx/effects';
import { Store } from '@ngrx/store';
import { Observable, Subject } from 'rxjs';
import { take, takeUntil } from 'rxjs/operators';
import {
  createAuthor, createAuthorSuccess, createAuthorFailure,
  updateAuthor, updateAuthorSuccess, updateAuthorFailure,
  loadAuthors, clearAuthorError
} from '../../store/author.actions';
import { selectAllAuthors, selectAuthorError, selectAuthorLoading, selectAuthorLoaded } from '../../store/author.selectors';

@Component({
  standalone: false,
  selector: 'app-author-form',
  templateUrl: './author-form.component.html',
  styleUrls: ['./author-form.component.scss']
})
export class AuthorFormComponent implements OnInit, OnDestroy {
  form: FormGroup;
  isEditMode = false;
  authorId: number | null = null;
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
    this.error$ = this.store.select(selectAuthorError);
    this.loading$ = this.store.select(selectAuthorLoading);
    this.form = this.fb.group({
      name: ['', [Validators.required, Validators.maxLength(150)]],
      bio: ['', Validators.maxLength(1000)]
    });
  }

  ngOnInit(): void {
    this.authorId = this.route.snapshot.paramMap.get('id')
      ? Number(this.route.snapshot.paramMap.get('id'))
      : null;
    this.isEditMode = this.authorId !== null;

    if (this.isEditMode) {
      this.store.select(selectAuthorLoaded).pipe(take(1)).subscribe(loaded => {
        if (!loaded) this.store.dispatch(loadAuthors());
      });
      this.store.select(selectAllAuthors)
        .pipe(takeUntil(this.destroy$))
        .subscribe(authors => {
          const author = authors.find(a => a.id === this.authorId);
          if (author) {
            this.form.patchValue({ name: author.name, bio: author.bio ?? '' });
          }
        });
    }
  }

  onSubmit(): void {
    if (this.form.invalid) return;

    const payload = {
      name: this.form.value.name.trim(),
      bio: this.form.value.bio?.trim() || null
    };

    if (this.isEditMode && this.authorId) {
      this.store.dispatch(updateAuthor({ id: this.authorId, payload }));
    } else {
      this.store.dispatch(createAuthor({ payload }));
    }

    this.actions$.pipe(
      ofType(createAuthorSuccess, updateAuthorSuccess, createAuthorFailure, updateAuthorFailure),
      take(1),
      takeUntil(this.destroy$)
    ).subscribe(action => {
      if (action.type === createAuthorSuccess.type || action.type === updateAuthorSuccess.type) {
        this.router.navigate(['/authors']);
      }
    });
  }

  dismissError(): void {
    this.store.dispatch(clearAuthorError());
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
