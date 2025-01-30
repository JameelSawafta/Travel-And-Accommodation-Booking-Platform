using AutoMapper;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Enums;
using TravelAndAccommodationBookingPlatform.Domain.Exceptions;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Services;
using TravelAndAccommodationBookingPlatform.Domain.Models;
using TravelAndAccommodationBookingPlatform.Domain.Models.HotelDtos;

namespace TravelAndAccommodationBookingPlatform.Domain.Services;


public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly ICartRepository _cartRepository;
    private readonly IUserRepository _userRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IPaymentGatewayService _paymentGatewayService;
    private readonly IMapper _mapper;

    public BookingService(IBookingRepository bookingRepository, ICartRepository cartRepository, IUserRepository userRepository,
        IRoomRepository roomRepository, IPaymentGatewayService paymentGatewayService, IMapper mapper)
    {
        _bookingRepository = bookingRepository;
        _cartRepository = cartRepository;
        _userRepository = userRepository;
        _roomRepository = roomRepository;
        _paymentGatewayService = paymentGatewayService;
        _mapper = mapper;
    }
    
    public async Task<List<RecentlyVisitedHotelDto>> GetRecentlyVisitedHotelsAsync(Guid userId, int count)
    {
        var hotels = await _bookingRepository.GetRecentlyVisitedHotelsAsync(userId, count);

        if (!hotels.Any())
        {
            throw new NotFoundException("No recently visited hotels found.");
        }

        return _mapper.Map<List<RecentlyVisitedHotelDto>>(hotels);
    }

    public async Task<CheckoutDto> CreateBookingFromCartAsync(CheckoutRequestDto requestDto)
    {
        var user = await _userRepository.GetUserByIdAsync(requestDto.UserId);
        if (user == null)
        {
            throw new NotFoundException("User not found.");
        }

        var cartItems = await _cartRepository.GetCartItemsByUserIdAsync(requestDto.UserId);
        if (!cartItems.Any())
        {
            throw new NotFoundException("No items in the cart.");
        }

        decimal totalAmount = 0;
        foreach (var cartItem in cartItems)
        {
            var isRoomAvailable = await _roomRepository.GetRoomIfAvailableAsync(
                cartItem.RoomId,
                cartItem.CheckInDate,
                cartItem.CheckOutDate
            );

            if (isRoomAvailable == null)
            {
                throw new ConflictException($"Room {cartItem.RoomId} is not available for the selected dates.");
            }
            totalAmount += cartItem.Price;
        }
        
        var (approvalUrl,transactionId, paymentMethod) = await _paymentGatewayService.CreatePaymentAsync(totalAmount, "USD");
        
        var booking = new Booking
        {
            UserId = requestDto.UserId,
            Status = BookingStatus.Pending,
            BookingDetails = cartItems.Select(cartItem => new BookingDetail
            {
                RoomId = cartItem.RoomId,
                CheckInDate = cartItem.CheckInDate,
                CheckOutDate = cartItem.CheckOutDate,
                Price = cartItem.Price
            }).ToList(),
            Payment = new Payment
            {
                TransactionID = transactionId,
                Amount = totalAmount,
                PaymentMethod = paymentMethod,
                Status = PaymentStatus.Pending
            }
        };
        
        await _bookingRepository.AddBookingAsync(booking);
        await _cartRepository.ClearCartAsync(requestDto.UserId);

        return new CheckoutDto
        {
            approvalUrl = approvalUrl,
            PaymentId = booking.Payment.PaymentId
        };
    }
}