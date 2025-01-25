using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelAndAccommodationBookingPlatform.API.Validators.RoomValidators;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Services;
using TravelAndAccommodationBookingPlatform.Domain.Models.Common;
using TravelAndAccommodationBookingPlatform.Domain.Models.RoomDtos;

namespace TravelAndAccommodationBookingPlatform.API.Controllers;

[ApiController]
[Route("api/rooms")]
[ApiVersion("1.0")]
[Authorize(Policy = "AdminOnly")]
public class RoomsController : Controller
{
    private readonly IRoomService _roomService;
    
    public RoomsController(IRoomService roomService)
    {
        _roomService = roomService;
    }
    
    /// <summary>
    /// Retrieves a paginated list of all rooms.
    /// </summary>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="pageNumber">The page number to retrieve.</param>
    /// <returns>A paginated list of rooms.</returns>
    /// <response code="200">Returns the paginated list of rooms.</response>
    /// <response code="400">If the page number or page size is invalid.</response>
    /// <response code="401">If the user is not authenticated.</response>
    /// <response code="403">If the user is not authorized.</response>
    /// <response code="404">If no rooms are found.</response>
    [HttpGet]
    public async Task<PaginatedList<RoomDto>> GetRoomsAsync(int pageSize, int pageNumber)
    {
        return await _roomService.GetAllRoomsAsync(pageNumber, pageSize);
    }
    
    /// <summary>
    /// Retrieves a room by ID.
    /// </summary>
    /// <param name="roomId">The ID of the room.</param>
    /// <returns>A room.</returns>
    /// <response code="200">Returns the room.</response>
    /// <response code="401">If the user is not authenticated.</response>
    /// <response code="403">If the user is not authorized.</response>
    /// <response code="404">If the room is not found.</response>
    [HttpGet("{roomId}")]
    public async Task<RoomDto> GetRoomByIdAsync(Guid roomId)
    {
        return await _roomService.GetRoomByIdAsync(roomId);
    }
    
    /// <summary>
    /// Creates a new room.
    /// </summary>
    /// <param name="roomDto">The room to create.</param>
    /// <returns>A response with status code 201 (Created).</returns>
    /// <response code="201">Returns the created room.</response>
    /// <response code="400">If the room is invalid.</response>
    /// <response code="401">If the user is not authenticated.</response>
    /// <response code="403">If the user is not authorized.</response>
    /// <response code="409">If the room already exists.</response>
    [HttpPost]
    public async Task<IActionResult> CreateRoomAsync(CreateRoomDto roomDto)
    {
        var validator = new CreateRoomValidator();
        await validator.ValidateAndThrowCustomExceptionAsync(roomDto);
        await _roomService.CreateRoomAsync(roomDto);
        return Created();
    }
    
    /// <summary>
    /// Updates a room.
    /// </summary>
    /// <param name="roomId">The ID of the room to update.</param>
    /// <param name="roomDto">The room to update.</param>
    /// <returns>A response with status code 204 (No Content).</returns>
    /// <response code="200">Returns the updated room.</response>
    /// <response code="400">If the room is invalid.</response>
    /// <response code="401">If the user is not authenticated.</response>
    /// <response code="403">If the user is not authorized.</response>
    /// <response code="404">If the room is not found.</response>
    [HttpPut("{roomId}")]
    public async Task<IActionResult> UpdateRoomAsync(Guid roomId, UpdateRoomDto roomDto)
    {
        var validator = new UpdateRoomValidator();
        await validator.ValidateAndThrowCustomExceptionAsync(roomDto);
        await _roomService.UpdateRoomAsync(roomId, roomDto);
        return NoContent();
    }
    
    /// <summary>
    /// Deletes a room.
    /// </summary>
    /// <param name="roomId">The ID of the room to delete.</param>
    /// <returns>A response with status code 204 (No Content).</returns>
    /// <response code="204">Returns a response with status code 204 (No Content).</response>
    /// <response code="401">If the user is not authenticated.</response>
    /// <response code="403">If the user is not authorized.</response>
    /// <response code="404">If the room is not found.</response>
    [HttpDelete("{roomId}")]
    public async Task<IActionResult> DeleteRoomAsync(Guid roomId)
    {
        await _roomService.DeleteRoomAsync(roomId);
        return NoContent();
    }
}