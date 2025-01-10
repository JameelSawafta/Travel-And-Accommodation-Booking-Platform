
namespace TravelAndAccommodationBookingPlatform.Domain.Entities;

public class RoomDiscount
{
    public Guid RoomDiscountId { get; set; } = Guid.NewGuid();
    public Guid RoomId { get; set; }
    public Guid DiscountId { get; set; }

    public Room Room { get; set; }
    public Discount Discount { get; set; }
}