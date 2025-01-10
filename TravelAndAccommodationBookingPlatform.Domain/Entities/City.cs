
namespace TravelAndAccommodationBookingPlatform.Domain.Entities;

public class City
{
    public Guid CityId { get; set; } = Guid.NewGuid();
    public string CityName { get; set; }
    public string Country { get; set; }
    public string? PostCode { get; set; }
    
    public ICollection<Hotel> Hotels { get; set; }
}