using AutoMapper;
using Moq;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Enums;
using TravelAndAccommodationBookingPlatform.Domain.Exceptions;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Services;
using TravelAndAccommodationBookingPlatform.Domain.Models.UserDtos;
using TravelAndAccommodationBookingPlatform.Domain.Services;

namespace TravelAndAccommodationBookingPlatform.Tests.Services;

public class AuthServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IPasswordService> _passwordServiceMock;
    private readonly Mock<ITokenGeneratorService> _tokenGeneratorServiceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _passwordServiceMock = new Mock<IPasswordService>();
        _tokenGeneratorServiceMock = new Mock<ITokenGeneratorService>();
        _mapperMock = new Mock<IMapper>();

        _authService = new AuthService(
            _userRepositoryMock.Object,
            _passwordServiceMock.Object,
            _tokenGeneratorServiceMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnToken_WhenValidCredentials()
    {
        var loginDto = new LoginDto { Username = "testuser", Password = "password" };
        var user = new User { UserId = Guid.NewGuid(), Username = "testuser", PasswordHash = "hash", Salt = "salt" };

        _userRepositoryMock.Setup(r => r.GetUserByUsernameAsync(loginDto.Username))
            .ReturnsAsync(user);
        _passwordServiceMock.Setup(p => p.VerifyPassword(loginDto.Password, user.PasswordHash, Convert.FromBase64String(user.Salt)))
            .Returns(true);
        _tokenGeneratorServiceMock.Setup(t => t.GenerateToken(user.UserId, user.Username, user.Role))
            .Returns("valid_token");

        var token = await _authService.LoginAsync(loginDto);

        Assert.Equal("valid_token", token);
    }

    [Fact]
    public async Task LoginAsync_ShouldThrowException_WhenInvalidCredentials()
    {
        var loginDto = new LoginDto { Username = "testuser", Password = "password" };

        _userRepositoryMock.Setup(r => r.GetUserByUsernameAsync(loginDto.Username))
            .ReturnsAsync((User?)null);

        await Assert.ThrowsAsync<AuthenticationFailedException>(() => _authService.LoginAsync(loginDto));
    }
    
    [Fact]
    public async Task SignupAsync_ShouldReturnUserResponse_WhenUserIsCreated()
    {
        var signupDto = new SignupDto { Username = "newuser", Password = "password" };
        var user = new User { UserId = Guid.NewGuid(), Username = "newuser", Role = UserRole.User };
        var userResponse = new UserCreationResponseDto {Username = user.Username, Token = "valid_token" };

        _userRepositoryMock.Setup(r => r.GetUserByUsernameAsync(signupDto.Username))
            .ReturnsAsync((User?)null);
        _mapperMock.Setup(m => m.Map<User>(signupDto)).Returns(user);
        _passwordServiceMock.Setup(p => p.GenerateSalt()).Returns(new byte[] { 1, 2, 3 });
        _passwordServiceMock.Setup(p => p.GenerateHashedPassword(signupDto.Password, It.IsAny<byte[]>()))
            .Returns("hashedpassword");
        _userRepositoryMock.Setup(r => r.CreateUserAsync(user)).Returns(Task.CompletedTask);
        _tokenGeneratorServiceMock.Setup(t => t.GenerateToken(user.UserId, user.Username, user.Role))
            .Returns("valid_token");
        _mapperMock.Setup(m => m.Map<UserCreationResponseDto>(user)).Returns(userResponse);

        var result = await _authService.SignupAsync(signupDto);

        Assert.Equal(userResponse, result);
    }

}
