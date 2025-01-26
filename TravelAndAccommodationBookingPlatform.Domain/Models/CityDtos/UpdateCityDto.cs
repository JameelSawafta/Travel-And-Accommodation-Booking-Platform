namespace TravelAndAccommodationBookingPlatform.Domain.Models.CityDtos;

public class UpdateCityDto
{
    public string CityName { get; set; }
    public string Country { get; set; }
    public string? PostCode { get; set; }
    public string? ThumbnailUrl { get; set; }
}