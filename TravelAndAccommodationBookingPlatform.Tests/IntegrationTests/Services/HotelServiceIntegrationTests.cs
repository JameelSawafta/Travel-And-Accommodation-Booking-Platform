using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TravelAndAccommodationBookingPlatform.Db.DbContext;
using TravelAndAccommodationBookingPlatform.Db.DbServices;
using TravelAndAccommodationBookingPlatform.Db.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Exceptions;
using TravelAndAccommodationBookingPlatform.Domain.Models.HotelDtos;
using TravelAndAccommodationBookingPlatform.Domain.Models.RoomDtos;
using TravelAndAccommodationBookingPlatform.Domain.Models.SearchDtos;
using TravelAndAccommodationBookingPlatform.Domain.Services;

namespace TravelAndAccommodationBookingPlatform.Tests.IntegrationTests.Services;

public class HotelServiceIntegrationTests : IDisposable
{
    private readonly HotelService _hotelService;
    private readonly DbContextOptions<TravelAndAccommodationBookingPlatformDbContext> _dbOptions;
    private readonly TravelAndAccommodationBookingPlatformDbContext _dbContext;

    public HotelServiceIntegrationTests()
    {
        _dbOptions = new DbContextOptionsBuilder<TravelAndAccommodationBookingPlatformDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _dbContext = new TravelAndAccommodationBookingPlatformDbContext(_dbOptions);

        var hotelRepository = new HotelRepository(_dbContext, new PaginationService());

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Hotel, HotelSearchResultDto>()
                .ForMember(dest => dest.Rooms, opt => opt.MapFrom(src => src.Rooms)); 

            cfg.CreateMap<Room, RoomDto>();
            cfg.CreateMap<Hotel, FeaturedDealDto>();
            cfg.CreateMap<Hotel, HotelDto>();
            cfg.CreateMap<CreateHotelDto, Hotel>();
            cfg.CreateMap<UpdateHotelDto, Hotel>();
        });
        var mapper = mapperConfig.CreateMapper();

        _hotelService = new HotelService(hotelRepository, mapper);
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    [Fact]
    public async Task SearchHotelsAsync_ShouldReturnPaginatedList_WhenHotelsExist()
    {
        
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
                    AdultsCapacity = 2,
                    ChildrenCapacity = 1,
                    Availability = true,
                    Bookings = new List<Booking>()
                }
            }
        };
        _dbContext.Hotels.Add(hotel);
        await _dbContext.SaveChangesAsync();

        var searchRequest = new SearchRequestDto
        {
            Query = "Test",
            Adults = 2,
            Children = 1,
            Rooms = 1,
            CheckInDate = DateTime.Now,
            CheckOutDate = DateTime.Now.AddDays(2)
        };

        
        var result = await _hotelService.SearchHotelsAsync(searchRequest, 10, 1);

        
        Assert.NotNull(result);
        Assert.Single(result.Items);
        Assert.Equal("Test Hotel", result.Items.First().HotelName);
    }

    [Fact]
    public async Task SearchHotelsAsync_ShouldThrowNotFoundException_WhenNoHotelsExist()
    {
        
        var searchRequest = new SearchRequestDto
        {
            Query = "Nonexistent",
            Adults = 2,
            Children = 1,
            Rooms = 1,
            CheckInDate = DateTime.Now,
            CheckOutDate = DateTime.Now.AddDays(2)
        };

        
        await Assert.ThrowsAsync<NotFoundException>(() => _hotelService.SearchHotelsAsync(searchRequest, 10, 1));
    }

    [Fact]
    public async Task GetFeaturedDealsAsync_ShouldReturnFeaturedDeals_WhenHotelsWithDiscountsExist()
    {
        
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
                    Availability = true,
                    RoomDiscounts = new List<RoomDiscount>
                    {
                        new RoomDiscount
                        {
                            Discount = new Discount
                            {
                                DiscountPercentageValue = 0.1,
                                ValidFrom = DateTime.Now.AddDays(-1),
                                ValidTo = DateTime.Now.AddDays(1)
                            }
                        }
                    }
                }
            }
        };
        _dbContext.Hotels.Add(hotel);
        await _dbContext.SaveChangesAsync();

        
        var result = await _hotelService.GetFeaturedDealsAsync(5);

        
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Test Hotel", result.First().HotelName);
        Assert.Equal(90, result.First().DiscountedPrice); // 100 - 10% discount
    }

    [Fact]
    public async Task GetFeaturedDealsAsync_ShouldThrowNotFoundException_WhenNoFeaturedDealsExist()
    {
        
        await Assert.ThrowsAsync<NotFoundException>(() => _hotelService.GetFeaturedDealsAsync(5));
    }
    
    [Fact]
    public async Task GetAllHotelsAsync_ShouldReturnPaginatedList_WhenHotelsExist()
    {
        
        var hotel1 = new Hotel { HotelId = Guid.NewGuid(), HotelName = "Hotel 1", Address = "123 Test Street", PhoneNumber = "123-456-7890" };
        var hotel2 = new Hotel { HotelId = Guid.NewGuid(), HotelName = "Hotel 2", Address = "456 Test Street", PhoneNumber = "123-456-7890" };
        _dbContext.Hotels.AddRange(hotel1, hotel2);
        await _dbContext.SaveChangesAsync();

        
        var result = await _hotelService.GetAllHotelsAsync(1, 10);

        
        Assert.Equal(2, result.Items.Count);
        Assert.Equal(10, result.PageData.PageSize);
    }

    [Fact]
    public async Task GetAllHotelsAsync_ShouldReturnEmptyList_WhenNoHotelsExist()
    {
        
        var result = await _hotelService.GetAllHotelsAsync(1, 10);

        
        Assert.Empty(result.Items);
    }
    
    [Fact]
    public async Task GetHotelByIdAsync_ShouldReturnHotel_WhenHotelExists()
    {
        
        var hotelId = Guid.NewGuid();
        var hotel = new Hotel { HotelId = hotelId, HotelName = "Test Hotel", Address = "123 Test Street", PhoneNumber = "123-456-7890" };
        _dbContext.Hotels.Add(hotel);
        await _dbContext.SaveChangesAsync();

        
        var result = await _hotelService.GetHotelByIdAsync(hotelId);

        
        Assert.Equal(hotelId, result.HotelId);
        Assert.Equal("Test Hotel", result.HotelName);
    }

    [Fact]
    public async Task GetHotelByIdAsync_ShouldThrowNotFoundException_WhenHotelDoesNotExist()
    {
        
        await Assert.ThrowsAsync<NotFoundException>(() => 
            _hotelService.GetHotelByIdAsync(Guid.NewGuid()));
    }
    
    [Fact]
    public async Task CreateHotelAsync_ShouldCreateHotel_WhenValidInput()
    {
        
        var dto = new CreateHotelDto { HotelName = "Test Hotel", CityId = Guid.NewGuid(), OwnerId = Guid.NewGuid(), PhoneNumber = "123-456-7890", Address = "123 Test Street", Latitude = 0, Longitude = 0 };

        
        await _hotelService.CreateHotelAsync(dto);

        
        var createdHotel = await _dbContext.Hotels.FirstOrDefaultAsync(h => h.HotelName == "Test Hotel");
        Assert.NotNull(createdHotel);
    }
    
    [Fact]
    public async Task UpdateHotelAsync_ShouldUpdateHotel_WhenHotelExists()
    {
        
        var hotelId = Guid.NewGuid();
        var hotel = new Hotel { HotelId = hotelId, HotelName = "Old Name", Address = "123 Test Street", PhoneNumber = "123-456-7890" };
        _dbContext.Hotels.Add(hotel);
        await _dbContext.SaveChangesAsync();

        var dto = new UpdateHotelDto { HotelName = "New Name" };

        
        await _hotelService.UpdateHotelAsync(hotelId, dto);

        
        var updatedHotel = await _dbContext.Hotels.FindAsync(hotelId);
        Assert.Equal("New Name", updatedHotel.HotelName);
    }

    [Fact]
    public async Task UpdateHotelAsync_ShouldThrowNotFoundException_WhenHotelDoesNotExist()
    {
        
        await Assert.ThrowsAsync<NotFoundException>(() => 
            _hotelService.UpdateHotelAsync(Guid.NewGuid(), new UpdateHotelDto()));
    }
    
    [Fact]
    public async Task DeleteHotelAsync_ShouldDeleteHotel_WhenHotelExists()
    {
        
        var hotelId = Guid.NewGuid();
        var hotel = new Hotel { HotelId = hotelId, HotelName = "Test Hotel", Address = "123 Test Street", PhoneNumber = "123-456-7890" };
        _dbContext.Hotels.Add(hotel);
        await _dbContext.SaveChangesAsync();

        
        await _hotelService.DeleteHotelAsync(hotelId);

        
        var deletedHotel = await _dbContext.Hotels.FindAsync(hotelId);
        Assert.Null(deletedHotel);
    }

    [Fact]
    public async Task DeleteHotelAsync_ShouldThrowNotFoundException_WhenHotelDoesNotExist()
    {
        
        await Assert.ThrowsAsync<NotFoundException>(() => 
            _hotelService.DeleteHotelAsync(Guid.NewGuid()));
    }
}