namespace TravelAndAccommodationBookingPlatform.Domain.Models.RoomDtos;

public class RoomPdfDto
{
    public Guid RoomId { get; set; }
    public string RoomNumber { get; set; }
    public decimal PricePerNight { get; set; }
    public string RoomType { get; set; }
    public string Description { get; set; }
}