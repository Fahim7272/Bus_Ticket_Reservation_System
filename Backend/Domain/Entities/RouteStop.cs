namespace Domain.Entities;

public class RouteStop
{
    public Guid Id { get; set; }
    public Guid RouteId { get; set; }
    public string StopName { get; set; } = string.Empty;
    public int StopOrder { get; set; }
    public TimeSpan ArrivalTime { get; set; }

    public virtual Route Route { get; set; } = null!;
}
