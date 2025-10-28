namespace Domain.Entities;

public class BusSchedule
{
    public Guid Id { get; set; }
    public Guid BusId { get; set; }
    public Guid RouteId { get; set; }
    public DateTime JourneyDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan ArrivalTime { get; set; }
    public decimal Price { get; set; }

    public virtual Bus Bus { get; set; } = null!;
    public virtual Route Route { get; set; } = null!;
    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
