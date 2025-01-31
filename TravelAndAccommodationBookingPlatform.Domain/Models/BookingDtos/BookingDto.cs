using TravelAndAccommodationBookingPlatform.Domain.Models.BookingDetailDtos;
using TravelAndAccommodationBookingPlatform.Domain.Models.UserDtos;

namespace TravelAndAccommodationBookingPlatform.Domain.Models;

public class BookingDto
{
    public Guid BookingId { get; set; }
    public string Status { get; set; }
    public UserDto User { get; set; }
    public List<BookingDetailDto> BookingDetails { get; set; }
}