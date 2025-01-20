namespace TravelAndAccommodationBookingPlatform.Domain.Models.HotelDtos;

public class RecentlyVisitedHotelDto
{
    public Guid HotelId { get; set; }
    public string HotelName { get; set; }
    public string City { get; set; }
    public string ThumbnailUrl { get; set; }
    public int StarRating { get; set; }
    public decimal PricePerNight { get; set; }
}