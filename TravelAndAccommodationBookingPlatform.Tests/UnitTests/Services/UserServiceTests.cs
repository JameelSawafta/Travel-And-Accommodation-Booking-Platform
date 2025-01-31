using AutoMapper;
using Moq;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Exceptions;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Models.UserDtos;
using TravelAndAccommodationBookingPlatform.Domain.Services;

namespace TravelAndAccommodationBookingPlatform.Tests.UnitTests.Services;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockMapper = new Mock<IMapper>();
        _userService = new UserService(_mockUserRepository.Object, _mockMapper.Object);
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
            Email = "testuser@example.com"
        };

        var userDto = new UserDto
        {
            UserId = userId,
            Username = "testuser",
            FirstName = "Test",
            LastName = "User",
            Email = "testuser@example.com"
        };

        _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(userId))
            .ReturnsAsync(user);
        _mockMapper.Setup(mapper => mapper.Map<UserDto>(user))
            .Returns(userDto);

        var result = await _userService.GetUserByIdAsync(userId);

        Assert.NotNull(result);
        Assert.Equal(userId, result.UserId);
        Assert.Equal("testuser", result.Username);
        Assert.Equal("Test", result.FirstName);
        Assert.Equal("User", result.LastName);
        Assert.Equal("testuser@example.com", result.Email);

        _mockUserRepository.Verify(repo => repo.GetUserByIdAsync(userId), Times.Once);
        _mockMapper.Verify(mapper => mapper.Map<UserDto>(user), Times.Once);
    }

    [Fact]
    public async Task GetUserByIdAsync_ShouldThrowNotFoundException_WhenUserDoesNotExist()
    {
        var userId = Guid.NewGuid();
        _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(userId))
            .ReturnsAsync((User)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _userService.GetUserByIdAsync(userId));

        _mockUserRepository.Verify(repo => repo.GetUserByIdAsync(userId), Times.Once);
        _mockMapper.Verify(mapper => mapper.Map<UserDto>(It.IsAny<User>()), Times.Never);
    }
}