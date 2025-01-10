
namespace TravelAndAccommodationBookingPlatform.Domain.Entities;

public class Amenity
{
    public Guid AmenityID { get; set; } = Guid.NewGuid();
    public string AmenityName { get; set; }
    
    public ICollection<HotelAmenity> HotelAmenities { get; set; }
    public ICollection<RoomAmenity> RoomAmenities { get; set; }
}