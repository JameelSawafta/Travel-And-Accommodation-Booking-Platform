using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TravelAndAccommodationBookingPlatform.Db.DbContext;
using TravelAndAccommodationBookingPlatform.Db.DbServices;
using TravelAndAccommodationBookingPlatform.Db.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Enums;
using TravelAndAccommodationBookingPlatform.Domain.Exceptions;
using TravelAndAccommodationBookingPlatform.Domain.Models.HotelDtos;
using TravelAndAccommodationBookingPlatform.Domain.Services;

namespace TravelAndAccommodationBookingPlatform.Tests.IntegrationTests.Services;

public class BookingServiceIntegrationTests : IDisposable
{
    private readonly BookingService _bookingService;
    private readonly DbContextOptions<TravelAndAccommodationBookingPlatformDbContext> _dbOptions;
    private readonly TravelAndAccommodationBookingPlatformDbContext _dbContext;

    public BookingServiceIntegrationTests()
    {
        _dbOptions = new DbContextOptionsBuilder<TravelAndAccommodationBookingPlatformDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _dbContext = new TravelAndAccommodationBookingPlatformDbContext(_dbOptions);

        var bookingRepository = new BookingRepository(_dbContext, new PaginationService());

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Hotel, RecentlyVisitedHotelDto>()
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City.CityName))
                .ForMember(dest => dest.PricePerNight, opt => opt.MapFrom(src => src.Rooms.Min(r => r.PricePerNight)));
        });
        var mapper = mapperConfig.CreateMapper();

        _bookingService = new BookingService(bookingRepository, mapper);
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    [Fact]
    public async Task GetRecentlyVisitedHotelsAsync_ShouldReturnRecentlyVisitedHotels_WhenHotelsExist()
    {
        var userId = Guid.NewGuid();
        var city = new City { CityId = Guid.NewGuid(), CityName = "Test City", Country = "Test Country" };
        var hotel = new Hotel
        {
            HotelId = Guid.NewGuid(),
            HotelName = "Test Hotel",
            Address = "123 Test Street",
            PhoneNumber = "123-456-7890",
            City = city,
            Rooms = new List<Room>
            {
                new Room
                {
                    RoomId = Guid.NewGuid(),
                    RoomNumber = "101",
                    Description = "Test Room Description",
                    PricePerNight = 100,
                    BookingDetails = new List<BookingDetail>
                    {
                        new BookingDetail
                        {
                            Booking = new Booking
                            {
                                UserId = userId,
                                Status = BookingStatus.Confirmed
                            },
                            CheckOutDate = DateTime.Now.AddDays(-1) 
                        }
                    }
                }
            }
        };

        _dbContext.Hotels.Add(hotel);
        await _dbContext.SaveChangesAsync();

        var result = await _bookingService.GetRecentlyVisitedHotelsAsync(userId, 5);

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Test Hotel", result.First().HotelName);
        Assert.Equal("Test City", result.First().City); 
        Assert.Equal(100, result.First().PricePerNight); 
    }

    [Fact]
    public async Task GetRecentlyVisitedHotelsAsync_ShouldThrowNotFoundException_WhenNoHotelsExist()
    {
        var userId = Guid.NewGuid();

        await Assert.ThrowsAsync<NotFoundException>(() => _bookingService.GetRecentlyVisitedHotelsAsync(userId, 5));
    }
}