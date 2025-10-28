import { Routes } from '@angular/router';
import { SearchComponent } from './components/search/search';
import { SearchResultsComponent } from './components/search-results/search-results';
import { SeatSelectionComponent } from './components/seat-selection/seat-selection';

export const routes: Routes = [
  { path: '', component: SearchComponent },
  { path: 'search-results', component: SearchResultsComponent },
  { path: 'seat-selection/:busScheduleId', component: SeatSelectionComponent },
  { path: '**', redirectTo: '' }
];
