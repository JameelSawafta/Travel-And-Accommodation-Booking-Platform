namespace TravelAndAccommodationBookingPlatform.Domain.Models.CartDtos;

public class CartDto
{
    public Guid CartId { get; set; }
    public Guid UserId { get; set; }
    public Guid RoomId { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public decimal Price { get; set; }
}