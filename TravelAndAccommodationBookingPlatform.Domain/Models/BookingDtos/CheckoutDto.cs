namespace TravelAndAccommodationBookingPlatform.Domain.Models;

public class CheckoutDto
{
    /// <summary>
    /// the approval url for the payment
    /// </summary>
    public string approvalUrl { get; set; }
    /// <summary>
    /// the Payment id for the payment
    /// </summary>
    public Guid PaymentId { get; set; }
}