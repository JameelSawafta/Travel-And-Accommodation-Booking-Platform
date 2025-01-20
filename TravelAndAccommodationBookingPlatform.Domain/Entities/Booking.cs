using TravelAndAccommodationBookingPlatform.Domain.Enums;

namespace TravelAndAccommodationBookingPlatform.Domain.Entities;

public class Booking
{
    public Guid BookingId { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public Guid RoomId { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public decimal TotalPrice { get; set; }
    public BookingStatus Status { get; set; }

    public User User { get; set; }
    public Room Room { get; set; }
    public ICollection<Payment> Payments { get; set; }
}