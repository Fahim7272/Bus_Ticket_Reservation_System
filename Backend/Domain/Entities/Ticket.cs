namespace Domain.Entities;

public class Ticket
{
    public Guid Id { get; set; }
    public Guid BusScheduleId { get; set; }
    public Guid SeatId { get; set; }
    public Guid PassengerId { get; set; }
    public string BoardingPoint { get; set; } = string.Empty;
    public string DroppingPoint { get; set; } = string.Empty;
    public SeatStatus Status { get; set; }
    public decimal Price { get; set; }
    public DateTime BookingDate { get; set; } = DateTime.UtcNow;
    public string BookingReference { get; set; } = string.Empty;

    public virtual BusSchedule BusSchedule { get; set; } = null!;
    public virtual Seat Seat { get; set; } = null!;
    public virtual Passenger Passenger { get; set; } = null!;
}

public enum SeatStatus
{
    Available,
    Booked,
    Sold
}
