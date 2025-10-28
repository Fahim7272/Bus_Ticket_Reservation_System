using Application.Contracts.DTOs;
using Application.Contracts.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpGet("seat-plan/{busScheduleId}")]
    public async Task<IActionResult> GetSeatPlan(Guid busScheduleId)
    {
        try
        {
            var seatPlan = await _bookingService.GetSeatPlanAsync(busScheduleId);
            return Ok(seatPlan);
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPost("book-seat")]
    public async Task<IActionResult> BookSeat([FromBody] BookSeatInputDto input)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _bookingService.BookSeatAsync(input);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
}
