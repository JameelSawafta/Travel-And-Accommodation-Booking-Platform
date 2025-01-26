using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TravelAndAccommodationBookingPlatform.Db.DbContext;
using TravelAndAccommodationBookingPlatform.Db.DbServices;
using TravelAndAccommodationBookingPlatform.Db.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Exceptions;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Services;
using TravelAndAccommodationBookingPlatform.Domain.Models.CityDtos;
using TravelAndAccommodationBookingPlatform.Domain.Services;

namespace TravelAndAccommodationBookingPlatform.Tests.IntegrationTests.Services;

public class CityServiceIntegrationTests : IDisposable
{
    private readonly CityService _cityService;
    private readonly DbContextOptions<TravelAndAccommodationBookingPlatformDbContext> _dbOptions;
    private readonly TravelAndAccommodationBookingPlatformDbContext _dbContext;
    private readonly IPaginationService _paginationService;

    public CityServiceIntegrationTests()
    {
        _dbOptions = new DbContextOptionsBuilder<TravelAndAccommodationBookingPlatformDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _dbContext = new TravelAndAccommodationBookingPlatformDbContext(_dbOptions);

        _paginationService = new PaginationService();

        var cityRepository = new CityRepository(_dbContext, _paginationService);

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<City, TrendingDestinationDto>()
                .ForMember(dest => dest.ThumbnailUrl,
                    opt => opt.MapFrom(src => src.Hotels.FirstOrDefault().ThumbnailUrl));
            cfg.CreateMap<City, CityDto>();
            cfg.CreateMap<CreateCityDto, City>();
            cfg.CreateMap<UpdateCityDto, City>();
        });
        var mapper = mapperConfig.CreateMapper();

        _cityService = new CityService(cityRepository, mapper);
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    [Fact]
    public async Task GetTrendingDestinationsAsync_ShouldReturnTrendingDestinations_WhenCitiesExist()
    {
        var city = new City
        {
            CityId = Guid.NewGuid(),
            CityName = "Test City",
            Country = "Test Country",
            Hotels = new List<Hotel>
            {
                new Hotel
                {
                    HotelId = Guid.NewGuid(),
                    HotelName = "Test Hotel",
                    Address = "123 Test Street",
                    PhoneNumber = "123-456-7890",
                    ThumbnailUrl = "https://example.com/test.jpg",
                    Rooms = new List<Room>
                    {
                        new Room
                        {
                            RoomId = Guid.NewGuid(),
                            RoomNumber = "101",
                            Description = "Test Room Description",
                            Bookings = new List<Booking>
                            {
                                new Booking
                                {
                                    BookingId = Guid.NewGuid()
                                }
                            }
                        }
                    }
                }
            }
        };
        _dbContext.Cities.Add(city);
        await _dbContext.SaveChangesAsync();

        var result = await _cityService.GetTrendingDestinationsAsync(5);

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Test City", result.First().CityName);
    }

    [Fact]
    public async Task GetTrendingDestinationsAsync_ShouldThrowNotFoundException_WhenNoCitiesExist()
    {
        await Assert.ThrowsAsync<NotFoundException>(() => _cityService.GetTrendingDestinationsAsync(5));
    }

    [Fact]
    public async Task GetAllCitiesAsync_ShouldReturnPaginatedCities()
    {
        var city1 = new City { CityId = Guid.NewGuid(), CityName = "Paris", Country = "France" };
        var city2 = new City { CityId = Guid.NewGuid(), CityName = "London", Country = "UK" };
        _dbContext.Cities.AddRange(city1, city2);
        await _dbContext.SaveChangesAsync();

        var result = await _cityService.GetAllCitiesAsync(pageNumber: 1, pageSize: 10);

        Assert.Equal(2, result.Items.Count);
        Assert.Equal(10, result.PageData.PageSize);
    }

    [Fact]
    public async Task GetCityByNameAsync_ShouldReturnCity_WhenCityExists()
    {
        var city = new City { CityId = Guid.NewGuid(), CityName = "Paris", Country = "France" };
        _dbContext.Cities.Add(city);
        await _dbContext.SaveChangesAsync();

        var result = await _cityService.GetCityByNameAsync("Paris");

        Assert.Equal("Paris", result.CityName);
    }

    [Fact]
    public async Task GetCityByNameAsync_ShouldThrowNotFoundException_WhenCityDoesNotExist()
    {
        await Assert.ThrowsAsync<NotFoundException>(() =>
            _cityService.GetCityByNameAsync("Unknown"));
    }

    [Fact]
    public async Task GetCityByIdAsync_ShouldReturnCity_WhenCityExists()
    {
        var cityId = Guid.NewGuid();
        var city = new City { CityId = cityId, CityName = "Paris", Country = "France" };
        _dbContext.Cities.Add(city);
        await _dbContext.SaveChangesAsync();

        var result = await _cityService.GetCityByIdAsync(cityId);

        Assert.Equal(cityId, result.CityId);
    }

    [Fact]
    public async Task GetCityByIdAsync_ShouldThrowNotFoundException_WhenCityDoesNotExist()
    {
        await Assert.ThrowsAsync<NotFoundException>(() =>
            _cityService.GetCityByIdAsync(Guid.NewGuid()));
    }

    [Fact]
    public async Task CreateCityAsync_ShouldCreateCity_WhenCityDoesNotExist()
    {
        var dto = new CreateCityDto { CityName = "Paris", Country = "France" };

        await _cityService.CreateCityAsync(dto);

        var createdCity = await _dbContext.Cities.FirstOrDefaultAsync(c => c.CityName == "Paris");
        Assert.NotNull(createdCity);
    }

    [Fact]
    public async Task CreateCityAsync_ShouldThrowConflictException_WhenCityExists()
    {
        var existingCity = new City { CityId = Guid.NewGuid(), CityName = "Paris", Country = "France" };
        _dbContext.Cities.Add(existingCity);
        await _dbContext.SaveChangesAsync();

        var dto = new CreateCityDto { CityName = "Paris", Country = "France" };

        await Assert.ThrowsAsync<ConflictException>(() =>
            _cityService.CreateCityAsync(dto));
    }

    [Fact]
    public async Task UpdateCityAsync_ShouldUpdateCity_WhenNoConflict()
    {
        var cityId = Guid.NewGuid();
        var existingCity = new City { CityId = cityId, CityName = "OldName", Country = "OldCountry" };
        _dbContext.Cities.Add(existingCity);
        await _dbContext.SaveChangesAsync();

        var dto = new UpdateCityDto { CityName = "NewName" };

        await _cityService.UpdateCityAsync(cityId, dto);

        var updatedCity = await _dbContext.Cities.FindAsync(cityId);
        Assert.Equal("NewName", updatedCity.CityName);
    }

    [Fact]
    public async Task UpdateCityAsync_ShouldThrowConflictException_WhenNameExists()
    {
        var city1 = new City { CityId = Guid.NewGuid(), CityName = "Paris", Country = "France" };
        var city2 = new City { CityId = Guid.NewGuid(), CityName = "London", Country = "UK" };
        _dbContext.Cities.AddRange(city1, city2);
        await _dbContext.SaveChangesAsync();

        var dto = new UpdateCityDto { CityName = "Paris" }; // Try to rename London to Paris

        await Assert.ThrowsAsync<ConflictException>(() =>
            _cityService.UpdateCityAsync(city2.CityId, dto));
    }

    [Fact]
    public async Task DeleteCityAsync_ShouldDeleteCity_WhenCityExists()
    {
        var cityId = Guid.NewGuid();
        var city = new City { CityId = cityId, CityName = "Paris", Country = "France" };
        _dbContext.Cities.Add(city);
        await _dbContext.SaveChangesAsync();

        await _cityService.DeleteCityAsync(cityId);

        var deletedCity = await _dbContext.Cities.FindAsync(cityId);
        Assert.Null(deletedCity);
    }
}