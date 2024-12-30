using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TravelAndAccommodationBookingPlatform.Domain.Enums;

namespace TravelAndAccommodationBookingPlatform.Domain.Entities;

public class Booking
{
    [Key]
    public Guid BookingId { get; set; } = Guid.NewGuid();
    [Required]
    public Guid UserId { get; set; }
    public Guid DiscountId { get; set; }
    [Required]
    [Column(TypeName = "timestamp")]
    public DateTime CheckInDate { get; set; }
    [Required]
    [Column(TypeName = "timestamp")]
    public DateTime CheckOutDate { get; set; }
    [Range(0, 100000)]
    public decimal TotalPrice { get; set; }
    [MaxLength(50)]
    public BookingStatus Status { get; set; }
    
    public User User { get; set; }
    public Discount Discount { get; set; }
}