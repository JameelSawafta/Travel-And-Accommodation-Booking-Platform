using AutoMapper;
using Moq;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Enums;
using TravelAndAccommodationBookingPlatform.Domain.Exceptions;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Models.RoomDtos;
using TravelAndAccommodationBookingPlatform.Domain.Services;

namespace TravelAndAccommodationBookingPlatform.Tests.UnitTests.Services;

public class RoomServiceTests
{
    private readonly Mock<IRoomRepository> _mockRoomRepository;
    private readonly Mock<IHotelRepository> _mockHotelRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly RoomService _roomService;

    public RoomServiceTests()
    {
        _mockRoomRepository = new Mock<IRoomRepository>();
        _mockHotelRepository = new Mock<IHotelRepository>();
        _mockMapper = new Mock<IMapper>();
        _roomService = new RoomService(_mockRoomRepository.Object, _mockHotelRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task GetAllRoomsAsync_ShouldReturnPaginatedList_WhenRoomsExist()
    {
        
        var rooms = new List<Room>
        {
            new Room { RoomId = Guid.NewGuid(), RoomNumber = "101", HotelId = Guid.NewGuid() },
            new Room { RoomId = Guid.NewGuid(), RoomNumber = "102", HotelId = Guid.NewGuid() }
        };
        var paginatedRooms = (rooms.AsEnumerable(), 2);
        var roomDtos = rooms.Select(r => new RoomDto { RoomId = r.RoomId, RoomNumber = r.RoomNumber }).ToList();

        _mockRoomRepository.Setup(repo => repo.GetAllRoomsAsync(1, 10))
            .ReturnsAsync(paginatedRooms);
        _mockMapper.Setup(mapper => mapper.Map<IEnumerable<RoomDto>>(It.IsAny<IEnumerable<Room>>()))
            .Returns(roomDtos);

        
        var result = await _roomService.GetAllRoomsAsync(1, 10);

        
        Assert.Equal(2, result.Items.Count);
        Assert.Equal(10, result.PageData.PageSize);
    }

    [Fact]
    public async Task GetAllRoomsAsync_ShouldReturnEmptyList_WhenNoRoomsExist()
    {
        
        var emptyResult = (Enumerable.Empty<Room>(), 0);
        _mockRoomRepository.Setup(repo => repo.GetAllRoomsAsync(1, 10))
            .ReturnsAsync(emptyResult);

        
        var result = await _roomService.GetAllRoomsAsync(1, 10);

        
        Assert.Empty(result.Items);
    }

    [Fact]
    public async Task GetRoomByIdAsync_ShouldReturnRoom_WhenRoomExists()
    {
        
        var roomId = Guid.NewGuid();
        var room = new Room { RoomId = roomId, RoomNumber = "101", HotelId = Guid.NewGuid() };
        var roomDto = new RoomDto { RoomId = roomId, RoomNumber = "101" };

        _mockRoomRepository.Setup(repo => repo.GetRoomByIdAsync(roomId))
            .ReturnsAsync(room);
        _mockMapper.Setup(mapper => mapper.Map<RoomDto>(room))
            .Returns(roomDto);

        
        var result = await _roomService.GetRoomByIdAsync(roomId);

        
        Assert.Equal(roomId, result.RoomId);
        Assert.Equal("101", result.RoomNumber);
    }

    [Fact]
    public async Task GetRoomByIdAsync_ShouldThrowNotFoundException_WhenRoomDoesNotExist()
    {
        
        var roomId = Guid.NewGuid();
        _mockRoomRepository.Setup(repo => repo.GetRoomByIdAsync(roomId))
            .ReturnsAsync((Room)null);

        
        await Assert.ThrowsAsync<NotFoundException>(() => 
            _roomService.GetRoomByIdAsync(roomId));
    }

    [Fact]
    public async Task CreateRoomAsync_ShouldCreateRoom_WhenValidInput()
    {
        
        var hotelId = Guid.NewGuid();
        var hotel = new Hotel { HotelId = hotelId, HotelName = "Test Hotel" };
        _mockHotelRepository.Setup(repo => repo.GetHotelByIdAsync(hotelId))
            .ReturnsAsync(hotel);

        var dto = new CreateRoomDto 
        { 
            RoomNumber = "101", 
            Description = "Test Room", 
            PricePerNight = 100, 
            RoomType = RoomType.Single, 
            AdultsCapacity = 2, 
            ChildrenCapacity = 1, 
            Availability = true,
            HotelId = hotelId
        };
        var room = new Room { RoomId = Guid.NewGuid(), RoomNumber = "101", HotelId = hotelId };

        _mockMapper.Setup(mapper => mapper.Map<Room>(dto))
            .Returns(room);

        
        await _roomService.CreateRoomAsync(dto);

        
        _mockRoomRepository.Verify(repo => repo.CreateRoomAsync(room), Times.Once);
    }

    [Fact]
    public async Task CreateRoomAsync_ShouldThrowNotFoundException_WhenHotelDoesNotExist()
    {
        
        var hotelId = Guid.NewGuid();
        _mockHotelRepository.Setup(repo => repo.GetHotelByIdAsync(hotelId))
            .ReturnsAsync((Hotel)null);

        var dto = new CreateRoomDto 
        { 
            RoomNumber = "101", 
            Description = "Test Room", 
            PricePerNight = 100, 
            RoomType = RoomType.Single, 
            AdultsCapacity = 2, 
            ChildrenCapacity = 1, 
            Availability = true,
            HotelId = hotelId
        };

        
        await Assert.ThrowsAsync<NotFoundException>(() => 
            _roomService.CreateRoomAsync(dto));
    }

    [Fact]
    public async Task UpdateRoomAsync_ShouldUpdateRoom_WhenRoomExists()
    {
        
        var roomId = Guid.NewGuid();
        var hotelId = Guid.NewGuid();
        var existingRoom = new Room { RoomId = roomId, RoomNumber = "101", HotelId = hotelId };
        var dto = new UpdateRoomDto 
        { 
            RoomNumber = "102", 
            Description = "Updated Room", 
            PricePerNight = 120, 
            RoomType = RoomType.Double, 
            AdultsCapacity = 3, 
            ChildrenCapacity = 2, 
            Availability = false,
            HotelId = hotelId
        };

        var hotel = new Hotel { HotelId = hotelId, HotelName = "Test Hotel" };
        _mockHotelRepository.Setup(repo => repo.GetHotelByIdAsync(hotelId))
            .ReturnsAsync(hotel);

        _mockRoomRepository.Setup(repo => repo.GetRoomByIdAsync(roomId))
            .ReturnsAsync(existingRoom);
        _mockMapper.Setup(mapper => mapper.Map(dto, existingRoom))
            .Callback<UpdateRoomDto, Room>((src, dest) => dest.RoomNumber = src.RoomNumber);

        
        await _roomService.UpdateRoomAsync(roomId, dto);

        
        _mockRoomRepository.Verify(repo => repo.UpdateRoomAsync(existingRoom), Times.Once);
        Assert.Equal("102", existingRoom.RoomNumber);
    }

    [Fact]
    public async Task UpdateRoomAsync_ShouldThrowNotFoundException_WhenRoomDoesNotExist()
    {
        
        var roomId = Guid.NewGuid();
        _mockRoomRepository.Setup(repo => repo.GetRoomByIdAsync(roomId))
            .ReturnsAsync((Room)null);

        
        await Assert.ThrowsAsync<NotFoundException>(() => 
            _roomService.UpdateRoomAsync(roomId, new UpdateRoomDto()));
    }

    [Fact]
    public async Task UpdateRoomAsync_ShouldThrowNotFoundException_WhenHotelDoesNotExist()
    {
        
        var roomId = Guid.NewGuid();
        var hotelId = Guid.NewGuid();
        var existingRoom = new Room { RoomId = roomId, RoomNumber = "101", HotelId = hotelId };
        var dto = new UpdateRoomDto 
        { 
            RoomNumber = "102", 
            Description = "Updated Room", 
            PricePerNight = 120, 
            RoomType = RoomType.Double, 
            AdultsCapacity = 3, 
            ChildrenCapacity = 2, 
            Availability = false,
            HotelId = hotelId
        };

        _mockRoomRepository.Setup(repo => repo.GetRoomByIdAsync(roomId))
            .ReturnsAsync(existingRoom);
        _mockHotelRepository.Setup(repo => repo.GetHotelByIdAsync(hotelId))
            .ReturnsAsync((Hotel)null);

        
        await Assert.ThrowsAsync<NotFoundException>(() => 
            _roomService.UpdateRoomAsync(roomId, dto));
    }

    [Fact]
    public async Task DeleteRoomAsync_ShouldDeleteRoom_WhenRoomExists()
    {
        
        var roomId = Guid.NewGuid();
        var room = new Room { RoomId = roomId, RoomNumber = "101", HotelId = Guid.NewGuid() };

        _mockRoomRepository.Setup(repo => repo.GetRoomByIdAsync(roomId))
            .ReturnsAsync(room);

        
        await _roomService.DeleteRoomAsync(roomId);

        
        _mockRoomRepository.Verify(repo => repo.DeleteRoomAsync(roomId), Times.Once);
    }

    [Fact]
    public async Task DeleteRoomAsync_ShouldThrowNotFoundException_WhenRoomDoesNotExist()
    {
        
        var roomId = Guid.NewGuid();
        _mockRoomRepository.Setup(repo => repo.GetRoomByIdAsync(roomId))
            .ReturnsAsync((Room)null);

        
        await Assert.ThrowsAsync<NotFoundException>(() => 
            _roomService.DeleteRoomAsync(roomId));
    }
}