using TravelAndAccommodationBookingPlatform.Domain.Models.CityDtos;
using TravelAndAccommodationBookingPlatform.Domain.Models.Common;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Services;

public interface ICityService
{
    Task<List<TrendingDestinationDto>> GetTrendingDestinationsAsync(int count);
    Task<PaginatedList<CityDto>> GetAllCitiesAsync(int pageNumber, int pageSize);
    Task<CityDto> GetCityByNameAsync(string cityName);
    Task<CityDto> GetCityByIdAsync(Guid cityId);
    Task CreateCityAsync(CreateCityDto cityDto);
}