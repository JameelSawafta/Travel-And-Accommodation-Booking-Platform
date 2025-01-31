using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelAndAccommodationBookingPlatform.API.Validators.AttributeValidators;
using TravelAndAccommodationBookingPlatform.API.Validators.ModelsValidators.CartValidators;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Services;
using TravelAndAccommodationBookingPlatform.Domain.Models.CartDtos;
using TravelAndAccommodationBookingPlatform.Domain.Models.Common;

namespace TravelAndAccommodationBookingPlatform.API.Controllers;

[ApiController]
[Route("api/carts")]
[ApiVersion("1.0")]
[Authorize(Policy = "UserOrAdmin")]
public class CartsController : Controller
{
    private readonly ICartService _cartService;
    
    public CartsController(ICartService cartService)
    {
        _cartService = cartService;
    }
    
    /// <summary>
    /// Adds an item to the cart.
    /// </summary>
    /// <param name="cartDto"> The cart item to add.</param>
    /// <returns> A response with status code 201 (Created).</returns>
    /// <response code="201">Returns a response with status code 201 (Created).</response>
    /// <response code="400">If the request is invalid.</response>
    /// <response code="401">If the user is not authenticated.</response>
    /// <response code="403">if the user id in the token does not match the user id in the request</response>
    /// <response code="404">If the user or room is not found.</response>
    /// <response code="409">If there is a date conflict with existing cart items.</response>
    [ValidateUserId]
    [HttpPost]
    public async Task<IActionResult> AddToCartAsync([FromBody] AddToCartDto cartDto)
    {
        var validator = new AddToCartValidator();
        await validator.ValidateAndThrowCustomExceptionAsync(cartDto);
        await _cartService.AddToCartAsync(cartDto);
        return Created();
    }
    
    /// <summary>
    /// Retrieves a paginated list of cart items.
    /// </summary>
    /// <param name="userId"> The ID of the user.</param>
    /// <param name="pageNumber"> The page number to retrieve.</param>
    /// <param name="pageSize"> The number of items per page.</param>
    /// <returns> A paginated list of cart items.</returns>
    /// <response code="200">Returns a paginated list of cart items.</response>
    /// <response code="400">If the page number or page size is invalid.</response>
    /// <response code="401">If the user is not authenticated.</response>
    /// <response code="403">if the user id in the token does not match the user id in the request</response>
    /// <response code="404">If the user is not found.</response>
    [ValidateUserId]
    [HttpGet("{userId}")]
    public async Task<PaginatedList<CartDto>> GetCartItemsAsync(Guid userId, [FromQuery] int pageNumber, [FromQuery] int pageSize)
    {
        return await _cartService.GetCartItemsAsync(userId, pageNumber, pageSize);
    }
    
    /// <summary>
    /// Removes an item from the cart.
    /// </summary>
    /// <param name="cartId"> The ID of the cart item.</param>
    /// <returns> A response with status code 204 (No Content).</returns>
    /// <response code="204">Returns a response with status code 204 (No Content).</response>
    /// <response code="401">If the user is not authenticated.</response>
    [HttpDelete("{cartId}")]
    public async Task<IActionResult> RemoveFromCartAsync(Guid cartId)
    {
        await _cartService.RemoveFromCartAsync(cartId);
        return NoContent();
    }
    
    /// <summary>
    /// Clears the cart.
    /// </summary>
    /// <param name="userId"> The ID of the user.</param>
    /// <returns> A response with status code 204 (No Content).</returns>
    /// <response code="204">Returns a response with status code 204 (No Content).</response>
    /// <response code="401">If the user is not authenticated.</response>
    /// <response code="403">if the user id in the token does not match the user id in the request</response>
    [ValidateUserId]
    [HttpDelete("clear/{userId}")]
    public async Task<IActionResult> ClearCartAsync(Guid userId)
    {
        await _cartService.ClearCartAsync(userId);
        return NoContent();
    }
}