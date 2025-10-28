using Application.Contracts.DTOs;

namespace Application.Contracts.Services;

public interface ISearchService
{
    Task<List<AvailableBusDto>> SearchAvailableBusesAsync(string from, string to, DateTime journeyDate);
}
