using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TravelAndAccommodationBookingPlatform.Db.DbContext;
using TravelAndAccommodationBookingPlatform.Db.DbServices;
using TravelAndAccommodationBookingPlatform.Db.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Models.CartDtos;
using TravelAndAccommodationBookingPlatform.Domain.Services;

namespace TravelAndAccommodationBookingPlatform.Tests.IntegrationTests.Services;

public class CartServiceIntegrationTests : IDisposable
{
    private readonly CartService _cartService;
    private readonly DbContextOptions<TravelAndAccommodationBookingPlatformDbContext> _dbOptions;
    private readonly TravelAndAccommodationBookingPlatformDbContext _dbContext;

    public CartServiceIntegrationTests()
    {
        _dbOptions = new DbContextOptionsBuilder<TravelAndAccommodationBookingPlatformDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _dbContext = new TravelAndAccommodationBookingPlatformDbContext(_dbOptions);

        var cartRepository = new CartRepository(_dbContext, new PaginationService());
        var userRepository = new UserRepository(_dbContext);
        var roomRepository = new RoomRepository(_dbContext, new PaginationService());

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Cart, CartDto>();
            cfg.CreateMap<AddToCartDto, Cart>();
        });
        var mapper = mapperConfig.CreateMapper();

        _cartService = new CartService(cartRepository, userRepository, roomRepository, mapper);
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
    
    [Fact]
    public async Task AddToCartAsync_ShouldAddCartItem_WhenValidInput()
    {
        
        var userId = Guid.NewGuid();
        var roomId = Guid.NewGuid();
        var checkInDate = DateTime.Now.AddDays(1);
        var checkOutDate = DateTime.Now.AddDays(3);

        var user = new User 
        { 
            UserId = userId,
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            PasswordHash = "hashedpassword",
            Salt = "somesalt",
            Username = "johndoe"
        };
        var room = new Room { 
            RoomId = roomId,
            PricePerNight = 100,
            Availability = true,
            Description = "A comfortable room with a view",
            RoomNumber = "101"
        };
        var cartDto = new AddToCartDto
        {
            UserId = userId,
            RoomId = roomId,
            CheckInDate = checkInDate,
            CheckOutDate = checkOutDate
        };

        _dbContext.Users.Add(user);
        _dbContext.Rooms.Add(room);
        await _dbContext.SaveChangesAsync();

        
        await _cartService.AddToCartAsync(cartDto);

        
        var cartItem = await _dbContext.Carts.FirstOrDefaultAsync(c => c.UserId == userId && c.RoomId == roomId);
        Assert.NotNull(cartItem);
        Assert.Equal(200, cartItem.Price); // 100 * 2 nights
    }
    
    [Fact]
    public async Task GetCartItemsAsync_ShouldReturnCartItems_WhenUserExists()
    {
        
        var userId = Guid.NewGuid();
        var user = new User 
        { 
            UserId = userId,
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            PasswordHash = "hashedpassword",
            Salt = "somesalt",
            Username = "johndoe"
        };
        var cartItem = new Cart
        {
            CartId = Guid.NewGuid(),
            UserId = userId,
            RoomId = Guid.NewGuid(),
            Price = 200
        };

        _dbContext.Users.Add(user);
        _dbContext.Carts.Add(cartItem);
        await _dbContext.SaveChangesAsync();

        
        var result = await _cartService.GetCartItemsAsync(userId, 1, 10);

        
        Assert.NotNull(result);
        Assert.Single(result.Items);
        Assert.Equal(200, result.Items.First().Price);
    }
    
    [Fact]
    public async Task RemoveFromCartAsync_ShouldRemoveCartItem_WhenCartItemExists()
    {
        
        var cartId = Guid.NewGuid();
        var cartItem = new Cart { CartId = cartId, UserId = Guid.NewGuid(), RoomId = Guid.NewGuid(), Price = 200 };

        _dbContext.Carts.Add(cartItem);
        await _dbContext.SaveChangesAsync();

        
        await _cartService.RemoveFromCartAsync(cartId);

        
        var deletedCartItem = await _dbContext.Carts.FindAsync(cartId);
        Assert.Null(deletedCartItem);
    }
    
    [Fact]
    public async Task ClearCartAsync_ShouldClearCart_WhenUserExists()
    {
        
        var userId = Guid.NewGuid();
        var cartItem1 = new Cart { CartId = Guid.NewGuid(), UserId = userId, RoomId = Guid.NewGuid(), Price = 200 };
        var cartItem2 = new Cart { CartId = Guid.NewGuid(), UserId = userId, RoomId = Guid.NewGuid(), Price = 300 };

        _dbContext.Carts.AddRange(cartItem1, cartItem2);
        await _dbContext.SaveChangesAsync();

        
        await _cartService.ClearCartAsync(userId);

        
        var cartItems = await _dbContext.Carts.Where(c => c.UserId == userId).ToListAsync();
        Assert.Empty(cartItems);
    }
}