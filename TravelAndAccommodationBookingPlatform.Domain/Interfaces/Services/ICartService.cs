using TravelAndAccommodationBookingPlatform.Domain.Models.CartDtos;
using TravelAndAccommodationBookingPlatform.Domain.Models.Common;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Services;

public interface ICartService
{
    Task AddToCartAsync(AddToCartDto cartDto);
    Task<PaginatedList<CartDto>> GetCartItemsAsync(Guid userId, int pageNumber, int pageSize);
    Task RemoveFromCartAsync(Guid cartId);
    Task ClearCartAsync(Guid userId);
}