namespace Application.Contracts.DTOs;

public class BookSeatResultDto
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? BookingReference { get; set; }
    public List<string> BookedSeats { get; set; } = new();
    public decimal TotalAmount { get; set; }
}
