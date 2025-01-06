using TravelAndAccommodationBookingPlatform.Domain.Models.UserDtos;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Services;

public interface IAuthService
{
    Task<string> LoginAsync(LoginDto loginDto);
    Task SignupAsync(SignupDto signupDto);
}