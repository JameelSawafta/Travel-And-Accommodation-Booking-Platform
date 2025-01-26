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
    
    [Fact]
    public async Task GetAllCitiesAsync_ShouldReturnPaginatedList_WhenCitiesExist()
    {
        var cities = new List<City>
        {
            new City { CityId = Guid.NewGuid(), CityName = "City1" },
            new City { CityId = Guid.NewGuid(), CityName = "City2" }
        };
        var paginatedCities = (cities.AsEnumerable(), 2);
    
        _mockCityRepository.Setup(repo => repo.GetAllCitiesAsync(1, 10))
            .ReturnsAsync(paginatedCities);
        _mockMapper.Setup(m => m.Map<IEnumerable<CityDto>>(It.IsAny<IEnumerable<City>>()))
            .Returns(cities.Select(c => new CityDto { CityId = c.CityId, CityName = c.CityName }));

        var result = await _cityService.GetAllCitiesAsync(1, 10);

        Assert.Equal(2, result.Items.Count);
        Assert.Equal(10, result.PageData.PageSize);
    }

    [Fact]
    public async Task GetAllCitiesAsync_ShouldReturnEmptyList_WhenNoCitiesExist()
    {
        var emptyResult = (Enumerable.Empty<City>(), 0);
        _mockCityRepository.Setup(repo => repo.GetAllCitiesAsync(1, 10))
            .ReturnsAsync(emptyResult);

        var result = await _cityService.GetAllCitiesAsync(1, 10);

        Assert.Empty(result.Items);
    }
    
    [Fact]
    public async Task GetCityByNameAsync_ShouldReturnCity_WhenCityExists()
    {
        var city = new City { CityName = "Paris" };
        _mockCityRepository.Setup(repo => repo.GetCityByNameAsync("Paris"))
            .ReturnsAsync(city);
        _mockMapper.Setup(m => m.Map<CityDto>(city))
            .Returns(new CityDto { CityName = "Paris" });

        var result = await _cityService.GetCityByNameAsync("Paris");

        Assert.Equal("Paris", result.CityName);
    }

    [Fact]
    public async Task GetCityByNameAsync_ShouldThrowNotFoundException_WhenCityDoesNotExist()
    {
        _mockCityRepository.Setup(repo => repo.GetCityByNameAsync("Unknown"))
            .ReturnsAsync((City)null);

        await Assert.ThrowsAsync<NotFoundException>(() => 
            _cityService.GetCityByNameAsync("Unknown"));
    }
    
    [Fact]
    public async Task GetCityByIdAsync_ShouldReturnCity_WhenCityExists()
    {
        var cityId = Guid.NewGuid();
        var city = new City { CityId = cityId };
        _mockCityRepository.Setup(repo => repo.GetCityByIdAsync(cityId))
            .ReturnsAsync(city);
        _mockMapper.Setup(m => m.Map<CityDto>(city))
            .Returns(new CityDto { CityId = cityId });

        var result = await _cityService.GetCityByIdAsync(cityId);

        Assert.Equal(cityId, result.CityId);
    }

    [Fact]
    public async Task GetCityByIdAsync_ShouldThrowNotFoundException_WhenCityDoesNotExist()
    {
        var cityId = Guid.NewGuid();
        _mockCityRepository.Setup(repo => repo.GetCityByIdAsync(cityId))
            .ReturnsAsync((City)null);

        await Assert.ThrowsAsync<NotFoundException>(() => 
            _cityService.GetCityByIdAsync(cityId));
    }
    
    [Fact]
    public async Task CreateCityAsync_ShouldCreateCity_WhenCityDoesNotExist()
    {
        var dto = new CreateCityDto { CityName = "London" };
        _mockCityRepository.Setup(repo => repo.GetCityByNameAsync("London"))
            .ReturnsAsync((City)null);
        _mockMapper.Setup(m => m.Map<City>(dto))
            .Returns(new City { CityName = "London" });

        await _cityService.CreateCityAsync(dto);

        _mockCityRepository.Verify(repo => repo.CreateCityAsync(It.IsAny<City>()), Times.Once);
    }

    [Fact]
    public async Task CreateCityAsync_ShouldThrowConflictException_WhenCityExists()
    {
        var dto = new CreateCityDto { CityName = "London" };
        _mockCityRepository.Setup(repo => repo.GetCityByNameAsync("London"))
            .ReturnsAsync(new City { CityName = "London" });

        await Assert.ThrowsAsync<ConflictException>(() => 
            _cityService.CreateCityAsync(dto));
    }
    
    [Fact]
    public async Task UpdateCityAsync_ShouldUpdateCity_WhenCityExistsAndNoConflict()
    {
        var cityId = Guid.NewGuid();
        var existingCity = new City { CityId = cityId, CityName = "OldName" };
        var dto = new UpdateCityDto { CityName = "NewName" };
    
        _mockCityRepository.Setup(repo => repo.GetCityByIdAsync(cityId))
            .ReturnsAsync(existingCity);
        _mockCityRepository.Setup(repo => repo.GetCityByNameAsync("NewName"))
            .ReturnsAsync((City)null);

        await _cityService.UpdateCityAsync(cityId, dto);

        _mockCityRepository.Verify(repo => repo.UpdateCityAsync(It.IsAny<City>()), Times.Once);
    }

    [Fact]
    public async Task UpdateCityAsync_ShouldThrowNotFoundException_WhenCityDoesNotExist()
    {
        var cityId = Guid.NewGuid();
        _mockCityRepository.Setup(repo => repo.GetCityByIdAsync(cityId))
            .ReturnsAsync((City)null);

        await Assert.ThrowsAsync<NotFoundException>(() => 
            _cityService.UpdateCityAsync(cityId, new UpdateCityDto()));
    }

    [Fact]
    public async Task UpdateCityAsync_ShouldThrowConflictException_WhenNewNameExists()
    {
        var cityId = Guid.NewGuid();
        var existingCity = new City { CityId = cityId, CityName = "OldName" };
        var dto = new UpdateCityDto { CityName = "NewName" };
    
        _mockCityRepository.Setup(repo => repo.GetCityByIdAsync(cityId))
            .ReturnsAsync(existingCity);
        _mockCityRepository.Setup(repo => repo.GetCityByNameAsync("NewName"))
            .ReturnsAsync(new City());

        await Assert.ThrowsAsync<ConflictException>(() => 
            _cityService.UpdateCityAsync(cityId, dto));
    }
    
    [Fact]
    public async Task DeleteCityAsync_ShouldDeleteCity_WhenCityExists()
    {
        var cityId = Guid.NewGuid();
        _mockCityRepository.Setup(repo => repo.GetCityByIdAsync(cityId))
            .ReturnsAsync(new City { CityId = cityId });

        await _cityService.DeleteCityAsync(cityId);

        _mockCityRepository.Verify(repo => repo.DeleteCityAsync(cityId), Times.Once);
    }

}