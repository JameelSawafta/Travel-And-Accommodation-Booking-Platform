using TravelAndAccommodationBookingPlatform.Domain.Models.RoomDtos;

namespace TravelAndAccommodationBookingPlatform.Domain.Models.BookingDetailDtos;

public class BookingDetailDto
{
    public Guid BookingDetailId { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public decimal Price { get; set; }
    public RoomPdfDto Room { get; set; }
}