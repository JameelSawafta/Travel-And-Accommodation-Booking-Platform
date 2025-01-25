using Microsoft.EntityFrameworkCore;
using TravelAndAccommodationBookingPlatform.Db.DbContext;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Repositories;

namespace TravelAndAccommodationBookingPlatform.Db.Repositories;

public class OwnerRepository : IOwnerRepository
{
    private readonly TravelAndAccommodationBookingPlatformDbContext _context;
    
    public OwnerRepository(TravelAndAccommodationBookingPlatformDbContext context)
    {
        _context = context;
    }

    public Task<Owner> GetOwnerByIdAsync(Guid ownerId)
    {
        return _context.Owners.FirstOrDefaultAsync(o => o.OwnerId == ownerId);
    }
}