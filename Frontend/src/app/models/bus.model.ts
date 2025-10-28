export interface AvailableBus {
  busScheduleId: string;
  companyName: string;
  busName: string;
  busType: string;
  startTime: string;
  arrivalTime: string;
  seatsLeft: number;
  totalSeats: number;
  price: number;
}

export interface SearchParams {
  from: string;
  to: string;
  journeyDate: string;
}

export interface Seat {
  seatId: string;
  seatNumber: string;
  row: number;
  column: number;
  seatType: string;
  status: string;
}

export interface SeatPlan {
  busScheduleId: string;
  companyName: string;
  busName: string;
  fromCity: string;
  toCity: string;
  journeyDate: string;
  startTime: string;
  price: number;
  seats: Seat[];
  boardingPoints: string[];
  droppingPoints: string[];
}

export interface BookingRequest {
  busScheduleId: string;
  seatIds: string[];
  passengerName: string;
  mobileNumber: string;
  email?: string;
  boardingPoint: string;
  droppingPoint: string;
}

export interface BookingResult {
  success: boolean;
  message: string;
  bookingReference?: string;
  bookedSeats: string[];
  totalAmount: number;
}
