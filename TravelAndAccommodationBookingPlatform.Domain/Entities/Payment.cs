using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TravelAndAccommodationBookingPlatform.Domain.Enums;

namespace TravelAndAccommodationBookingPlatform.Domain.Entities;

public class Payment
{
    [Key]
    public Guid PaymentId { get; set; } = Guid.NewGuid();
    [Required]
    public Guid BookingId { get; set; }
    [Range(0, 100000)]
    public decimal Amount { get; set; }
    [Required]
    public PaymentMethod PaymentMethod { get; set; }
    [Required]
    [Column(TypeName = "timestamp")]
    public DateTime TransactionDate { get; set; } = DateTime.Now;
    [Required]
    public PaymentStatus Status { get; set; }

    public Booking Booking { get; set; }
}