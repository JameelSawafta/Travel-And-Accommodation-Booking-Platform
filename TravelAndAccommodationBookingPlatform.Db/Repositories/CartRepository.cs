using Microsoft.EntityFrameworkCore;
using TravelAndAccommodationBookingPlatform.Db.DbContext;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Services;

namespace TravelAndAccommodationBookingPlatform.Db.Repositories;

public class CartRepository : ICartRepository
{
    private readonly TravelAndAccommodationBookingPlatformDbContext _context;
    private readonly IPaginationService _paginationService;

    public CartRepository(TravelAndAccommodationBookingPlatformDbContext context, IPaginationService paginationService)
    {
        _paginationService = paginationService;
        _context = context;
    }

    public async Task AddToCartAsync(Cart cartItem)
    {
        _context.Carts.Add(cartItem);
        await _context.SaveChangesAsync();
    }

    public async Task<(IEnumerable<Cart> Items, int TotalCount)> GetCartItemsAsync(Guid userId, int pageNumber, int pageSize)
    {
        var cartItems = _context.Carts.AsQueryable();
        var (paginatedCartItems, totalCount) = await _paginationService.PaginateAsync(cartItems, pageSize, pageNumber);
        return (paginatedCartItems, totalCount);
    }


    public async Task RemoveFromCartAsync(Guid cartId)
    {
        var cartItem = await _context.Carts.FindAsync(cartId);
        if (cartItem != null)
        {
            _context.Carts.Remove(cartItem);
            await _context.SaveChangesAsync();
        }
    }

    public async Task ClearCartAsync(Guid userId)
    {
        var cartItems = await _context.Carts.Where(c => c.UserId == userId).ToListAsync();
        if (cartItems.Any())
        {
            _context.Carts.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> HasDateConflictAsync(Guid userId, Guid roomId, DateTime checkInDate, DateTime checkOutDate)
    {
        return await _context.Carts.AnyAsync(c =>
            c.UserId == userId &&
            c.RoomId == roomId &&
            !(c.CheckOutDate <= checkInDate || c.CheckInDate >= checkOutDate)
        );
    }

    public async Task<IEnumerable<Cart>> GetCartItemsByUserIdAsync(Guid userId)
    {
        return await _context.Carts.Where(c => c.UserId == userId).ToListAsync();
    }
}