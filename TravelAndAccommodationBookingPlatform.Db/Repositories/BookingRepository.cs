using Microsoft.EntityFrameworkCore;
using TravelAndAccommodationBookingPlatform.Db.DbContext;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Enums;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Services;

namespace TravelAndAccommodationBookingPlatform.Db.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly TravelAndAccommodationBookingPlatformDbContext _context;

    public BookingRepository(TravelAndAccommodationBookingPlatformDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Hotel>> GetRecentlyVisitedHotelsAsync(Guid userId, int count)
    {
        var hotelIds = await _context.BookingDetails
            .Include(bd => bd.Booking) 
            .Include(bd => bd.Room)
            .Where(bd => bd.Booking.UserId == userId && bd.CheckOutDate <= DateTime.Now && bd.Booking.Status == BookingStatus.Confirmed)
            .OrderByDescending(bd => bd.CheckOutDate)
            .Select(bd => bd.Room.HotelId)
            .Distinct()
            .Take(count)
            .ToListAsync();

        var hotels = await _context.Hotels
            .Where(h => hotelIds.Contains(h.HotelId)) 
            .Include(h => h.City) 
            .Include(h => h.Rooms) 
            .ToListAsync();

        return hotels;
    }

    public Task AddBookingAsync(Booking? booking)
    {
        _context.Bookings.Add(booking);
        return _context.SaveChangesAsync();
    }

    public async Task<Booking?> GetBookingByIdAsync(Guid bookingId)
    {
        return await _context.Bookings
            .Include(b => b.Payment)
            .FirstOrDefaultAsync(b => b.BookingId == bookingId);
    }

    public async Task UpdateBookingAsync(Booking booking)
    {
        _context.Bookings.Update(booking);
        await _context.SaveChangesAsync();
    }
}