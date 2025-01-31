using TravelAndAccommodationBookingPlatform.Domain.Enums;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Services;

public interface ITokenGeneratorService
{
    Task<string> GenerateTokenAsync(Guid userId, string username, UserRole role);
}