namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Services;


public interface IPaymentGatewayService
{
    Task<(string approvalUrl, string transactionId)> CreatePaymentAsync(decimal amount, string currency);
    Task ExecutePaymentAsync(string paymentId, string payerId);
}