namespace Domain.Entities;

public class Seat
{
    public Guid Id { get; set; }
    public Guid BusId { get; set; }
    public string SeatNumber { get; set; } = string.Empty;
    public int Row { get; set; }
    public int Column { get; set; }
    public SeatType SeatType { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual Bus Bus { get; set; } = null!;
}

public enum SeatType
{
    Regular,
    Premium,
    Sleeper
}
