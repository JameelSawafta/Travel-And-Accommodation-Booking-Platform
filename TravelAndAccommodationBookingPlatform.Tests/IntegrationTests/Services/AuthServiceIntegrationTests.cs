using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PasswordHashing;
using TokenGenerator;
using TravelAndAccommodationBookingPlatform.Db.DbContext;
using TravelAndAccommodationBookingPlatform.Db.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Enums;
using TravelAndAccommodationBookingPlatform.Domain.Exceptions;
using TravelAndAccommodationBookingPlatform.Domain.Models.UserDtos;
using TravelAndAccommodationBookingPlatform.Domain.Services;

namespace TravelAndAccommodationBookingPlatform.Tests.IntegrationTests.Services;

public class AuthServiceIntegrationTests
{
    private readonly AuthService _authService;
    private readonly DbContextOptions<TravelAndAccommodationBookingPlatformDbContext> _dbOptions;
    private readonly IConfiguration _configuration;

    public AuthServiceIntegrationTests()
    {
        _dbOptions = new DbContextOptionsBuilder<TravelAndAccommodationBookingPlatformDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        
        
        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new[]
            {
                new KeyValuePair<string, string>("Authentication:SecretForKey", "SuperSecureAndLongTestKey_12345678"),
                new KeyValuePair<string, string>("Authentication:Issuer", "test_issuer"),
                new KeyValuePair<string, string>("Authentication:Audience", "test_audience"),
                new KeyValuePair<string, string>("Authentication:TokenExpirationHours", "1"),
                new KeyValuePair<string, string>("Argon2PasswordHashing:SaltSize", "16"),
                new KeyValuePair<string, string>("Argon2PasswordHashing:HashSize", "32"),
                new KeyValuePair<string, string>("Argon2PasswordHashing:MemorySize", "65536"),
                new KeyValuePair<string, string>("Argon2PasswordHashing:DegreeOfParallelism", "4"),
                new KeyValuePair<string, string>("Argon2PasswordHashing:Iterations", "4"),
                new KeyValuePair<string, string>("Argon2PasswordHashing:Secret", "SuperSecretKey123!")
            })
            .Build();

        var dbContext = new TravelAndAccommodationBookingPlatformDbContext(_dbOptions);
        var userRepository = new UserRepository(dbContext);

        var passwordService = new Argon2PasswordService(_configuration); 
        var tokenGeneratorService = new JwtGeneratorService(_configuration, new HttpClient()); 

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<SignupDto, User>();
        });
        var mapper = mapperConfig.CreateMapper();

        _authService = new AuthService(userRepository, passwordService, tokenGeneratorService, mapper);
    }

    [Fact]
    public async Task SignupAsync_ShouldCreateUser()
    {
        var signupDto = new SignupDto { Username = "testuser", Password = "Password@123", Role = UserRole.User, FirstName = "Test", LastName = "User", Email = "test@gmail.com"};

        await _authService.SignupAsync(signupDto);
        
        var dbContext = new TravelAndAccommodationBookingPlatformDbContext(_dbOptions);
        var result = await dbContext.Users.FirstOrDefaultAsync(u => u.Username == signupDto.Username);

        Assert.NotNull(result);
        Assert.Equal(signupDto.Username, result.Username);
        Assert.Equal(signupDto.Role, result.Role);
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnToken_WhenValidCredentialsAreProvided()
    {
        var dbContext = new TravelAndAccommodationBookingPlatformDbContext(_dbOptions);

        var passwordService = new Argon2PasswordService(_configuration);
        var salt = passwordService.GenerateSalt();
        var hashedPassword = passwordService.GenerateHashedPassword("Password@123", salt);

        dbContext.Users.Add(new User
        {
            UserId = Guid.NewGuid(),
            Username = "testuser",
            PasswordHash = hashedPassword,
            Salt = Convert.ToBase64String(salt),
            Role = UserRole.User,
            FirstName = "Test",
            LastName = "User",
            Email = "test@gmail.com"
        });
        await dbContext.SaveChangesAsync();

        var loginDto = new LoginDto { Username = "testuser", Password = "Password@123" };

        var token = await _authService.LoginAsync(loginDto);

        Assert.NotNull(token);
        Assert.Contains(".", token); // Validate it's a JWT
    }

    [Fact]
    public async Task LoginAsync_ShouldThrowAuthenticationFailedException_WhenInvalidCredentialsAreProvided()
    {
        var loginDto = new LoginDto { Username = "nonexistentuser", Password = "wrongpassword" };

        await Assert.ThrowsAsync<AuthenticationFailedException>(() => _authService.LoginAsync(loginDto));
    }
}