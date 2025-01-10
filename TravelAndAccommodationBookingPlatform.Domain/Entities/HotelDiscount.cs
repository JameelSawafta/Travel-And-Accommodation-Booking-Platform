
namespace TravelAndAccommodationBookingPlatform.Domain.Entities;

public class HotelDiscount
{
    public Guid HotelDiscountId { get; set; } = Guid.NewGuid();
    public Guid HotelId { get; set; }
    public Guid DiscountId { get; set; }

    public Hotel Hotel { get; set; }
    public Discount Discount { get; set; }
}