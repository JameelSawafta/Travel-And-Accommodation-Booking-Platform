using TravelAndAccommodationBookingPlatform.Domain.Models.PaymentDtos;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Services;

public interface IInvoiceService
{
    byte[] GenerateInvoiceAsync(PaymentDto paymentDto);
}