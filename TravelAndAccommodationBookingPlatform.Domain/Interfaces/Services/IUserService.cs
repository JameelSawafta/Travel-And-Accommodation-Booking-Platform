using TravelAndAccommodationBookingPlatform.Domain.Models.UserDtos;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Services;

public interface IUserService
{
    Task<UserDto> GetUserByIdAsync(Guid userId);
}