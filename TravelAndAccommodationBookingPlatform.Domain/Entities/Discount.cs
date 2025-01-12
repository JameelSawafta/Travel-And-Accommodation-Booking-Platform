using TravelAndAccommodationBookingPlatform.Domain.Enums;

namespace TravelAndAccommodationBookingPlatform.Domain.Entities;

public class Discount
{
    public Guid DiscountId { get; set; } = Guid.NewGuid();
    public string? Description { get; set; }
    public double DiscountPercentageValue { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }
    
    public ICollection<RoomDiscount> RoomDiscounts { get; set; }
}