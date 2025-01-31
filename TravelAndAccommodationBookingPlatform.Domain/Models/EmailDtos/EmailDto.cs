namespace TravelAndAccommodationBookingPlatform.Domain.Models.EmailDtos;

public class EmailDto
{
    public string FirstName { get; set; }
    public Guid BookingId { get; set; }
    public decimal Amount { get; set; }
    public string ToEmail { get; set; }
}