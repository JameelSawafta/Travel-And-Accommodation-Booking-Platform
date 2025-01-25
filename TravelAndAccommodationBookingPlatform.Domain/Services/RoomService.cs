using AutoMapper;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Exceptions;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Services;
using TravelAndAccommodationBookingPlatform.Domain.Models.Common;
using TravelAndAccommodationBookingPlatform.Domain.Models.RoomDtos;

namespace TravelAndAccommodationBookingPlatform.Domain.Services;

public class RoomService : IRoomService
{
    private readonly IRoomRepository _roomRepository;
    private readonly IMapper _mapper;
    
    public RoomService(IRoomRepository roomRepository, IMapper mapper)
    {
        _roomRepository = roomRepository;
        _mapper = mapper;
    }
    
    
    public async Task<PaginatedList<RoomDto>> GetAllRoomsAsync(int pageNumber, int pageSize)
    {
        var (rooms, totalCount) = await _roomRepository.GetAllRoomsAsync(pageNumber, pageSize);
        var pageData = new PageData(totalCount, pageSize, pageNumber);
        var roomDtos = _mapper.Map<IEnumerable<RoomDto>>(rooms);
        return new PaginatedList<RoomDto>(roomDtos.ToList(), pageData);
    }

    public async Task<RoomDto> GetRoomByIdAsync(Guid roomId)
    {
        var room = await _roomRepository.GetRoomByIdAsync(roomId);
        if (room == null)
        {
            throw new NotFoundException("Room not found.");
        }
        return _mapper.Map<RoomDto>(room);
    }

    public async Task CreateRoomAsync(CreateRoomDto createRoomDto)
    {
        var room = _mapper.Map<Room>(createRoomDto);
        await _roomRepository.CreateRoomAsync(room);
    }

    public async Task UpdateRoomAsync(Guid roomId, UpdateRoomDto updateRoomDto)
    {
        var room = await _roomRepository.GetRoomByIdAsync(roomId);
        if (room == null)
        {
            throw new NotFoundException("Room not found.");
        }
        _mapper.Map(updateRoomDto, room);
        await _roomRepository.UpdateRoomAsync(room);
    }

    public async Task DeleteRoomAsync(Guid roomId)
    {
        var room = await _roomRepository.GetRoomByIdAsync(roomId);
        if (room == null)
        {
            throw new NotFoundException("Room not found.");
        }
        await _roomRepository.DeleteRoomAsync(roomId);
    }
}