using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TravelAndAccommodationBookingPlatform.Db.DbContext;
using TravelAndAccommodationBookingPlatform.Db.DbServices;
using TravelAndAccommodationBookingPlatform.Db.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Enums;
using TravelAndAccommodationBookingPlatform.Domain.Exceptions;
using TravelAndAccommodationBookingPlatform.Domain.Models.RoomDtos;
using TravelAndAccommodationBookingPlatform.Domain.Services;

namespace TravelAndAccommodationBookingPlatform.Tests.IntegrationTests.Services;

public class RoomServiceIntegrationTests : IDisposable
{
    private readonly RoomService _roomService;
    private readonly DbContextOptions<TravelAndAccommodationBookingPlatformDbContext> _dbOptions;
    private readonly TravelAndAccommodationBookingPlatformDbContext _dbContext;

    public RoomServiceIntegrationTests()
    {
        _dbOptions = new DbContextOptionsBuilder<TravelAndAccommodationBookingPlatformDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _dbContext = new TravelAndAccommodationBookingPlatformDbContext(_dbOptions);

        var roomRepository = new RoomRepository(_dbContext, new PaginationService());
        var hotelRepository = new HotelRepository(_dbContext , new PaginationService());

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Room, RoomDto>();
            cfg.CreateMap<CreateRoomDto, Room>();
            cfg.CreateMap<UpdateRoomDto, Room>();
        });
        var mapper = mapperConfig.CreateMapper();

        _roomService = new RoomService(roomRepository, hotelRepository, mapper);
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    [Fact]
    public async Task GetAllRoomsAsync_ShouldReturnPaginatedList_WhenRoomsExist()
    {
        
        var room1 = new Room 
        { 
            RoomId = Guid.NewGuid(), 
            RoomNumber = "101", 
            Description = "Test Room 1", 
            PricePerNight = 100, 
            RoomType = RoomType.Single, 
            AdultsCapacity = 2, 
            ChildrenCapacity = 1, 
            Availability = true 
        };
        var room2 = new Room 
        { 
            RoomId = Guid.NewGuid(), 
            RoomNumber = "102", 
            Description = "Test Room 2", 
            PricePerNight = 150, 
            RoomType = RoomType.Double, 
            AdultsCapacity = 3, 
            ChildrenCapacity = 2, 
            Availability = true 
        };
        _dbContext.Rooms.AddRange(room1, room2);
        await _dbContext.SaveChangesAsync();

        
        var result = await _roomService.GetAllRoomsAsync(1, 10);

        
        Assert.Equal(2, result.Items.Count);
        Assert.Equal(10, result.PageData.PageSize);
    }

    [Fact]
    public async Task GetAllRoomsAsync_ShouldReturnEmptyList_WhenNoRoomsExist()
    {
        
        var result = await _roomService.GetAllRoomsAsync(1, 10);

        
        Assert.Empty(result.Items);
    }

    [Fact]
    public async Task GetRoomByIdAsync_ShouldReturnRoom_WhenRoomExists()
    {
        
        var roomId = Guid.NewGuid();
        var room = new Room 
        { 
            RoomId = roomId, 
            RoomNumber = "101", 
            Description = "Test Room", 
            PricePerNight = 100, 
            RoomType = RoomType.Single, 
            AdultsCapacity = 2, 
            ChildrenCapacity = 1, 
            Availability = true 
        };
        _dbContext.Rooms.Add(room);
        await _dbContext.SaveChangesAsync();

        
        var result = await _roomService.GetRoomByIdAsync(roomId);

        Assert.Equal(roomId, result.RoomId);
        Assert.Equal("101", result.RoomNumber);
        Assert.Equal("Test Room", result.Description);
        Assert.Equal(100, result.PricePerNight);
        Assert.Equal(2, result.AdultsCapacity);
        Assert.Equal(1, result.ChildrenCapacity);
        Assert.True(result.Availability);
    }

    [Fact]
    public async Task GetRoomByIdAsync_ShouldThrowNotFoundException_WhenRoomDoesNotExist()
    {
        
        await Assert.ThrowsAsync<NotFoundException>(() => 
            _roomService.GetRoomByIdAsync(Guid.NewGuid()));
    }

    [Fact]
    public async Task CreateRoomAsync_ShouldCreateRoom_WhenValidInput()
    {
        
        var hotel = new Hotel { HotelId = Guid.NewGuid(), HotelName = "Test Hotel",Address = "Test Address", PhoneNumber = "1234567890"};
        _dbContext.Hotels.Add(hotel);
        await _dbContext.SaveChangesAsync();

        var dto = new CreateRoomDto 
        { 
            HotelId = hotel.HotelId,
            RoomNumber = "101", 
            Description = "Test Room", 
            PricePerNight = 100, 
            RoomType = RoomType.Single, 
            AdultsCapacity = 2, 
            ChildrenCapacity = 1, 
            Availability = true 
        };

        
        await _roomService.CreateRoomAsync(dto);

        
        var createdRoom = await _dbContext.Rooms.FirstOrDefaultAsync(r => r.RoomNumber == "101");
        Assert.NotNull(createdRoom);
    }

    [Fact]
    public async Task UpdateRoomAsync_ShouldUpdateRoom_WhenRoomExists()
    {
        
        var hotelId = Guid.NewGuid();
        var hotel = new Hotel 
        { 
            HotelId = hotelId, 
            HotelName = "Test Hotel", 
            Address = "Test Location", 
            StarRating = 4, 
            PhoneNumber = "1234567890",
        };
        _dbContext.Hotels.Add(hotel);
        await _dbContext.SaveChangesAsync();

        var roomId = Guid.NewGuid();
        var room = new Room 
        { 
            RoomId = roomId, 
            HotelId = hotelId, 
            RoomNumber = "101", 
            Description = "Test Room", 
            PricePerNight = 100, 
            RoomType = RoomType.Single, 
            AdultsCapacity = 2, 
            ChildrenCapacity = 1, 
            Availability = true 
        };
        _dbContext.Rooms.Add(room);
        await _dbContext.SaveChangesAsync();

        var dto = new UpdateRoomDto 
        { 
            HotelId = hotelId, 
            RoomNumber = "102", 
            Description = "Updated Room", 
            PricePerNight = 120, 
            RoomType = RoomType.Double, 
            AdultsCapacity = 3, 
            ChildrenCapacity = 2, 
            Availability = false 
        };

        
        await _roomService.UpdateRoomAsync(roomId, dto);

        
        var updatedRoom = await _dbContext.Rooms.FindAsync(roomId);
        Assert.NotNull(updatedRoom);
        Assert.Equal("102", updatedRoom.RoomNumber);
        Assert.Equal("Updated Room", updatedRoom.Description);
        Assert.Equal(120, updatedRoom.PricePerNight);
        Assert.Equal(RoomType.Double, updatedRoom.RoomType);
        Assert.Equal(3, updatedRoom.AdultsCapacity);
        Assert.Equal(2, updatedRoom.ChildrenCapacity);
        Assert.False(updatedRoom.Availability);
    }

    [Fact]
    public async Task UpdateRoomAsync_ShouldThrowNotFoundException_WhenRoomDoesNotExist()
    {
        
        await Assert.ThrowsAsync<NotFoundException>(() => 
            _roomService.UpdateRoomAsync(Guid.NewGuid(), new UpdateRoomDto()));
    }

    [Fact]
    public async Task DeleteRoomAsync_ShouldDeleteRoom_WhenRoomExists()
    {
        
        var roomId = Guid.NewGuid();
        var room = new Room 
        { 
            RoomId = roomId, 
            RoomNumber = "101", 
            Description = "Test Room", 
            PricePerNight = 100, 
            RoomType = RoomType.Single, 
            AdultsCapacity = 2, 
            ChildrenCapacity = 1, 
            Availability = true 
        };
        _dbContext.Rooms.Add(room);
        await _dbContext.SaveChangesAsync();

        
        await _roomService.DeleteRoomAsync(roomId);

        
        var deletedRoom = await _dbContext.Rooms.FindAsync(roomId);
        Assert.Null(deletedRoom);
    }
}