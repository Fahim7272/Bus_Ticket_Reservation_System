import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BusService } from '../../services/bus.service';
import { SeatPlan, Seat, BookingRequest } from '../../models/bus.model';

@Component({
  selector: 'app-seat-selection',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './seat-selection.html',
  styleUrl: './seat-selection.css'
})
export class SeatSelectionComponent implements OnInit {
  seatPlan: SeatPlan | null = null;
  selectedSeats: Seat[] = [];
  loading = false;

  passengerInfo = {
    name: '',
    mobile: '',
    email: '',
    boardingPoint: '',
    droppingPoint: ''
  };

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private busService: BusService
  ) {}

  ngOnInit() {
    const busScheduleId = this.route.snapshot.paramMap.get('busScheduleId');
    if (busScheduleId) {
      this.loadSeatPlan(busScheduleId);
    }
  }

  loadSeatPlan(busScheduleId: string) {
    this.loading = true;
    this.busService.getSeatPlan(busScheduleId).subscribe({
      next: (data) => {
        this.seatPlan = data;
        if (data.boardingPoints.length > 0) {
          this.passengerInfo.boardingPoint = data.boardingPoints[0];
        }
        if (data.droppingPoints.length > 0) {
          this.passengerInfo.droppingPoint = data.droppingPoints[0];
        }
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading seat plan:', error);
        alert('Failed to load seat plan');
        this.loading = false;
      }
    });
  }

  toggleSeat(seat: Seat) {
    if (seat.status !== 'Available') return;

    const index = this.selectedSeats.findIndex(s => s.seatId === seat.seatId);
    if (index > -1) {
      this.selectedSeats.splice(index, 1);
    } else {
      this.selectedSeats.push(seat);
    }
  }

  isSeatSelected(seat: Seat): boolean {
    return this.selectedSeats.some(s => s.seatId === seat.seatId);
  }

  getSeatClass(seat: Seat): string {
    if (seat.status === 'Booked') return 'seat booked';
    if (seat.status === 'Sold') return 'seat sold';
    if (this.isSeatSelected(seat)) return 'seat selected';
    return 'seat available';
  }

  getTotalAmount(): number {
    return this.selectedSeats.length * (this.seatPlan?.price || 0);
  }

  bookSeats() {
    if (this.selectedSeats.length === 0) {
      alert('Please select at least one seat');
      return;
    }

    if (!this.passengerInfo.name || !this.passengerInfo.mobile) {
      alert('Please enter passenger name and mobile number');
      return;
    }

    const bookingRequest: BookingRequest = {
      busScheduleId: this.seatPlan!.busScheduleId,
      seatIds: this.selectedSeats.map(s => s.seatId),
      passengerName: this.passengerInfo.name,
      mobileNumber: this.passengerInfo.mobile,
      email: this.passengerInfo.email,
      boardingPoint: this.passengerInfo.boardingPoint,
      droppingPoint: this.passengerInfo.droppingPoint
    };

    this.loading = true;
    this.busService.bookSeats(bookingRequest).subscribe({
      next: (result) => {
        if (result.success) {
          alert(`✅ Booking Successful!\n\nBooking Reference: ${result.bookingReference}\nSeats: ${result.bookedSeats.join(', ')}\nTotal: ৳${result.totalAmount}`);
          this.router.navigate(['/']);
        } else {
          alert('❌ Booking Failed: ' + result.message);
        }
        this.loading = false;
      },
      error: (error) => {
        console.error('Booking error:', error);
        alert('❌ Booking failed. Please try again.');
        this.loading = false;
      }
    });
  }

  goBack() {
    this.router.navigate(['/search-results'], {
      queryParams: {
        from: this.seatPlan?.fromCity,
        to: this.seatPlan?.toCity,
        journeyDate: this.seatPlan?.journeyDate
      }
    });
  }

  getGridSeats(): Seat[][] {
    if (!this.seatPlan) return [];

    const maxRow = Math.max(...this.seatPlan.seats.map(s => s.row));
    const maxCol = Math.max(...this.seatPlan.seats.map(s => s.column));

    const grid: Seat[][] = [];
    for (let row = 1; row <= maxRow; row++) {
      const rowSeats: Seat[] = [];
      for (let col = 1; col <= maxCol; col++) {
        const seat = this.seatPlan.seats.find(s => s.row === row && s.column === col);
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
