using TravelAndAccommodationBookingPlatform.Domain.Enums;

namespace TravelAndAccommodationBookingPlatform.Domain.Models.RoomDtos;

public class RoomDto
{
    public Guid RoomId { get; set; }
    public Guid HotelId { get; set; }
    public string RoomNumber { get; set; }
    public decimal PricePerNight { get; set; }
    public string RoomType { get; set; }
    public string Description { get; set; }
    public int AdultsCapacity { get; set; }
    public int ChildrenCapacity { get; set; }
    public bool Availability { get; set; }
}