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
    private readonly IPaginationService _paginationService;

    public BookingRepository(TravelAndAccommodationBookingPlatformDbContext context, IPaginationService paginationService)
    {
        _context = context;
        _paginationService = paginationService;
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
}