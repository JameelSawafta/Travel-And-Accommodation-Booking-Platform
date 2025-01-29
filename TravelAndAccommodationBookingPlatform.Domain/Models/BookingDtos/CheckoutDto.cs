namespace TravelAndAccommodationBookingPlatform.Domain.Models;

public class CheckoutDto
{
    /// <summary>
    /// the approval url for the payment
    /// </summary>
    public string approvalUrl { get; set; }
    /// <summary>
    /// the booking id
    /// </summary>
    public string BookingId { get; set; }
}