namespace TravelAndAccommodationBookingPlatform.Domain.Models.CartDtos;


public class AddToCartDto
{
    public Guid UserId { get; set; }
    public Guid RoomId { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
}