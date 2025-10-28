using Domain.Entities;

namespace Application.Contracts.Repositories;

public interface IBusScheduleRepository : IRepository<BusSchedule>
{
    Task<List<BusSchedule>> SearchAvailableAsync(string fromCity, string toCity, DateTime journeyDate);
    Task<BusSchedule?> GetWithDetailsAsync(Guid busScheduleId);
}
