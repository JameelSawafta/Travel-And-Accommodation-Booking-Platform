using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TravelAndAccommodationBookingPlatform.Db.DbContext;
using TravelAndAccommodationBookingPlatform.Db.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Exceptions;
using TravelAndAccommodationBookingPlatform.Domain.Models.CityDtos;
using TravelAndAccommodationBookingPlatform.Domain.Services;

namespace TravelAndAccommodationBookingPlatform.Tests.IntegrationTests.Services;

public class CityServiceIntegrationTests : IDisposable
{
    private readonly CityService _cityService;
    private readonly DbContextOptions<TravelAndAccommodationBookingPlatformDbContext> _dbOptions;
    private readonly TravelAndAccommodationBookingPlatformDbContext _dbContext;

    public CityServiceIntegrationTests()
    {
        _dbOptions = new DbContextOptionsBuilder<TravelAndAccommodationBookingPlatformDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _dbContext = new TravelAndAccommodationBookingPlatformDbContext(_dbOptions);

        var cityRepository = new CityRepository(_dbContext);

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<City, TrendingDestinationDto>()
                .ForMember(dest => dest.ThumbnailUrl, opt => opt.MapFrom(src => src.Hotels.FirstOrDefault().ThumbnailUrl));
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
}