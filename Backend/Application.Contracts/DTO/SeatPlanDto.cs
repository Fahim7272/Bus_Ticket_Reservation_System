namespace Application.Contracts.DTOs;

public class SeatPlanDto
{
    public Guid BusScheduleId { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string BusName { get; set; } = string.Empty;
    public string FromCity { get; set; } = string.Empty;
    public string ToCity { get; set; } = string.Empty;
    public DateTime JourneyDate { get; set; }
    public string StartTime { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public List<SeatDto> Seats { get; set; } = new();
    public List<string> BoardingPoints { get; set; } = new();
    public List<string> DroppingPoints { get; set; } = new();
}

public class SeatDto
{
    public Guid SeatId { get; set; }
    public string SeatNumber { get; set; } = string.Empty;
    public int Row { get; set; }
    public int Column { get; set; }
    public string SeatType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}
