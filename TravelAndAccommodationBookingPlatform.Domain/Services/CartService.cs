using AutoMapper;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Exceptions;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Services;
using TravelAndAccommodationBookingPlatform.Domain.Models.CartDtos;
using TravelAndAccommodationBookingPlatform.Domain.Models.Common;

namespace TravelAndAccommodationBookingPlatform.Domain.Services;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;
    private readonly IUserRepository _userRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IMapper _mapper;
    
    public CartService(ICartRepository cartRepository, IUserRepository userRepository, IRoomRepository roomRepository, IMapper mapper)
    {
        _cartRepository = cartRepository;
        _userRepository = userRepository;
        _roomRepository = roomRepository;
        _mapper = mapper;
    }
    
    public async Task AddToCartAsync(AddToCartDto cartDto)
    {
        var user = await _userRepository.GetUserByIdAsync(cartDto.UserId);
        if (user == null)
        {
            throw new NotFoundException("User not found.");
        }
        var room = await _roomRepository.GetRoomIfAvailableAsync(cartDto.RoomId, cartDto.CheckInDate, cartDto.CheckOutDate);
        if (room == null)
        {
            throw new NotFoundException("Room not found or not available.");
        }
        
        var hasDateConflict = await _cartRepository.HasDateConflictAsync(cartDto.UserId, cartDto.RoomId, cartDto.CheckInDate, cartDto.CheckOutDate);
        if (hasDateConflict)
        {
            throw new ConflictException("Date conflict with existing cart items.");
        }
        var cartItem = _mapper.Map<Cart>(cartDto);
        
        var numberOfNights = (cartDto.CheckOutDate - cartDto.CheckInDate).Days;

        var highestDiscount = room.RoomDiscounts
            .Select(rd => rd.Discount)
            .Where(d => d.ValidFrom <= cartDto.CheckInDate && d.ValidTo >= cartDto.CheckOutDate)
            .OrderByDescending(d => d.DiscountPercentageValue)
            .FirstOrDefault();
        
        if (highestDiscount != null)
        {
            cartItem.Price = room.PricePerNight * (1 - (decimal)highestDiscount.DiscountPercentageValue ) * numberOfNights;
        }
        else
        {
            cartItem.Price = room.PricePerNight * numberOfNights;
        }
        await _cartRepository.AddToCartAsync(cartItem);
    }

    public async Task<PaginatedList<CartDto>> GetCartItemsAsync(Guid userId, int pageNumber, int pageSize)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user == null)
        {
            throw new NotFoundException("User not found.");
        }
        var (cartItems, totalCount) = await _cartRepository.GetCartItemsAsync(userId, pageNumber, pageSize);
        var cartDtos = _mapper.Map<IEnumerable<CartDto>>(cartItems);
        var pageData = new PageData(totalCount, pageSize, pageNumber);
        return new PaginatedList<CartDto>(cartDtos.ToList(), pageData);
    }

    public async Task RemoveFromCartAsync(Guid cartId)
    {
        await _cartRepository.RemoveFromCartAsync(cartId);
    }

    public async Task ClearCartAsync(Guid userId)
    {
        await _cartRepository.ClearCartAsync(userId);
    }
}