using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using Xunit;
using TokenGenerator;
using TravelAndAccommodationBookingPlatform.Domain.Enums;

namespace TravelAndAccommodationBookingPlatform.Tests.UnitTests.Services;

public class JwtGeneratorServiceUnitTests
    {
        private readonly JwtGeneratorService _jwtGeneratorService;

        public JwtGeneratorServiceUnitTests()
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

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("203.0.113.1")
                });

            var httpClient = new HttpClient(handlerMock.Object);

            _jwtGeneratorService = new JwtGeneratorService(configuration, httpClient);
        }

        [Fact]
        public async Task GenerateToken_ShouldReturnToken()
        {
            var userId = Guid.NewGuid();
            var username = "testuser";
            var userRole = UserRole.User;

            var token = await _jwtGeneratorService.GenerateTokenAsync(userId, username, userRole);

            Assert.NotNull(token);
            Assert.Contains(".", token);
        }
    }