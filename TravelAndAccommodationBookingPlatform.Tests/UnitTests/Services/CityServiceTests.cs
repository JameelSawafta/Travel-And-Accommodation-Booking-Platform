using AutoMapper;
using Moq;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Exceptions;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Models.CityDtos;
using TravelAndAccommodationBookingPlatform.Domain.Services;

namespace TravelAndAccommodationBookingPlatform.Tests.UnitTests.Services;

public class CityServiceTests
{
    private readonly Mock<ICityRepository> _mockCityRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly CityService _cityService;

    public CityServiceTests()
    {
        _mockCityRepository = new Mock<ICityRepository>();
        _mockMapper = new Mock<IMapper>();
        _cityService = new CityService(_mockCityRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task GetTrendingDestinationsAsync_ShouldThrowNotFoundException_WhenNoCitiesFound()
    {
        _mockCityRepository.Setup(repo => repo.GetTrendingDestinationsAsync(It.IsAny<int>()))
            .ReturnsAsync(new List<City>());

        await Assert.ThrowsAsync<NotFoundException>(() => _cityService.GetTrendingDestinationsAsync(5));
    }

    [Fact]
    public async Task GetTrendingDestinationsAsync_ShouldReturnTrendingDestinations_WhenCitiesExist()
    {
        var cities = new List<City>
        {
            new City
            {
                CityId = Guid.NewGuid(),
                CityName = "Test City",
                Country = "Test Country",
                Hotels = new List<Hotel>
                {
                    new Hotel
                    {
                        ThumbnailUrl = "https://example.com/test.jpg"
                    }
                }
            }
        };

        var trendingDestinations = new List<TrendingDestinationDto>
        {
            new TrendingDestinationDto
            {
                CityId = Guid.NewGuid(),
                CityName = "Test City",
                Country = "Test Country",
                ThumbnailUrl = "https://example.com/test.jpg"
            }
        };

        _mockCityRepository.Setup(repo => repo.GetTrendingDestinationsAsync(It.IsAny<int>()))
            .ReturnsAsync(cities);
        _mockMapper.Setup(mapper => mapper.Map<List<TrendingDestinationDto>>(It.IsAny<List<City>>()))
            .Returns(trendingDestinations);

        var result = await _cityService.GetTrendingDestinationsAsync(5);

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Test City", result.First().CityName);
    }
}