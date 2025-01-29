using TravelAndAccommodationBookingPlatform.Db.DbContext;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Repositories;

namespace TravelAndAccommodationBookingPlatform.Db.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly TravelAndAccommodationBookingPlatformDbContext _dbContext;
    
    public PaymentRepository(TravelAndAccommodationBookingPlatformDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task CreatePaymentAsync(Payment payment)
    {
        await _dbContext.Payments.AddAsync(payment);
        await _dbContext.SaveChangesAsync();
    }
}