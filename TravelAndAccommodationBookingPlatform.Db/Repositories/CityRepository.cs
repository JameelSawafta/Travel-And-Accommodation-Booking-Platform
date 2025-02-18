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
            .ThenInclude(r => r.BookingDetails)
            .ThenInclude(bd => bd.Booking)
            .OrderByDescending(c => c.Hotels
                .Sum(h => h.Rooms
                    .Sum(r => r.BookingDetails
                        .Count(bd => bd.Booking.Status == BookingStatus.Confirmed))))
            .Take(count)
            .ToListAsync();
    }

    public async Task<(IEnumerable<City> Items, int TotalCount)> GetAllCitiesAsync(int pageNumber, int pageSize)
    {
        var cities = _context.Cities.AsQueryable();
        var (paginatedCities, totalCount) = await _paginationService.PaginateAsync(cities, pageSize, pageNumber);
        return (paginatedCities, totalCount);
    }

    public async Task<City> GetCityByNameAsync(string cityName)
    {
        return await _context.Cities.FirstOrDefaultAsync(c => c.CityName.ToLower() == cityName.ToLower());
    }

    public async Task<City> GetCityByIdAsync(Guid cityId)
    {
        return await _context.Cities.FirstOrDefaultAsync(c => c.CityId == cityId);
    }

    public async Task CreateCityAsync(City city)
    {
        await _context.Cities.AddAsync(city);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCityAsync(City city)
    {
        _context.Cities.Update(city);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCityAsync(Guid cityId)
    {
        var city = await GetCityByIdAsync(cityId);
        if (city != null)
        {
            _context.Cities.Remove(city);
            await _context.SaveChangesAsync();
        }
    }
}