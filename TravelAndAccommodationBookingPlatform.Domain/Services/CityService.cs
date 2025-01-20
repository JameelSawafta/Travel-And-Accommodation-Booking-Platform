using AutoMapper;
using TravelAndAccommodationBookingPlatform.Domain.Exceptions;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Services;
using TravelAndAccommodationBookingPlatform.Domain.Models.CityDtos;

namespace TravelAndAccommodationBookingPlatform.Domain.Services;

public class CityService : ICityService
{
    private readonly ICityRepository _cityRepository;
    private readonly IMapper _mapper;

    public CityService(ICityRepository cityRepository, IMapper mapper)
    {
        _cityRepository = cityRepository;
        _mapper = mapper;
    }

    public async Task<List<TrendingDestinationDto>> GetTrendingDestinationsAsync(int count)
    {
        var cities = await _cityRepository.GetTrendingDestinationsAsync(count);

        if (!cities.Any())
        {
            throw new NotFoundException("No trending destinations found.");
        }

        var trendingDestinations = _mapper.Map<List<TrendingDestinationDto>>(cities);
        return trendingDestinations;
    }
}