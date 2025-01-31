using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using PaymentGateway;
using PayPal.Api;
using TravelAndAccommodationBookingPlatform.Db.DbContext;
using TravelAndAccommodationBookingPlatform.Db.DbServices;
using TravelAndAccommodationBookingPlatform.Db.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Enums;
using TravelAndAccommodationBookingPlatform.Domain.Exceptions;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Services;
using TravelAndAccommodationBookingPlatform.Domain.Models;
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
        
        var bookingRepository = new BookingRepository(_dbContext);
        var cartRepository = new CartRepository(_dbContext , new PaginationService());
        var userRepository = new UserRepository(_dbContext);
        var roomRepository = new RoomRepository(_dbContext , new PaginationService());
        var mockPaymentGatewayService = new Mock<IPaymentGatewayService>();
        mockPaymentGatewayService
            .Setup(service => service.CreatePaymentAsync(It.IsAny<decimal>(), It.IsAny<string>()))
            .ReturnsAsync(("https://paypal.com/approval-url", Guid.NewGuid().ToString(), PaymentMethod.PayPal));

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Hotel, RecentlyVisitedHotelDto>()
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City.CityName))
                .ForMember(dest => dest.PricePerNight, opt => opt.MapFrom(src => src.Rooms.Min(r => r.PricePerNight)));
        });
        var mapper = mapperConfig.CreateMapper();

        _bookingService = new BookingService(bookingRepository, cartRepository, userRepository, roomRepository, mockPaymentGatewayService.Object, mapper);
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
    
    [Fact]
    public async Task CreateBookingFromCartAsync_ShouldThrowNotFoundException_WhenUserNotFound()
    {
        var request = new CheckoutRequestDto { UserId = Guid.NewGuid() };
        await Assert.ThrowsAsync<NotFoundException>(() => _bookingService.CreateBookingFromCartAsync(request));
    }

    [Fact]
    public async Task CreateBookingFromCartAsync_ShouldThrowNotFoundException_WhenCartIsEmpty()
    {
        var userId = Guid.NewGuid();
        _dbContext.Users.Add(new User
        {
            UserId = userId,
            Email = "test@example.com",
            FirstName = "Test",
            LastName = "User",
            PasswordHash = "hashed_password",
            Salt = "salt_value",
            Username = "testuser"
        });
        await _dbContext.SaveChangesAsync();
        
        var request = new CheckoutRequestDto { UserId = userId };
        await Assert.ThrowsAsync<NotFoundException>(() => _bookingService.CreateBookingFromCartAsync(request));
    }

    [Fact]
    public async Task CreateBookingFromCartAsync_ShouldReturnCheckoutDto_WhenBookingIsSuccessful()
    {
        var userId = Guid.NewGuid();
        var roomId = Guid.NewGuid();

        _dbContext.Users.Add(new User
        {
            UserId = userId,
            Email = "test@example.com",
            FirstName = "Test",
            LastName = "User",
            PasswordHash = "hashed_password",
            Salt = "salt_value",
            Username = "testuser"
        });

        _dbContext.Rooms.Add(new Room
        {
            RoomId = roomId,
            RoomNumber = "101",
            Description = "Standard Room",
            PricePerNight = 100,
            Availability = true,
            BookingDetails = new List<BookingDetail>() 
        });

        await _dbContext.SaveChangesAsync();

        var checkInDate = DateTime.Now.AddDays(1);
        var checkOutDate = DateTime.Now.AddDays(3);

        _dbContext.Carts.Add(new Cart
        {
            UserId = userId,
            RoomId = roomId,
            CheckInDate = checkInDate,
            CheckOutDate = checkOutDate,
            Price = 200 // 100 * 2 nights
        });

        await _dbContext.SaveChangesAsync();

        var request = new CheckoutRequestDto { UserId = userId };
        var result = await _bookingService.CreateBookingFromCartAsync(request);

        Assert.NotNull(result);
        Assert.False(string.IsNullOrEmpty(result.approvalUrl));
        Assert.NotEqual(Guid.Empty, result.PaymentId);
    }
}