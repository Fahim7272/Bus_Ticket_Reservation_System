namespace Domain.Entities;

public class Bus
{
    public Guid Id { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string BusName { get; set; } = string.Empty;
    public string BusNumber { get; set; } = string.Empty;
    public int TotalSeats { get; set; }
    public BusType BusType { get; set; }

    public virtual ICollection<BusSchedule> BusSchedules { get; set; } = new List<BusSchedule>();
    public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();
}

public enum BusType
{
    AC,
    NonAC
}
