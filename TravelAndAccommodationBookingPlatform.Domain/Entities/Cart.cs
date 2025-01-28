namespace TravelAndAccommodationBookingPlatform.Domain.Entities;

public class Cart
{
    public Guid CartId { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public Guid RoomId { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public decimal Price { get; set; }
    
    public Room Room { get; set; }
    public User User { get; set; }
}