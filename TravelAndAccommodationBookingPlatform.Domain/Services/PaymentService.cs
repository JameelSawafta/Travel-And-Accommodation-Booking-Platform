using TravelAndAccommodationBookingPlatform.Domain.Enums;
using TravelAndAccommodationBookingPlatform.Domain.Exceptions;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Services;
using TravelAndAccommodationBookingPlatform.Domain.Models.PaymentDtos;

namespace TravelAndAccommodationBookingPlatform.Domain.Services;

public class PaymentService : IPaymentService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IPaymentGatewayService _paymentGatewayService;
    
    public PaymentService(IBookingRepository bookingRepository, IPaymentGatewayService paymentGatewayService)
    {
        _bookingRepository = bookingRepository;
        _paymentGatewayService = paymentGatewayService;
    }
    
    public async Task ConfirmPaymentAsync(ConfirmPaymentRequestDto requestDto)
    {
        var booking = await _bookingRepository.GetBookingByIdAsync(requestDto.BookingId);
        if (booking == null)
        {
            throw new NotFoundException("Booking not found.");
        }

        if (booking.Payment.Status == PaymentStatus.Success)
        {
            throw new ConflictException("Payment already confirmed.");
        }

        await _paymentGatewayService.ExecutePaymentAsync(requestDto.PaymentId, requestDto.PayerId);

        booking.Status = BookingStatus.Confirmed;
        booking.Payment.Status = PaymentStatus.Success;
        await _bookingRepository.UpdateBookingAsync(booking);
    }
    
    public async Task CancelPaymentAsync(CancelPaymentRequestDto requestDto)
    {
        var booking = await _bookingRepository.GetBookingByIdAsync(requestDto.BookingId);
        if (booking == null)
        {
            throw new NotFoundException("Booking not found.");
        }

        if (booking.Status == BookingStatus.Cancelled)
        {
            throw new ConflictException("Booking already cancelled.");
        }

        booking.Status = BookingStatus.Cancelled;
        booking.Payment.Status = PaymentStatus.Failed;
        await _bookingRepository.UpdateBookingAsync(booking);
    }
}