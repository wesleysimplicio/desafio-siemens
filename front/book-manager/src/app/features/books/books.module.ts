import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { BooksRoutingModule } from './books-routing.module';
import { BookListComponent } from './components/book-list/book-list.component';
import { BookFormComponent } from './components/book-form/book-form.component';
import { bookReducer } from './store/book.reducer';
import { BookEffects } from './store/book.effects';
import { genreReducer } from '../genres/store/genre.reducer';
import { GenreEffects } from '../genres/store/genre.effects';
import { authorReducer } from '../authors/store/author.reducer';
import { AuthorEffects } from '../authors/store/author.effects';

@NgModule({
  declarations: [BookListComponent, BookFormComponent],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    BooksRoutingModule,
    StoreModule.forFeature('books', bookReducer),
    StoreModule.forFeature('genres', genreReducer),
    StoreModule.forFeature('authors', authorReducer),
    EffectsModule.forFeature([BookEffects, GenreEffects, AuthorEffects])
  ]
})
export class BooksModule {}
