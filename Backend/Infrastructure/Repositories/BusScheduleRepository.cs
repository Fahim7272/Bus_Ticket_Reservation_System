using Application.Contracts.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class BusScheduleRepository : Repository<BusSchedule>, IBusScheduleRepository
{
    public BusScheduleRepository(BusReservationDbContext context) : base(context)
    {
    }

    public async Task<List<BusSchedule>> SearchAvailableAsync(string fromCity, string toCity, DateTime journeyDate)
    {
        return await _context.BusSchedules
            .Include(bs => bs.Bus)
                .ThenInclude(b => b.Seats)
            .Include(bs => bs.Route)
            .Include(bs => bs.Tickets)
            .Where(bs =>
                bs.Route.FromCity.ToLower() == fromCity.ToLower() &&
                bs.Route.ToCity.ToLower() == toCity.ToLower() &&
                bs.JourneyDate.Date == journeyDate.Date)
            .ToListAsync();
    }

    public async Task<BusSchedule?> GetWithDetailsAsync(Guid busScheduleId)
    {
        return await _context.BusSchedules
            .Include(bs => bs.Bus)
                .ThenInclude(b => b.Seats)
            .Include(bs => bs.Route)
            .Include(bs => bs.Tickets)
                .ThenInclude(t => t.Seat)
            .FirstOrDefaultAsync(bs => bs.Id == busScheduleId);
    }
}
