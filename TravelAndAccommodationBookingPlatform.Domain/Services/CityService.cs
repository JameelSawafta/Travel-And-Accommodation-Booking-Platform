using AutoMapper;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Exceptions;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Services;
using TravelAndAccommodationBookingPlatform.Domain.Models.CityDtos;
using TravelAndAccommodationBookingPlatform.Domain.Models.Common;

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

    public async Task<PaginatedList<CityDto>> GetAllCitiesAsync(int pageNumber, int pageSize)
    {
        var (cities, totalCount) = await _cityRepository.GetAllCitiesAsync(pageNumber, pageSize);
        var pageData = new PageData(totalCount, pageSize, pageNumber);
        var cityDtos = _mapper.Map<IEnumerable<CityDto>>(cities);
        return new PaginatedList<CityDto>(cityDtos.ToList(), pageData);
    }
    
    public async Task<CityDto> GetCityByNameAsync(string cityName)
    {
        var city = await _cityRepository.GetCityByNameAsync(cityName);
        if (city == null)
        {
            throw new NotFoundException("City not found.");
        }
        return _mapper.Map<CityDto>(city);
    }
    
    public async Task<CityDto> GetCityByIdAsync(Guid cityId)
    {
        var city = await _cityRepository.GetCityByIdAsync(cityId);
        if (city == null)
        {
            throw new NotFoundException("City not found.");
        }
        return _mapper.Map<CityDto>(city);
    }
    
    public async Task CreateCityAsync(CreateCityDto cityDto)
    {
        var existingCity = await _cityRepository.GetCityByNameAsync(cityDto.CityName);
        if (existingCity != null)
        {
            throw new ConflictException("City already exists.");
        }
        var city = _mapper.Map<City>(cityDto);
        await _cityRepository.CreateCityAsync(city);
    }

    public async Task UpdateCityAsync(Guid cityId,UpdateCityDto cityDto)
    {
        var city = await _cityRepository.GetCityByIdAsync(cityId);
        if (city == null)
        {
            throw new NotFoundException("City not found.");
        }

        if (cityDto.CityName.ToLower() != city.CityName.ToLower())
        {
            var existingCity = await _cityRepository.GetCityByNameAsync(cityDto.CityName);
            if (existingCity != null)
            {
                throw new ConflictException("City already exists.");
            }
        }
        _mapper.Map(cityDto, city);
        await _cityRepository.UpdateCityAsync(city);
    }
    
    public async Task DeleteCityAsync(Guid cityId)
    {
        var city = await _cityRepository.GetCityByIdAsync(cityId);
        if (city == null)
        {
            throw new NotFoundException("City not found.");
        }
        await _cityRepository.DeleteCityAsync(cityId);
    }
}