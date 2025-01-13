namespace TravelAndAccommodationBookingPlatform.Domain.Models.SearchDtos;

public class SearchRequestDto
{
    public string Query { get; set; }
    public DateTime CheckInDate { get; set; } = DateTime.Today;
    public DateTime CheckOutDate { get; set; } = DateTime.Today.AddDays(1);
    public int Adults { get; set; } = 2;
    public int Children { get; set; } = 0;
    public int Rooms { get; set; } = 1;
}