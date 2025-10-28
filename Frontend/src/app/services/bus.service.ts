import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AvailableBus, SeatPlan, BookingRequest, BookingResult } from '../models/bus.model';

@Injectable({
  providedIn: 'root'
})
export class BusService {
  private apiUrl = 'https://localhost:7081/api';

  constructor(private http: HttpClient) { }

  searchBuses(from: string, to: string, journeyDate: string): Observable<AvailableBus[]> {
    const params = new HttpParams()
      .set('from', from)
      .set('to', to)
      .set('journeyDate', journeyDate);

    return this.http.get<AvailableBus[]>(`${this.apiUrl}/search/available-buses`, { params });
  }

  getSeatPlan(busScheduleId: string): Observable<SeatPlan> {
    return this.http.get<SeatPlan>(`${this.apiUrl}/booking/seat-plan/${busScheduleId}`);
  }

  bookSeats(bookingRequest: BookingRequest): Observable<BookingResult> {
    return this.http.post<BookingResult>(`${this.apiUrl}/booking/book-seat`, bookingRequest);
  }
}
