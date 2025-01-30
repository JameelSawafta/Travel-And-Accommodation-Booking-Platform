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

    public PaymentService(IBookingRepository bookingRepository, IPaymentRepository paymentRepository,
        IPaymentGatewayService paymentGatewayService, IMapper mapper)
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
                    .Row(row =>
                    {
                        row.RelativeItem().AlignCenter().Column(column =>
                        {
                            column.Item().Text("Travel & Accommodation Booking Platform").Bold().FontSize(16);
                            column.Item().Text("Payment Receipt").SemiBold().FontSize(14);
                        });
                    });

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

                        column.Item().PaddingTop(10).Text("Booking Details:").Bold();
                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(); // Room Number
                                columns.RelativeColumn(); // Room Type
                                columns.RelativeColumn(); // Check-In Date
                                columns.RelativeColumn(); // Check-Out Date
                                columns.RelativeColumn(); // Price
                            });

                            table.Header(header =>
                            {
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Room Number").Bold();
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Room Type").Bold();
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Check-In Date").Bold();
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Check-Out Date").Bold();
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Price").Bold();
                            });

                            foreach (var detail in paymentDto.Booking.BookingDetails)
                            {
                                table.Cell().Background(Colors.Grey.Lighten4).Padding(5).Text(detail.Room.RoomNumber);
                                table.Cell().Background(Colors.Grey.Lighten4).Padding(5).Text(detail.Room.RoomType);
                                table.Cell().Background(Colors.Grey.Lighten4).Padding(5)
                                    .Text(detail.CheckInDate.ToString("dd/MM/yyyy"));
                                table.Cell().Background(Colors.Grey.Lighten4).Padding(5)
                                    .Text(detail.CheckOutDate.ToString("dd/MM/yyyy"));
                                table.Cell().Background(Colors.Grey.Lighten4).Padding(5).Text($"{detail.Price:C}");
                            }
                        });
                        
                    });

                page.Footer()
                    .AlignCenter()
                    .Text("Thank you for choosing our platform! Contact us at support@TABP.com for any queries.")
                    .FontSize(10);
            });
        }).GeneratePdf();

        return pdfBytes;
    }
}