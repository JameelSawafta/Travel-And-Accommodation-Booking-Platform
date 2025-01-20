
namespace TravelAndAccommodationBookingPlatform.Domain.Entities;

public class Review
{
    public Guid ReviewId { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public Guid HotelId { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }

    public User User { get; set; }
    public Hotel Hotel { get; set; }
}