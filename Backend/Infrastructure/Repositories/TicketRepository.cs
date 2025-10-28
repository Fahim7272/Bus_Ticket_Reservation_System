using Application.Contracts.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TicketRepository : Repository<Ticket>, ITicketRepository
{
    public TicketRepository(BusReservationDbContext context) : base(context)
    {
    }

    public async Task<List<Ticket>> GetBookedSeatsForScheduleAsync(Guid busScheduleId)
    {
        return await _context.Tickets
            .Include(t => t.Seat)
            .Where(t => t.BusScheduleId == busScheduleId &&
                       (t.Status == SeatStatus.Booked || t.Status == SeatStatus.Sold))
            .ToListAsync();
    }

    public async Task<bool> IsSeatBookedAsync(Guid seatId, Guid busScheduleId)
    {
        return await _context.Tickets
            .AnyAsync(t => t.SeatId == seatId &&
                          t.BusScheduleId == busScheduleId &&
                          (t.Status == SeatStatus.Booked || t.Status == SeatStatus.Sold));
    }
}
