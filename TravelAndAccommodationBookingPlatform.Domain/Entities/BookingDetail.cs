using System.ComponentModel.DataAnnotations;

namespace TravelAndAccommodationBookingPlatform.Domain.Entities;

public class BookingDetail
{
    [Key]
    public Guid BookingDetailsId { get; set; } = Guid.NewGuid();
    [Required]
    public Guid BookingId { get; set; }
    [Required]
    public Guid RoomId { get; set; }
    public Guid DiscountId { get; set; }
    [Range(0, 100000)]
    public decimal Price { get; set; }

    public Booking Booking { get; set; }
    public Room Room { get; set; }
    public Discount Discount { get; set; }
}