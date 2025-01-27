namespace TravelAndAccommodationBookingPlatform.Domain.Entities;

public class BookingDetail
{
    public Guid BookingDetailId { get; set; } = Guid.NewGuid();
    public Guid BookingId { get; set; }
    public Guid RoomId { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public decimal Price { get; set; }

    public Booking Booking { get; set; }
    public Room Room { get; set; }
}