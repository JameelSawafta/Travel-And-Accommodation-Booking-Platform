using System;
using Microsoft.Extensions.Configuration;
using PasswordHashing;
using Xunit;

namespace TravelAndAccommodationBookingPlatform.Tests.UnitTests.Services;

public class Argon2PasswordServiceUnitTests
{
    private readonly Argon2PasswordService _passwordService;

    public Argon2PasswordServiceUnitTests()
    {
        var inMemorySettings = new Dictionary<string, string>
        {
            { "Argon2PasswordHashing:SaltSize", "16" },
            { "Argon2PasswordHashing:HashSize", "32" },
            { "Argon2PasswordHashing:MemorySize", "65536" },
            { "Argon2PasswordHashing:DegreeOfParallelism", "4" },
            { "Argon2PasswordHashing:Iterations", "4" },
            { "Argon2PasswordHashing:Secret", "SuperSecretKey123!" }
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        _passwordService = new Argon2PasswordService(configuration);
    }

    [Fact]
    public void GenerateSalt_ShouldReturnValidSalt()
    {
        var salt = _passwordService.GenerateSalt();

        Assert.NotNull(salt);
        Assert.Equal(16, salt.Length);
    }

    [Fact]
    public void GenerateHashedPassword_ShouldReturnHash()
    {
        var password = "TestPassword";
        var salt = _passwordService.GenerateSalt();

        var hash = _passwordService.GenerateHashedPassword(password, salt);

        Assert.NotNull(hash);
        Assert.NotEmpty(hash);
    }

    [Fact]
    public void VerifyPassword_ShouldReturnTrue_ForValidPassword()
    {
        var password = "TestPassword";
        var salt = _passwordService.GenerateSalt();

        var hash = _passwordService.GenerateHashedPassword(password, salt);

        var isValid = _passwordService.VerifyPassword(password, hash, salt);

        Assert.True(isValid);
    }

    [Fact]
    public void VerifyPassword_ShouldReturnFalse_ForInvalidPassword()
    {
        var password = "TestPassword";
        var salt = _passwordService.GenerateSalt();

        var hash = _passwordService.GenerateHashedPassword(password, salt);

        var isValid = _passwordService.VerifyPassword("WrongPassword", hash, salt);

        Assert.False(isValid);
    }
}
