namespace TravelAndAccommodationBookingPlatform.Domain.Models.HotelDtos;

public class FeaturedDealDto
{
    public Guid HotelId { get; set; }
    public Guid RoomId { get; set; }
    public string HotelName { get; set; }
    public string Location { get; set; }
    public string ThumbnailUrl { get; set; }
    public int StarRating { get; set; }
    public decimal OriginalPrice { get; set; }
    public decimal DiscountedPrice { get; set; }
}