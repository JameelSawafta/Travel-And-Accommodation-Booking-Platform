
namespace TravelAndAccommodationBookingPlatform.Domain.Entities;

public class Amenity
{
    public Guid AmenityId { get; set; } = Guid.NewGuid();
    public string AmenityName { get; set; }
    public string Description { get; set; }
    
    public ICollection<RoomAmenity> RoomAmenities { get; set; }
}