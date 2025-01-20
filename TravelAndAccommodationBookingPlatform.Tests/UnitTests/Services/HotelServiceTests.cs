using AutoMapper;
using Moq;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Exceptions;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Models.HotelDtos;
using TravelAndAccommodationBookingPlatform.Domain.Models.SearchDtos;
using TravelAndAccommodationBookingPlatform.Domain.Services;

namespace TravelAndAccommodationBookingPlatform.Tests.UnitTests.Services;

public class HotelServiceTests
{
    private readonly Mock<IHotelRepository> _mockHotelRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly HotelService _hotelService;

    public HotelServiceTests()
    {
        _mockHotelRepository = new Mock<IHotelRepository>();
        _mockMapper = new Mock<IMapper>();
        _hotelService = new HotelService(_mockHotelRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task SearchHotelsAsync_ShouldThrowNotFoundException_WhenNoHotelsFound()
    {
        var searchRequest = new SearchRequestDto { Query = "Test", Adults = 2, Children = 1, Rooms = 1, CheckInDate = DateTime.Now, CheckOutDate = DateTime.Now.AddDays(2) };
        _mockHotelRepository.Setup(repo => repo.SearchHotelsAsync(It.IsAny<SearchRequestDto>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync((new List<Hotel>(), 0));

        await Assert.ThrowsAsync<NotFoundException>(() => _hotelService.SearchHotelsAsync(searchRequest, 10, 1));
    }

    [Fact]
    public async Task SearchHotelsAsync_ShouldThrowNotFoundException_WhenInvalidPageNumber()
    {
        var searchRequest = new SearchRequestDto { Query = "Test", Adults = 2, Children = 1, Rooms = 1, CheckInDate = DateTime.Now, CheckOutDate = DateTime.Now.AddDays(2) };
        _mockHotelRepository.Setup(repo => repo.SearchHotelsAsync(It.IsAny<SearchRequestDto>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync((new List<Hotel> { new Hotel() }, 1));

        await Assert.ThrowsAsync<NotFoundException>(() => _hotelService.SearchHotelsAsync(searchRequest, 10, 2));
    }

    [Fact]
    public async Task SearchHotelsAsync_ShouldReturnPaginatedList_WhenSuccessfulSearch()
    {
        var searchRequest = new SearchRequestDto { Query = "Test", Adults = 2, Children = 1, Rooms = 1, CheckInDate = DateTime.Now, CheckOutDate = DateTime.Now.AddDays(2) };
        var hotels = new List<Hotel> { new Hotel { HotelId = Guid.NewGuid(), HotelName = "Test Hotel" } };
        var hotelDtos = new List<HotelSearchResultDto> { new HotelSearchResultDto { HotelId = Guid.NewGuid(), HotelName = "Test Hotel" } };

        _mockHotelRepository.Setup(repo => repo.SearchHotelsAsync(It.IsAny<SearchRequestDto>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync((hotels, 1));
        _mockMapper.Setup(mapper => mapper.Map<IEnumerable<HotelSearchResultDto>>(It.IsAny<IEnumerable<Hotel>>()))
            .Returns(hotelDtos);

        var result = await _hotelService.SearchHotelsAsync(searchRequest, 10, 1);

        Assert.NotNull(result);
        Assert.Single(result.Items);
        Assert.Equal("Test Hotel", result.Items.First().HotelName);
    }

    [Fact]
    public async Task GetFeaturedDealsAsync_ShouldThrowNotFoundException_WhenNoFeaturedDealsFound()
    {
        _mockHotelRepository.Setup(repo => repo.GetFeaturedDealsAsync(It.IsAny<int>()))
            .ReturnsAsync(new List<Hotel>());

        await Assert.ThrowsAsync<NotFoundException>(() => _hotelService.GetFeaturedDealsAsync(5));
    }

    [Fact]
    public async Task GetFeaturedDealsAsync_ShouldReturnFeaturedDeals_WhenSuccessfulFeaturedDeals()
    {
        var hotels = new List<Hotel>
        {
            new Hotel
            {
                HotelId = Guid.NewGuid(),
                HotelName = "Test Hotel",
                Rooms = new List<Room>
                {
                    new Room
                    {
                        RoomId = Guid.NewGuid(),
                        PricePerNight = 100,
                        RoomDiscounts = new List<RoomDiscount>
                        {
                            new RoomDiscount
                            {
                                Discount = new Discount { DiscountPercentageValue = 0.1, ValidFrom = DateTime.Now.AddDays(-1), ValidTo = DateTime.Now.AddDays(1) }
                            }
                        }
                    }
                }
            }
        };

        _mockHotelRepository.Setup(repo => repo.GetFeaturedDealsAsync(It.IsAny<int>()))
            .ReturnsAsync(hotels);
        _mockMapper.Setup(mapper => mapper.Map<FeaturedDealDto>(It.IsAny<Hotel>()))
            .Returns(new FeaturedDealDto { HotelId = Guid.NewGuid(), HotelName = "Test Hotel" });

        var result = await _hotelService.GetFeaturedDealsAsync(5);

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Test Hotel", result.First().HotelName);
        Assert.Equal(90, result.First().DiscountedPrice);
    }
}