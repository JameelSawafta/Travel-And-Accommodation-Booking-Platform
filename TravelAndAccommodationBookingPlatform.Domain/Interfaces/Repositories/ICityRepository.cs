using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Repositories;

public interface ICityRepository
{
    Task<List<City>> GetTrendingDestinationsAsync(int count);
    Task<(IEnumerable<City> Items, int TotalCount)> GetAllCitiesAsync(int pageNumber, int pageSize);
    Task<City> GetCityByNameAsync(string cityName);
    Task CreateCityAsync(City city);
}