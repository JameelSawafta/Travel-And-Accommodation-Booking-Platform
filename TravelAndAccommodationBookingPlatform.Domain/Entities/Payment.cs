using TravelAndAccommodationBookingPlatform.Domain.Enums;

namespace TravelAndAccommodationBookingPlatform.Domain.Entities;

public class Payment
{
    public Guid PaymentId { get; set; } = Guid.NewGuid();
    public Guid BookingId { get; set; }
    public decimal Amount { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public string? TransactionID { get; set; }
    public DateTime TransactionDate { get; set; } = DateTime.Now;
    public PaymentStatus Status { get; set; }

    public Booking Booking { get; set; }
}