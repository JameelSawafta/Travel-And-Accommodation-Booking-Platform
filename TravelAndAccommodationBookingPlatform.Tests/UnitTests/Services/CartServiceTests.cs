using AutoMapper;
using Moq;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Exceptions;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Models.CartDtos;
using TravelAndAccommodationBookingPlatform.Domain.Services;

namespace TravelAndAccommodationBookingPlatform.Tests.UnitTests.Services;

public class CartServiceTests
{
    private readonly Mock<ICartRepository> _mockCartRepository;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IRoomRepository> _mockRoomRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly CartService _cartService;

    public CartServiceTests()
    {
        _mockCartRepository = new Mock<ICartRepository>();
        _mockUserRepository = new Mock<IUserRepository>();
        _mockRoomRepository = new Mock<IRoomRepository>();
        _mockMapper = new Mock<IMapper>();
        _cartService = new CartService(
            _mockCartRepository.Object,
            _mockUserRepository.Object,
            _mockRoomRepository.Object,
            _mockMapper.Object
        );
    }
    
    [Fact]
    public async Task AddToCartAsync_ShouldAddCartItem_WhenValidInput()
    {
        
        var userId = Guid.NewGuid();
        var roomId = Guid.NewGuid();
        var checkInDate = DateTime.Now.AddDays(1);
        var checkOutDate = DateTime.Now.AddDays(3);

        var user = new User { UserId = userId };
        var room = new Room { RoomId = roomId, PricePerNight = 100, Availability = true };
        var cartDto = new AddToCartDto
        {
            UserId = userId,
            RoomId = roomId,
            CheckInDate = checkInDate,
            CheckOutDate = checkOutDate
        };
        var cartItem = new Cart
        {
            UserId = userId,
            RoomId = roomId,
            CheckInDate = checkInDate,
            CheckOutDate = checkOutDate,
            Price = 200 // 100 * 2 nights
        };

        _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(user);
        _mockRoomRepository.Setup(repo => repo.GetRoomIfAvailableAsync(roomId, checkInDate, checkOutDate)).ReturnsAsync(room);
        _mockCartRepository.Setup(repo => repo.HasDateConflictAsync(userId, roomId, checkInDate, checkOutDate)).ReturnsAsync(false);
        _mockMapper.Setup(mapper => mapper.Map<Cart>(cartDto)).Returns(cartItem);

        
        await _cartService.AddToCartAsync(cartDto);

        
        _mockCartRepository.Verify(repo => repo.AddToCartAsync(cartItem), Times.Once);
    }
    
    [Fact]
    public async Task AddToCartAsync_ShouldThrowNotFoundException_WhenUserNotFound()
    {
        
        var userId = Guid.NewGuid();
        var cartDto = new AddToCartDto { UserId = userId };

        _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync((User)null);

        
        await Assert.ThrowsAsync<NotFoundException>(() => _cartService.AddToCartAsync(cartDto));
    }
    
    [Fact]
    public async Task AddToCartAsync_ShouldThrowNotFoundException_WhenRoomNotAvailable()
    {
        
        var userId = Guid.NewGuid();
        var roomId = Guid.NewGuid();
        var checkInDate = DateTime.Now.AddDays(1);
        var checkOutDate = DateTime.Now.AddDays(3);

        var user = new User { UserId = userId };
        var cartDto = new AddToCartDto
        {
            UserId = userId,
            RoomId = roomId,
            CheckInDate = checkInDate,
            CheckOutDate = checkOutDate
        };

        _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(user);
        _mockRoomRepository.Setup(repo => repo.GetRoomIfAvailableAsync(roomId, checkInDate, checkOutDate)).ReturnsAsync((Room)null);

        
        await Assert.ThrowsAsync<NotFoundException>(() => _cartService.AddToCartAsync(cartDto));
    }
    
    [Fact]
    public async Task AddToCartAsync_ShouldThrowConflictException_WhenDateConflict()
    {
        
        var userId = Guid.NewGuid();
        var roomId = Guid.NewGuid();
        var checkInDate = DateTime.Now.AddDays(1);
        var checkOutDate = DateTime.Now.AddDays(3);

        var user = new User { UserId = userId };
        var room = new Room { RoomId = roomId, PricePerNight = 100, Availability = true };
        var cartDto = new AddToCartDto
        {
            UserId = userId,
            RoomId = roomId,
            CheckInDate = checkInDate,
            CheckOutDate = checkOutDate
        };

        _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(user);
        _mockRoomRepository.Setup(repo => repo.GetRoomIfAvailableAsync(roomId, checkInDate, checkOutDate)).ReturnsAsync(room);
        _mockCartRepository.Setup(repo => repo.HasDateConflictAsync(userId, roomId, checkInDate, checkOutDate)).ReturnsAsync(true);

        
        await Assert.ThrowsAsync<ConflictException>(() => _cartService.AddToCartAsync(cartDto));
    }
    
    [Fact]
    public async Task GetCartItemsAsync_ShouldReturnCartItems_WhenUserExists()
    {
        
        var userId = Guid.NewGuid();
        var user = new User { UserId = userId };
        var cartItems = new List<Cart>
        {
            new Cart { CartId = Guid.NewGuid(), UserId = userId, RoomId = Guid.NewGuid(), Price = 200 }
        };
        var cartDtos = new List<CartDto>
        {
            new CartDto { CartId = cartItems[0].CartId, UserId = userId, RoomId = cartItems[0].RoomId, Price = 200 }
        };

        _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(user);
        _mockCartRepository.Setup(repo => repo.GetCartItemsAsync(userId, 1, 10)).ReturnsAsync((cartItems, 1));
        _mockMapper.Setup(mapper => mapper.Map<IEnumerable<CartDto>>(cartItems)).Returns(cartDtos);

        
        var result = await _cartService.GetCartItemsAsync(userId, 1, 10);

        
        Assert.NotNull(result);
        Assert.Single(result.Items);
        Assert.Equal(200, result.Items.First().Price);
    }
    
    [Fact]
    public async Task RemoveFromCartAsync_ShouldRemoveCartItem_WhenCartItemExists()
    {
        
        var cartId = Guid.NewGuid();

        
        await _cartService.RemoveFromCartAsync(cartId);

        
        _mockCartRepository.Verify(repo => repo.RemoveFromCartAsync(cartId), Times.Once);
    }
    
    [Fact]
    public async Task ClearCartAsync_ShouldClearCart_WhenUserExists()
    {
        
        var userId = Guid.NewGuid();

        
        await _cartService.ClearCartAsync(userId);

        
        _mockCartRepository.Verify(repo => repo.ClearCartAsync(userId), Times.Once);
    }
}