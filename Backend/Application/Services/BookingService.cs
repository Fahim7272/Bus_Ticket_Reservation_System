using Application.Contracts.DTOs;
using Application.Contracts.Repositories;
using Application.Contracts.Services;
using Domain.Entities;

namespace Application.Services;

public class BookingService : IBookingService
{
    private readonly IBusScheduleRepository _busScheduleRepository;
    private readonly ITicketRepository _ticketRepository;
    private readonly IPassengerRepository _passengerRepository;

    public BookingService(
        IBusScheduleRepository busScheduleRepository,
        ITicketRepository ticketRepository,
        IPassengerRepository passengerRepository)
    {
        _busScheduleRepository = busScheduleRepository;
        _ticketRepository = ticketRepository;
        _passengerRepository = passengerRepository;
    }

    public async Task<SeatPlanDto> GetSeatPlanAsync(Guid busScheduleId)
    {
        var schedule = await _busScheduleRepository.GetWithDetailsAsync(busScheduleId);

        if (schedule == null)
            throw new Exception("Bus schedule not found");

        var bookedTickets = await _ticketRepository.GetBookedSeatsForScheduleAsync(busScheduleId);
        var bookedSeatIds = bookedTickets.Select(t => t.SeatId).ToHashSet();

        var seatDtos = schedule.Bus.Seats.Select(seat => new SeatDto
        {
            SeatId = seat.Id,
            SeatNumber = seat.SeatNumber,
            Row = seat.Row,
            Column = seat.Column,
            SeatType = seat.SeatType.ToString(),
            Status = bookedSeatIds.Contains(seat.Id) ? "Booked" : "Available"
        }).ToList();

        return new SeatPlanDto
        {
            BusScheduleId = schedule.Id,
            CompanyName = schedule.Bus.CompanyName,
            BusName = schedule.Bus.BusName,
            FromCity = schedule.Route.FromCity,
            ToCity = schedule.Route.ToCity,
            JourneyDate = schedule.JourneyDate,
            StartTime = schedule.StartTime.ToString(@"hh\:mm"),
            Price = schedule.Price,
            Seats = seatDtos,
            BoardingPoints = new List<string> { schedule.Route.FromCity + " Counter" },
            DroppingPoints = new List<string> { schedule.Route.ToCity + " Counter" }
        };
    }

    public async Task<BookSeatResultDto> BookSeatAsync(BookSeatInputDto input)
    {
        // Validate seats availability
        foreach (var seatId in input.SeatIds)
        {
            var isBooked = await _ticketRepository.IsSeatBookedAsync(seatId, input.BusScheduleId);
            if (isBooked)
            {
                return new BookSeatResultDto
                {
                    Success = false,
                    Message = "One or more selected seats are already booked"
                };
            }
        }

        // Get or create passenger
        var passenger = await _passengerRepository.GetByMobileNumberAsync(input.MobileNumber);

        if (passenger == null)
        {
            passenger = new Passenger
            {
                Id = Guid.NewGuid(),
                Name = input.PassengerName,
                MobileNumber = input.MobileNumber,
                Email = input.Email
            };
            await _passengerRepository.AddAsync(passenger);
        }

        // Get schedule for price
        var schedule = await _busScheduleRepository.GetWithDetailsAsync(input.BusScheduleId);
        if (schedule == null)
        {
            return new BookSeatResultDto
            {
                Success = false,
                Message = "Bus schedule not found"
            };
        }

        // Create tickets
        var bookingReference = $"BUS{DateTime.Now:yyyyMMddHHmmss}";
        var bookedSeats = new List<string>();

        foreach (var seatId in input.SeatIds)
        {
            var seat = schedule.Bus.Seats.FirstOrDefault(s => s.Id == seatId);
            if (seat != null)
            {
                var ticket = new Ticket
                {
                    Id = Guid.NewGuid(),
                    BusScheduleId = input.BusScheduleId,
                    SeatId = seatId,
                    PassengerId = passenger.Id,
                    BoardingPoint = input.BoardingPoint,
                    DroppingPoint = input.DroppingPoint,
                    Status = SeatStatus.Booked,
                    Price = schedule.Price,
                    BookingDate = DateTime.UtcNow,
                    BookingReference = bookingReference
                };

                await _ticketRepository.AddAsync(ticket);
                bookedSeats.Add(seat.SeatNumber);
            }
        }

        await _ticketRepository.SaveChangesAsync();

        return new BookSeatResultDto
        {
            Success = true,
            Message = "Booking successful",
            BookingReference = bookingReference,
            BookedSeats = bookedSeats,
            TotalAmount = schedule.Price * input.SeatIds.Count
        };
    }
}
