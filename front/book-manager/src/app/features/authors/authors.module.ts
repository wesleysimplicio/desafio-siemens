import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { AuthorsRoutingModule } from './authors-routing.module';
import { AuthorListComponent } from './components/author-list/author-list.component';
import { AuthorFormComponent } from './components/author-form/author-form.component';
import { authorReducer } from './store/author.reducer';
import { AuthorEffects } from './store/author.effects';

@NgModule({
  declarations: [AuthorListComponent, AuthorFormComponent],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    AuthorsRoutingModule,
    StoreModule.forFeature('authors', authorReducer),
    EffectsModule.forFeature([AuthorEffects])
  ]
})
export class AuthorsModule {}
