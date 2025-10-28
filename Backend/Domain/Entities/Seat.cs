namespace Domain.Entities;

public class Seat
{
    public Guid Id { get; set; }
    public Guid BusId { get; set; }
    public string SeatNumber { get; set; } = string.Empty;
    public int Row { get; set; }
    public int Column { get; set; }
    public SeatType SeatType { get; set; }

    public virtual Bus Bus { get; set; } = null!;
    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}

public enum SeatType
{
    Regular,
    Premium
}
