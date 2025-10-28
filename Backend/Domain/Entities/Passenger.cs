namespace Domain.Entities;

public class Passenger
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string MobileNumber { get; set; } = string.Empty;
    public string? Email { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
