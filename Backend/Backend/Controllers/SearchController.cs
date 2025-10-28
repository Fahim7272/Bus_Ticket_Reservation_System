using Application.Contracts.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SearchController : ControllerBase
{
    private readonly ISearchService _searchService;

    public SearchController(ISearchService searchService)
    {
        _searchService = searchService;
    }

    [HttpGet("available-buses")]
    public async Task<IActionResult> SearchAvailableBuses(
        [FromQuery] string from,
        [FromQuery] string to,
        [FromQuery] DateTime journeyDate)
    {
        if (string.IsNullOrWhiteSpace(from) || string.IsNullOrWhiteSpace(to))
            return BadRequest("From and To cities are required");

        var results = await _searchService.SearchAvailableBusesAsync(from, to, journeyDate);
        return Ok(results);
    }
}
