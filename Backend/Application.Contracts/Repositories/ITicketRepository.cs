using Domain.Entities;

namespace Application.Contracts.Repositories;

public interface ITicketRepository : IRepository<Ticket>
{
    Task<List<Ticket>> GetBookedSeatsForScheduleAsync(Guid busScheduleId);
    Task<bool> IsSeatBookedAsync(Guid seatId, Guid busScheduleId);
}
