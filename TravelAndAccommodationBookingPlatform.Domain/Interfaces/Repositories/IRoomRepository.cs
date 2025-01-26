using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Repositories;

public interface IRoomRepository
{
    Task<(IEnumerable<Room> Items, int TotalCount)> GetAllRoomsAsync(int pageNumber, int pageSize);
    Task<Room> GetRoomByIdAsync(Guid roomId);
    Task<Room> GetRoomByHotelAndNumberAsync(Guid hotelId, string roomNumber);
    Task CreateRoomAsync(Room room);
    Task UpdateRoomAsync(Room room);
    Task DeleteRoomAsync(Guid roomId);
}