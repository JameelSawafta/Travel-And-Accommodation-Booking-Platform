using Microsoft.EntityFrameworkCore;
using TravelAndAccommodationBookingPlatform.Db.DbContext;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Repositories;

namespace TravelAndAccommodationBookingPlatform.Db.Repositories;

public class UserRepository : IUserRepository
{
    private readonly TravelAndAccommodationBookingPlatformDbContext _context;

    public UserRepository(TravelAndAccommodationBookingPlatformDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task CreateUserAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }
}