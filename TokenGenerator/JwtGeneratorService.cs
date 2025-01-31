using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TravelAndAccommodationBookingPlatform.Domain.Enums;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Services;

namespace TokenGenerator;

public class JwtGeneratorService : ITokenGeneratorService
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public JwtGeneratorService(IConfiguration configuration, HttpClient httpClient)
    {
        _configuration = configuration;
        _httpClient = httpClient;
    }

    public async Task<string> GenerateTokenAsync(Guid userId, string username, UserRole role)
    {
        var clientIp = await GetRouterPublicIpAsync();

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:SecretForKey"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("UserId", userId.ToString()),
            new Claim("Role", role.ToString()),
            new Claim("ClientIp", clientIp)
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Authentication:Issuer"],
            audience: _configuration["Authentication:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(Convert.ToDouble(_configuration["Authentication:TokenExpirationHours"])),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private async Task<string> GetRouterPublicIpAsync()
    {
        try
        {
            string apiUrl = _configuration["Authentication:PublicIpApiUrl"];
            var response = await _httpClient.GetStringAsync(apiUrl);
            return response;
        }
        catch (Exception ex)
        {
            return "Unknown";
        }
    }
}