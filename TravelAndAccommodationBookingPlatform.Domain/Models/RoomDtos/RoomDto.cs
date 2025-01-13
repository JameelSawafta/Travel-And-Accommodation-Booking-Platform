using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Enums;
using TravelAndAccommodationBookingPlatform.Domain.Models.AmenityDtos;
using TravelAndAccommodationBookingPlatform.Domain.Models.ImageDtos;

namespace TravelAndAccommodationBookingPlatform.Domain.Models.RoomDtos;

public class RoomDto
{
    public Guid RoomId { get; set; }
    public string RoomNumber { get; set; }
    public decimal PricePerNight { get; set; }
    public string RoomType { get; set; }
    public string Description { get; set; }
    public int AdultsCapacity { get; set; }
    public int ChildrenCapacity { get; set; }
    public bool Availability { get; set; }
    public double DiscountPercentageValue { get; set; }
    public ICollection<AmenityDto> RoomAmenities { get; set; }
    public ICollection<ImageDto> Images { get; set; }
}