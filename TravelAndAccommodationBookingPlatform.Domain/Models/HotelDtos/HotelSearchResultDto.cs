using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Models.RoomDtos;

namespace TravelAndAccommodationBookingPlatform.Domain.Models.HotelDtos;

public class HotelSearchResultDto
{
    public Guid HotelId { get; set; }
    public string HotelName { get; set; }
    public int StarRating { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string CityName { get; set; }
    public List<RoomDto> Rooms { get; set; }
}