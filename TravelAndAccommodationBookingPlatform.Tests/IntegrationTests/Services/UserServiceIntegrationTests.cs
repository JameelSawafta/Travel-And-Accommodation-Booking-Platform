using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TravelAndAccommodationBookingPlatform.Db.DbContext;
using TravelAndAccommodationBookingPlatform.Db.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Exceptions;
using TravelAndAccommodationBookingPlatform.Domain.Models.UserDtos;
using TravelAndAccommodationBookingPlatform.Domain.Services;

namespace TravelAndAccommodationBookingPlatform.Tests.IntegrationTests.Services;

public class UserServiceIntegrationTests :  IDisposable
{
     private readonly DbContextOptions<TravelAndAccommodationBookingPlatformDbContext> _dbOptions;
    private readonly TravelAndAccommodationBookingPlatformDbContext _dbContext;
    private readonly UserService _userService;

    public UserServiceIntegrationTests()
    {
        _dbOptions = new DbContextOptionsBuilder<TravelAndAccommodationBookingPlatformDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _dbContext = new TravelAndAccommodationBookingPlatformDbContext(_dbOptions);

        var userRepository = new UserRepository(_dbContext);
        var mapper = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<User, UserDto>();
        }).CreateMapper();

        _userService = new UserService(userRepository, mapper);
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    [Fact]
    public async Task GetUserByIdAsync_ShouldReturnUserDto_WhenUserExists()
    {
        var userId = Guid.NewGuid();
        var user = new User
        {
            UserId = userId,
            Username = "testuser",
            FirstName = "Test",
            LastName = "User",
            Email = "testuser@example.com",
            PasswordHash = "hashedpassword",
            Salt = "somesalt"
        };
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        var result = await _userService.GetUserByIdAsync(userId);

        Assert.NotNull(result);
        Assert.Equal(userId, result.UserId);
        Assert.Equal("testuser", result.Username);
        Assert.Equal("Test", result.FirstName);
        Assert.Equal("User", result.LastName);
        Assert.Equal("testuser@example.com", result.Email);
    }

    [Fact]
    public async Task GetUserByIdAsync_ShouldThrowNotFoundException_WhenUserDoesNotExist()
    {
        var userId = Guid.NewGuid();

        await Assert.ThrowsAsync<NotFoundException>(() => _userService.GetUserByIdAsync(userId));
    }
}