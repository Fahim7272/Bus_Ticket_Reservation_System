namespace Application.Contracts.DTOs;

public class AvailableBusDto
{
    public Guid BusScheduleId { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string BusName { get; set; } = string.Empty;
    public string BusType { get; set; } = string.Empty;
    public string StartTime { get; set; } = string.Empty;
    public string ArrivalTime { get; set; } = string.Empty;
    public int SeatsLeft { get; set; }
    public int TotalSeats { get; set; }
    public decimal Price { get; set; }
}
