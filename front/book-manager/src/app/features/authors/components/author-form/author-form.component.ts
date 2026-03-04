import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { Subject } from 'rxjs';
import { takeUntil, filter } from 'rxjs/operators';
import { createAuthor, updateAuthor, loadAuthors, clearAuthorError } from '../../store/author.actions';
import { selectAllAuthors, selectAuthorError, selectAuthorLoading } from '../../store/author.selectors';

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
  error$!: import('rxjs').Observable<string | null>;
  loading$!: import('rxjs').Observable<boolean>;
  private destroy$ = new Subject<void>();

  constructor(
    private fb: FormBuilder,
    private store: Store,
    private route: ActivatedRoute,
    private router: Router
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
      this.store.dispatch(loadAuthors());
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

    this.loading$.pipe(
      takeUntil(this.destroy$),
      filter(loading => !loading)
    ).subscribe(() => {
      this.error$.pipe(takeUntil(this.destroy$)).subscribe(error => {
        if (!error) this.router.navigate(['/authors']);
      });
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
