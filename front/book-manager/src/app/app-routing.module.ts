import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path: 'genres',
    loadChildren: () =>
      import('./features/genres/genres.module').then(m => m.GenresModule)
  },
  {
    path: 'authors',
    loadChildren: () =>
      import('./features/authors/authors.module').then(m => m.AuthorsModule)
  },
  {
    path: 'books',
    loadChildren: () =>
      import('./features/books/books.module').then(m => m.BooksModule)
  },
  { path: '', redirectTo: 'books', pathMatch: 'full' },
  { path: '**', redirectTo: 'books' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
