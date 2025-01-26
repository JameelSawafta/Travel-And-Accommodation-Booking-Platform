using Microsoft.EntityFrameworkCore;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Services;

namespace TravelAndAccommodationBookingPlatform.Db.DbServices;

public class PaginationService : IPaginationService
{
    public async Task<(IEnumerable<T> Items, int TotalCount)> PaginateAsync<T>(
        IQueryable<T> query, int pageSize, int pageNumber) where T : class
    {
        if (pageNumber <= 0 || pageSize <= 0)
        {
            var totalCount0 = await query.CountAsync();
            var items0 = await query
                .ToListAsync();
            return (items0, totalCount0);
        }
            

        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();

        return (items, totalCount);
    }
}