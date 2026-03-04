import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { GenresRoutingModule } from './genres-routing.module';
import { GenreListComponent } from './components/genre-list/genre-list.component';
import { GenreFormComponent } from './components/genre-form/genre-form.component';
import { genreReducer } from './store/genre.reducer';
import { GenreEffects } from './store/genre.effects';

@NgModule({
  declarations: [GenreListComponent, GenreFormComponent],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    GenresRoutingModule,
    StoreModule.forFeature('genres', genreReducer),
    EffectsModule.forFeature([GenreEffects])
  ]
})
export class GenresModule {}
