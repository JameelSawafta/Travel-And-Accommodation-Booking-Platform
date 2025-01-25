using Microsoft.EntityFrameworkCore;
using TravelAndAccommodationBookingPlatform.Db.DbContext;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Services;

namespace TravelAndAccommodationBookingPlatform.Db.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly TravelAndAccommodationBookingPlatformDbContext _context;
    private readonly IPaginationService _paginationService;

    public RoomRepository(TravelAndAccommodationBookingPlatformDbContext context, IPaginationService paginationService)
    {
        _context = context;
        _paginationService = paginationService;
    }
    
    public async Task<(IEnumerable<Room> Items, int TotalCount)> GetAllRoomsAsync(int pageNumber, int pageSize)
    {
        var rooms = _context.Rooms.AsQueryable();
        var (paginatedRooms, totalCount) = await _paginationService.PaginateAsync(rooms, pageSize, pageNumber);
        return (paginatedRooms, totalCount);
    }

    public async Task<Room> GetRoomByIdAsync(Guid roomId)
    {
        return await _context.Rooms.FirstOrDefaultAsync(r => r.RoomId == roomId);
    }

    public async Task CreateRoomAsync(Room room)
    {
        await _context.Rooms.AddAsync(room);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateRoomAsync(Room room)
    {
        _context.Rooms.Update(room);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteRoomAsync(Guid roomId)
    {
        var room = await GetRoomByIdAsync(roomId);
        _context.Rooms.Remove(room);
        await _context.SaveChangesAsync();
    }
}