using AutoMapper;
using Moq;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Exceptions;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Models.HotelDtos;
using TravelAndAccommodationBookingPlatform.Domain.Models.RoomDtos;
using TravelAndAccommodationBookingPlatform.Domain.Models.SearchDtos;
using TravelAndAccommodationBookingPlatform.Domain.Services;

namespace TravelAndAccommodationBookingPlatform.Tests.UnitTests.Services;

public class HotelServiceTests
{
    private readonly Mock<IHotelRepository> _mockHotelRepository;
    private readonly Mock<IOwnerRepository> _mockOwnerRepository;
    private readonly Mock<ICityRepository> _mockCityRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly HotelService _hotelService;

    public HotelServiceTests()
    {
        _mockHotelRepository = new Mock<IHotelRepository>();
        _mockOwnerRepository = new Mock<IOwnerRepository>();
        _mockCityRepository = new Mock<ICityRepository>();
        _mockMapper = new Mock<IMapper>();
        _hotelService = new HotelService(
            _mockHotelRepository.Object,
            _mockOwnerRepository.Object,
            _mockCityRepository.Object,
            _mockMapper.Object
        );
    }

    [Fact]
    public async Task SearchHotelsAsync_ShouldReturnPaginatedList_WhenSuccessfulSearch()
    {
        var searchRequest = new SearchRequestDto
        {
            Query = "Test", Adults = 2, Children = 1, Rooms = 1, CheckInDate = DateTime.Now,
            CheckOutDate = DateTime.Now.AddDays(2)
        };
        var hotels = new List<Hotel> { new Hotel { HotelId = Guid.NewGuid(), HotelName = "Test Hotel" } };
        var hotelDtos = new List<HotelSearchResultDto>
            { new HotelSearchResultDto { HotelId = Guid.NewGuid(), HotelName = "Test Hotel" } };

        _mockHotelRepository.Setup(repo =>
                repo.SearchHotelsAsync(It.IsAny<SearchRequestDto>(), It.IsAny<int>(), It.IsAny<int>()))
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
                                Discount = new Discount
                                {
                                    DiscountPercentageValue = 0.1, ValidFrom = DateTime.Now.AddDays(-1),
                                    ValidTo = DateTime.Now.AddDays(1)
                                }
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

    [Fact]
    public async Task GetAllHotelsAsync_ShouldReturnPaginatedList_WhenHotelsExist()
    {
        var hotels = new List<Hotel>
        {
            new Hotel { HotelId = Guid.NewGuid(), HotelName = "Hotel 1" },
            new Hotel { HotelId = Guid.NewGuid(), HotelName = "Hotel 2" }
        };
        var paginatedHotels = (hotels.AsEnumerable(), 2);
        var hotelDtos = hotels.Select(h => new HotelDto { HotelId = h.HotelId, HotelName = h.HotelName }).ToList();

        _mockHotelRepository.Setup(repo => repo.GetAllHotelsAsync(1, 10))
            .ReturnsAsync(paginatedHotels);
        _mockMapper.Setup(mapper => mapper.Map<IEnumerable<HotelDto>>(It.IsAny<IEnumerable<Hotel>>()))
            .Returns(hotelDtos);

        var result = await _hotelService.GetAllHotelsAsync(1, 10);

        Assert.Equal(2, result.Items.Count);
        Assert.Equal(10, result.PageData.PageSize);
    }

    [Fact]
    public async Task GetAllHotelsAsync_ShouldReturnEmptyList_WhenNoHotelsExist()
    {
        var emptyResult = (Enumerable.Empty<Hotel>(), 0);
        _mockHotelRepository.Setup(repo => repo.GetAllHotelsAsync(1, 10))
            .ReturnsAsync(emptyResult);

        var result = await _hotelService.GetAllHotelsAsync(1, 10);

        Assert.Empty(result.Items);
    }

    [Fact]
    public async Task GetHotelByIdAsync_ShouldReturnHotel_WhenHotelExists()
    {
        var hotelId = Guid.NewGuid();
        var hotel = new Hotel { HotelId = hotelId, HotelName = "Test Hotel" };
        var hotelDto = new HotelDto { HotelId = hotelId, HotelName = "Test Hotel" };

        _mockHotelRepository.Setup(repo => repo.GetHotelByIdAsync(hotelId))
            .ReturnsAsync(hotel);
        _mockMapper.Setup(mapper => mapper.Map<HotelDto>(hotel))
            .Returns(hotelDto);

        var result = await _hotelService.GetHotelByIdAsync(hotelId);

        Assert.Equal(hotelId, result.HotelId);
        Assert.Equal("Test Hotel", result.HotelName);
    }

    [Fact]
    public async Task GetHotelByIdAsync_ShouldThrowNotFoundException_WhenHotelDoesNotExist()
    {
        var hotelId = Guid.NewGuid();
        _mockHotelRepository.Setup(repo => repo.GetHotelByIdAsync(hotelId))
            .ReturnsAsync((Hotel)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _hotelService.GetHotelByIdAsync(hotelId));
    }

    [Fact]
    public async Task CreateHotelAsync_ShouldCreateHotel_WhenValidInput()
    {
        var ownerId = Guid.NewGuid();
        var cityId = Guid.NewGuid();
        var dto = new CreateHotelDto { HotelName = "Test Hotel", OwnerId = ownerId, CityId = cityId };
        var hotel = new Hotel { HotelId = Guid.NewGuid(), HotelName = "Test Hotel" };

        _mockOwnerRepository.Setup(repo => repo.GetOwnerByIdAsync(ownerId))
            .ReturnsAsync(new Owner { OwnerId = ownerId });
        _mockCityRepository.Setup(repo => repo.GetCityByIdAsync(cityId))
            .ReturnsAsync(new City { CityId = cityId });
        _mockMapper.Setup(mapper => mapper.Map<Hotel>(dto))
            .Returns(hotel);

        await _hotelService.CreateHotelAsync(dto);

        _mockHotelRepository.Verify(repo => repo.CreateHotelAsync(hotel), Times.Once);
    }

    [Fact]
    public async Task CreateHotelAsync_ShouldThrowNotFoundException_WhenOwnerDoesNotExist()
    {
        var ownerId = Guid.NewGuid();
        var cityId = Guid.NewGuid();
        var dto = new CreateHotelDto { HotelName = "Test Hotel", OwnerId = ownerId, CityId = cityId };

        _mockOwnerRepository.Setup(repo => repo.GetOwnerByIdAsync(ownerId))
            .ReturnsAsync((Owner)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _hotelService.CreateHotelAsync(dto));
    }

    [Fact]
    public async Task CreateHotelAsync_ShouldThrowNotFoundException_WhenCityDoesNotExist()
    {
        var ownerId = Guid.NewGuid();
        var cityId = Guid.NewGuid();
        var dto = new CreateHotelDto { HotelName = "Test Hotel", OwnerId = ownerId, CityId = cityId };

        _mockOwnerRepository.Setup(repo => repo.GetOwnerByIdAsync(ownerId))
            .ReturnsAsync(new Owner { OwnerId = ownerId });
        _mockCityRepository.Setup(repo => repo.GetCityByIdAsync(cityId))
            .ReturnsAsync((City)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _hotelService.CreateHotelAsync(dto));
    }

    [Fact]
    public async Task UpdateHotelAsync_ShouldUpdateHotel_WhenHotelExists()
    {
        var hotelId = Guid.NewGuid();
        var ownerId = Guid.NewGuid();
        var cityId = Guid.NewGuid();

        var existingHotel = new Hotel
        {
            HotelId = hotelId,
            HotelName = "Old Name",
            OwnerId = ownerId,
            CityId = cityId
        };

        var dto = new UpdateHotelDto
        {
            HotelName = "New Name",
            OwnerId = ownerId,
            CityId = cityId
        };

        _mockHotelRepository.Setup(repo => repo.GetHotelByIdAsync(hotelId))
            .ReturnsAsync(existingHotel);

        _mockOwnerRepository.Setup(repo => repo.GetOwnerByIdAsync(ownerId))
            .ReturnsAsync(new Owner { OwnerId = ownerId });

        _mockCityRepository.Setup(repo => repo.GetCityByIdAsync(cityId))
            .ReturnsAsync(new City { CityId = cityId });

        _mockMapper.Setup(mapper => mapper.Map(dto, existingHotel))
            .Callback<UpdateHotelDto, Hotel>((src, dest) =>
            {
                dest.HotelName = src.HotelName;
                dest.OwnerId = src.OwnerId;
                dest.CityId = src.CityId;
            });

        await _hotelService.UpdateHotelAsync(hotelId, dto);

        _mockHotelRepository.Verify(repo => repo.UpdateHotelAsync(existingHotel), Times.Once);
        Assert.Equal("New Name", existingHotel.HotelName);
        Assert.Equal(ownerId, existingHotel.OwnerId);
        Assert.Equal(cityId, existingHotel.CityId);
    }

    [Fact]
    public async Task UpdateHotelAsync_ShouldThrowNotFoundException_WhenHotelDoesNotExist()
    {
        var hotelId = Guid.NewGuid();
        _mockHotelRepository.Setup(repo => repo.GetHotelByIdAsync(hotelId))
            .ReturnsAsync((Hotel)null);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            _hotelService.UpdateHotelAsync(hotelId, new UpdateHotelDto()));
    }

    [Fact]
    public async Task DeleteHotelAsync_ShouldDeleteHotel_WhenHotelExists()
    {
        var hotelId = Guid.NewGuid();
        var hotel = new Hotel { HotelId = hotelId, HotelName = "Test Hotel" };

        _mockHotelRepository.Setup(repo => repo.GetHotelByIdAsync(hotelId))
            .ReturnsAsync(hotel);

        await _hotelService.DeleteHotelAsync(hotelId);

        _mockHotelRepository.Verify(repo => repo.DeleteHotelAsync(hotelId), Times.Once);
    }

    [Fact]
    public async Task GetHotelByIdWithRoomsAsync_ShouldReturnHotelDetailedDto_WhenHotelExists()
    {
        
        var hotelId = Guid.NewGuid();
        var hotel = new Hotel
        {
            HotelId = hotelId,
            HotelName = "Test Hotel",
            Rooms = new List<Room>
            {
                new Room
                {
                    RoomId = Guid.NewGuid(),
                    RoomNumber = "101",
                    Availability = true
                }
            }
        };

        var hotelDetailedDto = new HotelDetailedDto
        {
            HotelId = hotelId,
            HotelName = "Test Hotel",
            Rooms = new List<RoomDetailedDto>
            {
                new RoomDetailedDto
                {
                    RoomId = hotel.Rooms.First().RoomId,
                    RoomNumber = "101",
                    Availability = true
                }
            }
        };

        _mockHotelRepository.Setup(repo => repo.GetHotelByIdWithRoomsAsync(hotelId))
            .ReturnsAsync(hotel);

        _mockMapper.Setup(mapper => mapper.Map<HotelDetailedDto>(hotel))
            .Returns(hotelDetailedDto);

        
        var result = await _hotelService.GetHotelByIdWithRoomsAsync(hotelId);

        
        Assert.NotNull(result);
        Assert.Equal(hotelId, result.HotelId);
        Assert.Equal("Test Hotel", result.HotelName);
        Assert.Single(result.Rooms);
        Assert.Equal("101", result.Rooms.First().RoomNumber);
    }

    [Fact]
    public async Task GetHotelByIdWithRoomsAsync_ShouldThrowNotFoundException_WhenHotelDoesNotExist()
    {
        
        var hotelId = Guid.NewGuid();
        _mockHotelRepository.Setup(repo => repo.GetHotelByIdWithRoomsAsync(hotelId))
            .ReturnsAsync((Hotel)null);

        
        await Assert.ThrowsAsync<NotFoundException>(() => _hotelService.GetHotelByIdWithRoomsAsync(hotelId));
    }
}