
namespace TravelAndAccommodationBookingPlatform.Domain.Entities;

public class HotelAmenity
{
    public Guid HotelAmenityId { get; set; } = Guid.NewGuid();
    public Guid HotelId { get; set; }
    public Guid AmenityId { get; set; }

    public Hotel Hotel { get; set; }
    public Amenity Amenity { get; set; }
}