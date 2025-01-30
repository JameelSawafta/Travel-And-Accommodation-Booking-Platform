namespace TravelAndAccommodationBookingPlatform.Domain.Models.PaymentDtos;

public class PaymentDto
{
    public Guid PaymentId { get; set; }
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; }
    public string TransactionID { get; set; }
    public DateTime TransactionDate { get; set; }
    public string Status { get; set; }
    public BookingDto Booking { get; set; }
}