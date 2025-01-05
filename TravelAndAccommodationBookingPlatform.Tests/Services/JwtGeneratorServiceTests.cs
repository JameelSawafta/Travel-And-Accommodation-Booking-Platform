using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;
using TokenGenerator;
using TravelAndAccommodationBookingPlatform.Domain.Enums;

namespace TravelAndAccommodationBookingPlatform.Tests.Services;

public class JwtGeneratorServiceTests
{
    private readonly JwtGeneratorService _jwtGeneratorService;

    public JwtGeneratorServiceTests()
    {
        var inMemorySettings = new Dictionary<string, string>
        {
            { "Authentication:SecretForKey", "thisisthesecretforgeneratingakey(mustbeatleast32bitlong)" },
            { "Authentication:Issuer", "https://localhost:7278" },
            { "Authentication:Audience", "TAABPapi" },
            { "Authentication:TokenExpirationHours", "1" }
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        httpContextAccessorMock.Setup(x => x.HttpContext)
            .Returns(new DefaultHttpContext { Connection = { RemoteIpAddress = System.Net.IPAddress.Parse("127.0.0.1") } });

        _jwtGeneratorService = new JwtGeneratorService(configuration, httpContextAccessorMock.Object);
    }

    [Fact]
    public void GenerateToken_ShouldReturnToken()
    {
        var userId = Guid.NewGuid();
        var username = "testuser";
        var userRole = UserRole.User;

        var token = _jwtGeneratorService.GenerateToken(userId, username, userRole);

        Assert.NotNull(token);
        Assert.Contains(".", token);
    }
}