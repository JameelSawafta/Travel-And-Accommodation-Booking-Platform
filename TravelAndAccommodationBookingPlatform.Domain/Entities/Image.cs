namespace TravelAndAccommodationBookingPlatform.Domain.Entities;

public class Image
{
    public Guid ImageId { get; set; } = Guid.NewGuid();
    public Guid RoomId { get; set; }
    public string ImageUrl { get; set; }

    public Room Room { get; set; }
}