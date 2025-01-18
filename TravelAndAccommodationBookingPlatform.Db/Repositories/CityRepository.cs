using Microsoft.EntityFrameworkCore;
using TravelAndAccommodationBookingPlatform.Db.DbContext;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Repositories;

namespace TravelAndAccommodationBookingPlatform.Db.Repositories;

public class CityRepository : ICityRepository
{
    private readonly TravelAndAccommodationBookingPlatformDbContext _context;

    public CityRepository(TravelAndAccommodationBookingPlatformDbContext context)
    {
        _context = context;
    }

    public async Task<List<City>> GetTrendingDestinationsAsync(int count)
    {
        return await _context.Cities
            .Include(c => c.Hotels)
            .ThenInclude(h => h.Rooms)
            .ThenInclude(r => r.Bookings)
            .OrderByDescending(c => c.Hotels
                .Sum(h => h.Rooms
                    .Sum(r => r.Bookings.Count)))
            .Take(count)
            .ToListAsync();
    }
}