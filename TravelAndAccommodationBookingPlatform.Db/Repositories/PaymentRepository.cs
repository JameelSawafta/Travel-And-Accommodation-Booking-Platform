using Microsoft.EntityFrameworkCore;
using TravelAndAccommodationBookingPlatform.Db.DbContext;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Enums;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Repositories;

namespace TravelAndAccommodationBookingPlatform.Db.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly TravelAndAccommodationBookingPlatformDbContext _context;
    
    public PaymentRepository(TravelAndAccommodationBookingPlatformDbContext context)
    {
        _context = context;
    }

    public async Task<Payment?> GetPaymentWithBookingDetailsByIdAsync(Guid paymentId)
    {
        return await _context.Payments
            .Include(p => p.Booking)
            .ThenInclude(b => b.User)
            .Include(p => p.Booking)
            .ThenInclude(b => b.BookingDetails)
            .ThenInclude(bd => bd.Room)
            .FirstOrDefaultAsync(p => p.PaymentId == paymentId);
    }

    public async Task<Payment?> GetPaymentWithBookingByIdAsync(Guid paymentId)
    {
        return await _context.Payments
            .Include(p => p.Booking)
            .ThenInclude(b => b.User)
            .FirstOrDefaultAsync(p => p.PaymentId == paymentId);
    }

    public async Task UpdatePaymentAsync(Payment payment)
    {
        _context.Payments.Update(payment);
        await _context.SaveChangesAsync();
    }
}