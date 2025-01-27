using TravelAndAccommodationBookingPlatform.Domain.Enums;

namespace TravelAndAccommodationBookingPlatform.Domain.Entities;

public class Booking
{
    public Guid BookingId { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public BookingStatus Status { get; set; }

    public User User { get; set; }
    public ICollection<Payment> Payments { get; set; }
    public ICollection<BookingDetail> BookingDetails { get; set; }
}