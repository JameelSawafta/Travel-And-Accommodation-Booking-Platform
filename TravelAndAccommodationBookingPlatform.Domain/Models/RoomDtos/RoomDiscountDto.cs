using TravelAndAccommodationBookingPlatform.Domain.Models.DiscountDtos;

namespace TravelAndAccommodationBookingPlatform.Domain.Models.RoomDtos;

public class RoomDiscountDto
{
    public Guid RoomDiscountId { get; set; } = Guid.NewGuid();
    public Guid RoomId { get; set; }
    public Guid DiscountId { get; set; }
    public DiscountDto Discount { get; set; }
}