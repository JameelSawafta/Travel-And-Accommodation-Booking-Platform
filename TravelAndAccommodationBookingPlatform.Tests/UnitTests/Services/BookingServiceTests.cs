using AutoMapper;
using Moq;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Exceptions;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Services;
using TravelAndAccommodationBookingPlatform.Domain.Models;
using TravelAndAccommodationBookingPlatform.Domain.Models.HotelDtos;
using TravelAndAccommodationBookingPlatform.Domain.Services;

namespace TravelAndAccommodationBookingPlatform.Tests.UnitTests.Services;

public class BookingServiceTests
{
    private readonly Mock<IBookingRepository> _mockBookingRepository;
    private readonly Mock<ICartRepository> _mockCartRepository;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IRoomRepository> _mockRoomRepository;
    private readonly Mock<IPaymentGatewayService> _mockPaymentGatewayService;
    private readonly Mock<IMapper> _mockMapper;
    private readonly BookingService _bookingService;

    public BookingServiceTests()
    {
        _mockBookingRepository = new Mock<IBookingRepository>();
        _mockCartRepository = new Mock<ICartRepository>();
        _mockUserRepository = new Mock<IUserRepository>();
        _mockRoomRepository = new Mock<IRoomRepository>();
        _mockPaymentGatewayService = new Mock<IPaymentGatewayService>();
        _mockMapper = new Mock<IMapper>();

        _bookingService = new BookingService(
            _mockBookingRepository.Object,
            _mockCartRepository.Object,
            _mockUserRepository.Object,
            _mockRoomRepository.Object,
            _mockPaymentGatewayService.Object,
            _mockMapper.Object
        );
    }

    [Fact]
    public async Task GetRecentlyVisitedHotelsAsync_ShouldThrowNotFoundException_WhenNoHotelsFound()
    {
        _mockBookingRepository.Setup(repo => repo.GetRecentlyVisitedHotelsAsync(It.IsAny<Guid>(), It.IsAny<int>()))
            .ReturnsAsync(new List<Hotel>());

        await Assert.ThrowsAsync<NotFoundException>(() => _bookingService.GetRecentlyVisitedHotelsAsync(Guid.NewGuid(), 5));
    }

    [Fact]
    public async Task GetRecentlyVisitedHotelsAsync_ShouldReturnRecentlyVisitedHotels_WhenHotelsExist()
    {
        var hotels = new List<Hotel>
        {
            new Hotel
            {
                HotelId = Guid.NewGuid(),
                HotelName = "Test Hotel",
                City = new City { CityName = "Test City" },
                Rooms = new List<Room> { new Room { PricePerNight = 100 } }
            }
        };

        var recentlyVisitedHotels = new List<RecentlyVisitedHotelDto>
        {
            new RecentlyVisitedHotelDto
            {
                HotelId = hotels.First().HotelId,
                HotelName = "Test Hotel",
                City = "Test City",
                PricePerNight = 100
            }
        };

        _mockBookingRepository.Setup(repo => repo.GetRecentlyVisitedHotelsAsync(It.IsAny<Guid>(), It.IsAny<int>()))
            .ReturnsAsync(hotels);
        _mockMapper.Setup(mapper => mapper.Map<List<RecentlyVisitedHotelDto>>(hotels))
            .Returns(recentlyVisitedHotels);

        var result = await _bookingService.GetRecentlyVisitedHotelsAsync(Guid.NewGuid(), 5);

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Test Hotel", result.First().HotelName);
    }
    
    [Fact]
    public async Task CreateBookingFromCartAsync_ShouldThrowNotFoundException_WhenUserNotFound()
    {
        var requestDto = new CheckoutRequestDto { UserId = Guid.NewGuid() };
        _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(requestDto.UserId)).ReturnsAsync((User)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _bookingService.CreateBookingFromCartAsync(requestDto));
    }

    [Fact]
    public async Task CreateBookingFromCartAsync_ShouldThrowNotFoundException_WhenCartIsEmpty()
    {
        var userId = Guid.NewGuid();
        var requestDto = new CheckoutRequestDto { UserId = userId };
        _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(new User());
        _mockCartRepository.Setup(repo => repo.GetCartItemsByUserIdAsync(userId)).ReturnsAsync(new List<Cart>());

        await Assert.ThrowsAsync<NotFoundException>(() => _bookingService.CreateBookingFromCartAsync(requestDto));
    }

    [Fact]
    public async Task CreateBookingFromCartAsync_ShouldReturnCheckoutDto_WhenBookingIsSuccessful()
    {
        var userId = Guid.NewGuid();
        var requestDto = new CheckoutRequestDto { UserId = userId };
        var cartItems = new List<Cart>
        {
            new Cart { RoomId = Guid.NewGuid(), CheckInDate = DateTime.Today, CheckOutDate = DateTime.Today.AddDays(2), Price = 200 }
        };

        _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(new User());
        _mockCartRepository.Setup(repo => repo.GetCartItemsByUserIdAsync(userId)).ReturnsAsync(cartItems);
        _mockRoomRepository.Setup(repo => repo.GetRoomIfAvailableAsync(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(new Room());
        _mockPaymentGatewayService.Setup(service => service.CreatePaymentAsync(It.IsAny<decimal>(), "USD"))
            .ReturnsAsync(("https://payment-approval-url", "transaction-123"));

        var result = await _bookingService.CreateBookingFromCartAsync(requestDto);

        Assert.NotNull(result);
        Assert.Equal("https://payment-approval-url", result.approvalUrl);
    }
}
