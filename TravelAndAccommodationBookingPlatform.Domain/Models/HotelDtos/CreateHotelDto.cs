namespace TravelAndAccommodationBookingPlatform.Domain.Models.HotelDtos;

public class CreateHotelDto
{
    public Guid CityId { get; set; }
    public string HotelName { get; set; }
    public string? Description { get; set; }
    public int StarRating { get; set; }
    public Guid OwnerId { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string? ThumbnailUrl { get; set; }
}