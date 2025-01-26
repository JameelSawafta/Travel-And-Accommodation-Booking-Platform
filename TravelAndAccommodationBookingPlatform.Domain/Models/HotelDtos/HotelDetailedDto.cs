using TravelAndAccommodationBookingPlatform.Domain.Models.CityDtos;
using TravelAndAccommodationBookingPlatform.Domain.Models.ReviewDtos;
using TravelAndAccommodationBookingPlatform.Domain.Models.RoomDtos;

namespace TravelAndAccommodationBookingPlatform.Domain.Models.HotelDtos;

public class HotelDetailedDto
{
    public Guid HotelId { get; set; }
    public string HotelName { get; set; }
    public int StarRating { get; set; }
    public string Description { get; set; }
    public string ThumbnailUrl { get; set; }
    public string Address { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public CityDto City { get; set; }
    public decimal AverageRating { get; set; }
    public List<ReviewDto> Reviews { get; set; }
    public List<RoomDetailedDto> Rooms { get; set; }
}