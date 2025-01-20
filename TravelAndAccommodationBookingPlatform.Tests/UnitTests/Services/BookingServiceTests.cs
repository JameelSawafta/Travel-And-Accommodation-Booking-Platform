using AutoMapper;
using Moq;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Exceptions;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Models.HotelDtos;
using TravelAndAccommodationBookingPlatform.Domain.Services;

namespace TravelAndAccommodationBookingPlatform.Tests.UnitTests.Services;

public class BookingServiceTests
{
    private readonly Mock<IBookingRepository> _mockBookingRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly BookingService _bookingService;

    public BookingServiceTests()
    {
        _mockBookingRepository = new Mock<IBookingRepository>();
        _mockMapper = new Mock<IMapper>();
        _bookingService = new BookingService(_mockBookingRepository.Object, _mockMapper.Object);
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
                Rooms = new List<Room>
                {
                    new Room { PricePerNight = 100 }
                }
            }
        };

        var recentlyVisitedHotels = new List<RecentlyVisitedHotelDto>
        {
            new RecentlyVisitedHotelDto
            {
                HotelId = Guid.NewGuid(),
                HotelName = "Test Hotel",
                City = "Test City",
                PricePerNight = 100
            }
        };

        _mockBookingRepository.Setup(repo => repo.GetRecentlyVisitedHotelsAsync(It.IsAny<Guid>(), It.IsAny<int>()))
            .ReturnsAsync(hotels);
        _mockMapper.Setup(mapper => mapper.Map<List<RecentlyVisitedHotelDto>>(It.IsAny<List<Hotel>>()))
            .Returns(recentlyVisitedHotels);

        var result = await _bookingService.GetRecentlyVisitedHotelsAsync(Guid.NewGuid(), 5);

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Test Hotel", result.First().HotelName);
    }
}