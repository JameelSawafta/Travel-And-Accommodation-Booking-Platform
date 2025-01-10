
namespace TravelAndAccommodationBookingPlatform.Domain.Entities;

public class Review
{
    public Guid ReviewID { get; set; } = Guid.NewGuid();
    public Guid UserID { get; set; }
    public Guid HotelID { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }

    public User User { get; set; }
    public Hotel Hotel { get; set; }
}