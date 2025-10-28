import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { BusService } from '../../services/bus.service';
import { SearchParams } from '../../models/bus.model';

@Component({
  selector: 'app-search',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './search.html',
  styleUrl: './search.css'
})
export class SearchComponent {
  searchParams: SearchParams = {
    from: 'Dhaka',
    to: 'Rajshahi',
    journeyDate: ''
  };

  trendingSearches = [
    { from: 'Dhaka', to: 'Rajshahi' },
    { from: 'Dhaka', to: 'Barisal' },
    { from: 'Dhaka', to: 'Cox\'s-Bazar' },
    { from: 'Dhaka', to: 'Chittagong' },
    { from: 'Dhaka', to: 'Chapainawabganj' }
  ];

  constructor(
    private busService: BusService,
    private router: Router
  ) {
    const today = new Date();
    this.searchParams.journeyDate = today.toISOString().split('T')[0];
  }

  searchBuses() {
    if (this.searchParams.from && this.searchParams.to && this.searchParams.journeyDate) {
      this.router.navigate(['/search-results'], {
        queryParams: this.searchParams
      });
    }
  }

  swapCities() {
    const temp = this.searchParams.from;
    this.searchParams.from = this.searchParams.to;
    this.searchParams.to = temp;
  }

  selectTrending(search: any) {
    this.searchParams.from = search.from;
    this.searchParams.to = search.to;
  }
}
