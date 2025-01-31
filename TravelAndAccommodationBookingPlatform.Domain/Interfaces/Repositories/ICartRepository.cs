using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Repositories;

public interface ICartRepository
{
    Task AddToCartAsync(Cart cartItem);
    Task<(IEnumerable<Cart> Items, int TotalCount)> GetCartItemsAsync(Guid userId, int pageNumber, int pageSize);
    Task RemoveFromCartAsync(Guid cartId);
    Task ClearCartAsync(Guid userId);
    Task<bool> HasDateConflictAsync(Guid userId, Guid roomId, DateTime checkInDate, DateTime checkOutDate);
    Task<IEnumerable<Cart>> GetCartItemsByUserIdAsync(Guid userId);
}