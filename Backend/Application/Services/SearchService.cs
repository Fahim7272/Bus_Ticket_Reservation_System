using Application.Contracts.DTOs;
using Application.Contracts.Repositories;
using Application.Contracts.Services;
using Domain.Entities;

namespace Application.Services;

public class SearchService : ISearchService
{
    private readonly IBusScheduleRepository _busScheduleRepository;

    public SearchService(IBusScheduleRepository busScheduleRepository)
    {
        _busScheduleRepository = busScheduleRepository;
    }

    public async Task<List<AvailableBusDto>> SearchAvailableBusesAsync(string from, string to, DateTime journeyDate)
    {
        var schedules = await _busScheduleRepository.SearchAvailableAsync(from, to, journeyDate);

        var results = schedules.Select(schedule =>
        {
            var totalSeats = schedule.Bus.TotalSeats;
            var bookedSeats = schedule.Tickets.Count(t => t.Status == SeatStatus.Booked || t.Status == SeatStatus.Sold);
            var seatsLeft = totalSeats - bookedSeats;

            return new AvailableBusDto
            {
                BusScheduleId = schedule.Id,
                CompanyName = schedule.Bus.CompanyName,
                BusName = schedule.Bus.BusName,
                BusType = schedule.Bus.BusType.ToString(),
                StartTime = schedule.StartTime.ToString(@"hh\:mm"),
                ArrivalTime = schedule.ArrivalTime.ToString(@"hh\:mm"),
                SeatsLeft = seatsLeft,
                TotalSeats = totalSeats,
                Price = schedule.Price
            };
        }).ToList();

        return results;
    }
}
