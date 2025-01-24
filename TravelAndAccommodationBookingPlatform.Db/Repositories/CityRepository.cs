using Microsoft.EntityFrameworkCore;
using TravelAndAccommodationBookingPlatform.Db.DbContext;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Enums;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Services;

namespace TravelAndAccommodationBookingPlatform.Db.Repositories;

public class CityRepository : ICityRepository
{
    private readonly TravelAndAccommodationBookingPlatformDbContext _context;
    private readonly IPaginationService _paginationService;

    public CityRepository(TravelAndAccommodationBookingPlatformDbContext context, IPaginationService paginationService)
    {
        _context = context;
        _paginationService = paginationService;
    }

    public async Task<List<City>> GetTrendingDestinationsAsync(int count)
    {
        return await _context.Cities
            .Include(c => c.Hotels)
            .ThenInclude(h => h.Rooms)
            .ThenInclude(r => r.Bookings)
            .OrderByDescending(c => c.Hotels
                .Sum(h => h.Rooms
                    .Sum(r => r.Bookings.Count(b => b.Status == BookingStatus.Confirmed))))
            .Take(count)
            .ToListAsync();
    }

    public async Task<(IEnumerable<City> Items, int TotalCount)> GetAllCitiesAsync(int pageNumber, int pageSize)
    {
        var cities = _context.Cities.AsQueryable();
        var (paginatedCities, totalCount) = await _paginationService.PaginateAsync(cities, pageNumber, pageSize);
        return (paginatedCities, totalCount);
    }

    public async Task<City> GetCityByNameAsync(string cityName)
    {
        return await _context.Cities.FirstOrDefaultAsync(c => c.CityName.ToLower() == cityName.ToLower());
    }

    public async Task CreateCityAsync(City city)
    {
        await _context.Cities.AddAsync(city);
        await _context.SaveChangesAsync();
    }
}