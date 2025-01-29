
namespace TravelAndAccommodationBookingPlatform.Domain.Entities;

public class Review
{
    public Guid ReviewId { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public Guid HotelId { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; }
    public Hotel Hotel { get; set; }
}