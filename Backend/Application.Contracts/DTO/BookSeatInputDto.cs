namespace Application.Contracts.DTOs;

public class BookSeatInputDto
{
    public Guid BusScheduleId { get; set; }
    public List<Guid> SeatIds { get; set; } = new();
    public string PassengerName { get; set; } = string.Empty;
    public string MobileNumber { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string BoardingPoint { get; set; } = string.Empty;
    public string DroppingPoint { get; set; } = string.Empty;
}
