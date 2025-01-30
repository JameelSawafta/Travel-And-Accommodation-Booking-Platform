using System.Reflection.Metadata;
using AutoMapper;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using TravelAndAccommodationBookingPlatform.Domain.Enums;
using TravelAndAccommodationBookingPlatform.Domain.Exceptions;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Services;
using TravelAndAccommodationBookingPlatform.Domain.Models.PaymentDtos;
using Document = QuestPDF.Fluent.Document;

namespace TravelAndAccommodationBookingPlatform.Domain.Services;

public class PaymentService : IPaymentService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IPaymentGatewayService _paymentGatewayService;
    private readonly IMapper _mapper;
    
    public PaymentService(IBookingRepository bookingRepository, IPaymentRepository paymentRepository, IPaymentGatewayService paymentGatewayService, IMapper mapper)
    {
        _bookingRepository = bookingRepository;
        _paymentRepository = paymentRepository;
        _paymentGatewayService = paymentGatewayService;
        _mapper = mapper;
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

    public async Task<byte[]> GeneratePaymentPdfAsync(Guid paymentId)
    {
        var payment = await _paymentRepository.GetPaymentByIdAsync(paymentId);
        if (payment == null)
        {
            throw new NotFoundException("Payment not found.");
        }
        var paymentDto = _mapper.Map<PaymentDto>(payment);
        
        var pdfBytes = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(12));

                page.Header()
                    .Text("Payment Receipt")
                    .SemiBold().FontSize(18).AlignCenter();

                page.Content()
                    .PaddingVertical(1, Unit.Centimetre)
                    .Column(column =>
                    {
                        column.Item().Text($"Payment ID: {paymentDto.PaymentId}");
                        column.Item().Text($"Amount: {paymentDto.Amount:C}");
                        column.Item().Text($"Payment Method: {paymentDto.PaymentMethod}");
                        column.Item().Text($"Transaction ID: {paymentDto.TransactionID}");
                        column.Item().Text($"Transaction Date: {paymentDto.TransactionDate:dd/MM/yyyy HH:mm}");
                        column.Item().Text($"Status: {paymentDto.Status}");

                        
                        column.Item().Text("Booking Details:").Bold();
                        foreach (var detail in paymentDto.Booking.BookingDetails)
                        {
                            column.Item().Text($"Room: {detail.Room.RoomNumber} - {detail.Room.RoomType}");
                            column.Item().Text($"Check-In: {detail.CheckInDate:dd/MM/yyyy}");
                            column.Item().Text($"Check-Out: {detail.CheckOutDate:dd/MM/yyyy}");
                            column.Item().Text($"Price: {detail.Price:C}");
                        }
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Page ");
                        x.CurrentPageNumber();
                    });
            });
        }).GeneratePdf();

        return pdfBytes;
    }
}