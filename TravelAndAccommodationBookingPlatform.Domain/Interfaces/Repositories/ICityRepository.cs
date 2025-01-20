using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Repositories;

public interface ICityRepository
{
    Task<List<City>> GetTrendingDestinationsAsync(int count);
}