namespace TravelAndAccommodationBookingPlatform.Domain.Models.CityDtos;

public class TrendingDestinationDto
{
    public Guid CityId { get; set; }
    public string CityName { get; set; }
    public string Country { get; set; }
    public string ThumbnailUrl { get; set; }
}