namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Services;

public interface IPaginationService
{
    public Task<(IEnumerable<T> Items, int TotalCount)> PaginateAsync<T>(IQueryable<T> query, int pageSize, int pageNumber) where T : class;
}