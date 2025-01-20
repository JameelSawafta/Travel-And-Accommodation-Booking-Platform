
namespace TravelAndAccommodationBookingPlatform.Domain.Entities;

public class Hotel
{
    public Guid HotelId { get; set; } = Guid.NewGuid();
    public Guid CityId { get; set; }
    public string HotelName { get; set; }
    public string? Description { get; set; }
    public int StarRating { get; set; }
    public Guid OwnerId { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string? ThumbnailUrl { get; set; }

    public City City { get; set; }
    public Owner Owner { get; set; }
    public ICollection<Room> Rooms { get; set; }
    public ICollection<Review> Reviews { get; set; }
}