using TravelAndAccommodationBookingPlatform.Domain.Enums;

namespace TravelAndAccommodationBookingPlatform.Domain.Entities;

public class Discount
{
    public Guid DiscountId { get; set; } = Guid.NewGuid();
    public string? Description { get; set; }
    public DiscountType DiscountType { get; set; }
    public double DiscountValue { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }
    
    public ICollection<HotelDiscount> HotelDiscounts { get; set; }
    public ICollection<RoomDiscount> RoomDiscounts { get; set; }
}