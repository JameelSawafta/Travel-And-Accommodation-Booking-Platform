using TravelAndAccommodationBookingPlatform.Domain.Models.CityDtos;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Services;

public interface ICityService
{
    Task<List<TrendingDestinationDto>> GetTrendingDestinationsAsync(int count);
}