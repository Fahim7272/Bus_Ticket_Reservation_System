namespace Domain.Entities;

public class Route
{
    public Guid Id { get; set; }
    public string FromCity { get; set; } = string.Empty;
    public string ToCity { get; set; } = string.Empty;
    public decimal Distance { get; set; }
    public TimeSpan EstimatedDuration { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual ICollection<RouteStop> RouteStops { get; set; } = new List<RouteStop>();
}
