import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthorListComponent } from './components/author-list/author-list.component';
import { AuthorFormComponent } from './components/author-form/author-form.component';

const routes: Routes = [
  { path: '', component: AuthorListComponent },
  { path: 'new', component: AuthorFormComponent },
  { path: ':id/edit', component: AuthorFormComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuthorsRoutingModule {}
