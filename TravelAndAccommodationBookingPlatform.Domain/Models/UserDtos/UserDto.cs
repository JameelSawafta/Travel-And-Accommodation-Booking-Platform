namespace TravelAndAccommodationBookingPlatform.Domain.Models.UserDtos;

public class UserDto
{
    public Guid UserId { get; set; }
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
}