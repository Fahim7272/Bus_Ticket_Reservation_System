import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BusService } from '../../services/bus.service';
import { AvailableBus, SeatPlan, Seat, BookingRequest } from '../../models/bus.model';

@Component({
  selector: 'app-search-results',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './search-results.html',
  styleUrl: './search-results.css',
})
export class SearchResultsComponent implements OnInit {
  buses: AvailableBus[] = [];
  loading = false;
  searchParams = {
    from: '',
    to: '',
    journeyDate: '',
  };

  // Seat selection state
  expandedBusId: string | null = null;
  seatPlan: SeatPlan | null = null;
  selectedSeats: Seat[] = [];
  loadingSeats = false;
  bookingInProgress = false;

  passengerInfo = {
    name: '',
    mobile: '',
    email: '',
    boardingPoint: '',
    droppingPoint: '',
  };

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private busService: BusService
  ) {}

  getTotalAvailableSeats(): number {
    return this.buses.reduce((sum, bus) => sum + bus.seatsLeft, 0);
  }

  ngOnInit() {
    this.route.queryParams.subscribe((params) => {
      this.searchParams.from = params['from'] || '';
      this.searchParams.to = params['to'] || '';
      this.searchParams.journeyDate = params['journeyDate'] || '';

      if (this.searchParams.from && this.searchParams.to && this.searchParams.journeyDate) {
        this.searchBuses();
      }
    });
  }

  searchBuses() {
    this.loading = true;
    this.busService
      .searchBuses(this.searchParams.from, this.searchParams.to, this.searchParams.journeyDate)
      .subscribe({
        next: (data) => {
          this.buses = data;
          this.loading = false;
        },
        error: (error) => {
          console.error('Error fetching buses:', error);
          this.loading = false;
        },
      });
  }

  toggleSeats(busScheduleId: string) {
    if (this.expandedBusId === busScheduleId) {
      // Close if already open
      this.expandedBusId = null;
      this.seatPlan = null;
      this.selectedSeats = [];
      return;
    }

    // Open new bus
    this.expandedBusId = busScheduleId;
    this.selectedSeats = [];
    this.loadSeats(busScheduleId);
  }

  loadSeats(busScheduleId: string) {
    this.loadingSeats = true;
    this.busService.getSeatPlan(busScheduleId).subscribe({
      next: (data) => {
        this.seatPlan = data;
        if (data.boardingPoints.length > 0) {
          this.passengerInfo.boardingPoint = data.boardingPoints[0];
        }
        if (data.droppingPoints.length > 0) {
          this.passengerInfo.droppingPoint = data.droppingPoints[0];
        }
        this.loadingSeats = false;
      },
      error: (error) => {
        console.error('Error loading seats:', error);
        this.loadingSeats = false;
      },
    });
  }

  toggleSeat(seat: Seat) {
    if (seat.status !== 'Available') return;

    const index = this.selectedSeats.findIndex((s) => s.seatId === seat.seatId);
    if (index > -1) {
      this.selectedSeats.splice(index, 1);
    } else {
      this.selectedSeats.push(seat);
    }
  }

  isSeatSelected(seat: Seat): boolean {
    return this.selectedSeats.some((s) => s.seatId === seat.seatId);
  }

  getSeatClass(seat: Seat): string {
    if (seat.status === 'Booked') return 'seat booked';
    if (seat.status === 'Sold') return 'seat sold';
    if (this.isSeatSelected(seat)) return 'seat selected';
    return 'seat available';
  }

  getTotalAmount(): number {
    if (!this.seatPlan) return 0;
    return this.selectedSeats.length * this.seatPlan.price;
  }

  bookSeats() {
    if (!this.seatPlan) return;

    if (this.selectedSeats.length === 0) {
      alert('Please select at least one seat');
      return;
    }

    if (!this.passengerInfo.mobile) {
      alert('Please enter mobile number');
      return;
    }

    const bookingRequest: BookingRequest = {
      busScheduleId: this.seatPlan.busScheduleId,
      seatIds: this.selectedSeats.map((s) => s.seatId),
      passengerName: this.passengerInfo.name || 'Guest',
      mobileNumber: this.passengerInfo.mobile,
      email: this.passengerInfo.email,
      boardingPoint: this.passengerInfo.boardingPoint,
      droppingPoint: this.passengerInfo.droppingPoint,
    };

    this.bookingInProgress = true;
    this.busService.bookSeats(bookingRequest).subscribe({
      next: (result) => {
        if (result.success) {
          alert(
            `✅ Booking Successful!\n\nReference: ${
              result.bookingReference
            }\nSeats: ${result.bookedSeats.join(', ')}\nTotal: ৳${result.totalAmount}`
          );
          this.router.navigate(['/']);
        } else {
          alert('Booking failed: ' + result.message);
        }
        this.bookingInProgress = false;
      },
      error: (error) => {
        console.error('Booking error:', error);
        alert('Booking failed. Please try again.');
        this.bookingInProgress = false;
      },
    });
  }

  modifySearch() {
    this.router.navigate(['/']);
  }

  formatDate(dateString: string): string {
    const date = new Date(dateString);
    return date.toLocaleDateString('en-GB', {
      day: '2-digit',
      month: 'short',
      year: 'numeric',
    });
  }

  getGridSeats(): Seat[][] {
    if (!this.seatPlan) return [];

    const maxRow = Math.max(...this.seatPlan.seats.map((s) => s.row));
    const maxCol = Math.max(...this.seatPlan.seats.map((s) => s.column));

    const grid: Seat[][] = [];
    for (let row = 1; row <= maxRow; row++) {
      const rowSeats: Seat[] = [];
      for (let col = 1; col <= maxCol; col++) {
        const seat = this.seatPlan.seats.find((s) => s.row === row && s.column === col);
        if (seat) {
          rowSeats.push(seat);
        }
      }
      if (rowSeats.length > 0) {
        grid.push(rowSeats);
      }
    }
    return grid;
  }
}
