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
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;

    public JwtGeneratorService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }

    public string GenerateToken(Guid userId, string username, UserRole role)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:SecretForKey"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var clientIp = _httpContextAccessor?.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "Unknown";        
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
}