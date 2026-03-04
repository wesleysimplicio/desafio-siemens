import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { GenreListComponent } from './components/genre-list/genre-list.component';
import { GenreFormComponent } from './components/genre-form/genre-form.component';

const routes: Routes = [
  { path: '', component: GenreListComponent },
  { path: 'new', component: GenreFormComponent },
  { path: ':id/edit', component: GenreFormComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GenresRoutingModule {}
