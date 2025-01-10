
namespace TravelAndAccommodationBookingPlatform.Domain.Entities;

public class RoomAmenity
{
    public Guid RoomAmenityId { get; set; } = Guid.NewGuid();
    public Guid RoomId { get; set; }
    public Guid AmenityId { get; set; }

    public Room Room { get; set; }
    public Amenity Amenity { get; set; }
}