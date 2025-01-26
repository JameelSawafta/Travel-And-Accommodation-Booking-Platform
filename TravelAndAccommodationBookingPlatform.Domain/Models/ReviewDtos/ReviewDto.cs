namespace TravelAndAccommodationBookingPlatform.Domain.Models.ReviewDtos;

public class ReviewDto
{
    public string UserName { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; }
    public DateTime CreatedAt { get; set; }
}