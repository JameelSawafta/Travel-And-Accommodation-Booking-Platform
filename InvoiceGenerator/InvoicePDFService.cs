using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Services;

using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using TravelAndAccommodationBookingPlatform.Domain.Models.PaymentDtos;
using Document = QuestPDF.Fluent.Document;


namespace InvoiceGenerator;

public class InvoicePDFService : IInvoiceService
{
    public byte[] GenerateInvoiceAsync(PaymentDto paymentDto)
    {
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
                    .Text("Thank you for choosing our platform! Contact us at support@tabp.com for any queries.")
                    .FontSize(10);
            });
        }).GeneratePdf();
        
        return pdfBytes;
    }
}