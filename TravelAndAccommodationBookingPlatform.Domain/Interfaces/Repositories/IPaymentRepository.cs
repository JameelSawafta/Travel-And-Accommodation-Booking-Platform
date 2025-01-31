using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Repositories;

public interface IPaymentRepository
{
    Task<Payment?> GetPaymentWithBookingDetailsByIdAsync(Guid paymentId);
    Task<Payment?> GetPaymentWithBookingByIdAsync(Guid paymentId);
    Task UpdatePaymentAsync(Payment payment);
}