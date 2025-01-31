using AutoMapper;
using Moq;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Enums;
using TravelAndAccommodationBookingPlatform.Domain.Exceptions;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Services;
using TravelAndAccommodationBookingPlatform.Domain.Models.EmailDtos;
using TravelAndAccommodationBookingPlatform.Domain.Models.PaymentDtos;
using TravelAndAccommodationBookingPlatform.Domain.Services;

namespace TravelAndAccommodationBookingPlatform.Tests.UnitTests.Services;

public class PaymentServiceTests
{
     private readonly Mock<IPaymentRepository> _mockPaymentRepository;
    private readonly Mock<IPaymentGatewayService> _mockPaymentGatewayService;
    private readonly Mock<IInvoiceService> _mockInvoiceService;
    private readonly Mock<IEmailService> _mockEmailService;
    private readonly Mock<IMapper> _mockMapper;
    private readonly PaymentService _paymentService;

    public PaymentServiceTests()
    {
        _mockPaymentRepository = new Mock<IPaymentRepository>();
        _mockPaymentGatewayService = new Mock<IPaymentGatewayService>();
        _mockInvoiceService = new Mock<IInvoiceService>();
        _mockEmailService = new Mock<IEmailService>();
        _mockMapper = new Mock<IMapper>();
        _paymentService = new PaymentService(
            _mockPaymentRepository.Object,
            _mockPaymentGatewayService.Object,
            _mockInvoiceService.Object,
            _mockEmailService.Object,
            _mockMapper.Object
        );
    }

    [Fact]
    public async Task ConfirmPaymentAsync_ShouldConfirmPayment_WhenPaymentExistsAndNotConfirmed()
    {
        
        var paymentId = Guid.NewGuid();
        var requestDto = new ConfirmPaymentRequestDto
        {
            PaymentId = paymentId,
            PayerId = "payer123"
        };

        var payment = new Payment
        {
            PaymentId = paymentId,
            Status = PaymentStatus.Pending,
            TransactionID = "txn123",
            Booking = new Booking
            {
                Status = BookingStatus.Pending
            }
        };

        var emailDto = new EmailDto();
        var paymentResponseDto = new PaymentResponsetDto();

        _mockPaymentRepository.Setup(repo => repo.GetPaymentWithBookingByIdAsync(paymentId))
            .ReturnsAsync(payment);
        _mockMapper.Setup(mapper => mapper.Map<EmailDto>(payment))
            .Returns(emailDto);
        _mockMapper.Setup(mapper => mapper.Map<PaymentResponsetDto>(payment))
            .Returns(paymentResponseDto);

        
        var result = await _paymentService.ConfirmPaymentAsync(requestDto);

        
        Assert.NotNull(result);
        Assert.Equal(PaymentStatus.Success, payment.Status);
        Assert.Equal(BookingStatus.Confirmed, payment.Booking.Status);
        _mockPaymentGatewayService.Verify(gateway => gateway.ExecutePaymentAsync("txn123", "payer123"), Times.Once);
        _mockEmailService.Verify(email => email.SendEmailAsync(emailDto), Times.Once);
        _mockPaymentRepository.Verify(repo => repo.UpdatePaymentAsync(payment), Times.Once);
    }

    [Fact]
    public async Task ConfirmPaymentAsync_ShouldThrowNotFoundException_WhenPaymentDoesNotExist()
    {
        
        var paymentId = Guid.NewGuid();
        var requestDto = new ConfirmPaymentRequestDto
        {
            PaymentId = paymentId,
            PayerId = "payer123"
        };

        _mockPaymentRepository.Setup(repo => repo.GetPaymentWithBookingByIdAsync(paymentId))
            .ReturnsAsync((Payment)null);

        
        await Assert.ThrowsAsync<NotFoundException>(() => _paymentService.ConfirmPaymentAsync(requestDto));
    }

    [Fact]
    public async Task ConfirmPaymentAsync_ShouldThrowConflictException_WhenPaymentAlreadyConfirmed()
    {
        
        var paymentId = Guid.NewGuid();
        var requestDto = new ConfirmPaymentRequestDto
        {
            PaymentId = paymentId,
            PayerId = "payer123"
        };

        var payment = new Payment
        {
            PaymentId = paymentId,
            Status = PaymentStatus.Success,
            Booking = new Booking
            {
                Status = BookingStatus.Confirmed
            }
        };

        _mockPaymentRepository.Setup(repo => repo.GetPaymentWithBookingByIdAsync(paymentId))
            .ReturnsAsync(payment);

        
        await Assert.ThrowsAsync<ConflictException>(() => _paymentService.ConfirmPaymentAsync(requestDto));
    }

    [Fact]
    public async Task CancelPaymentAsync_ShouldCancelPayment_WhenPaymentExistsAndNotFailed()
    {
        
        var paymentId = Guid.NewGuid();
        var requestDto = new CancelPaymentRequestDto
        {
            PaymentId = paymentId
        };

        var payment = new Payment
        {
            PaymentId = paymentId,
            Status = PaymentStatus.Pending,
            Booking = new Booking
            {
                Status = BookingStatus.Pending
            }
        };

        _mockPaymentRepository.Setup(repo => repo.GetPaymentWithBookingByIdAsync(paymentId))
            .ReturnsAsync(payment);

        
        await _paymentService.CancelPaymentAsync(requestDto);

        
        Assert.Equal(PaymentStatus.Failed, payment.Status);
        Assert.Equal(BookingStatus.Cancelled, payment.Booking.Status);
        _mockPaymentRepository.Verify(repo => repo.UpdatePaymentAsync(payment), Times.Once);
    }

    [Fact]
    public async Task CancelPaymentAsync_ShouldThrowNotFoundException_WhenPaymentDoesNotExist()
    {
        
        var paymentId = Guid.NewGuid();
        var requestDto = new CancelPaymentRequestDto
        {
            PaymentId = paymentId
        };

        _mockPaymentRepository.Setup(repo => repo.GetPaymentWithBookingByIdAsync(paymentId))
            .ReturnsAsync((Payment)null);

        
        await Assert.ThrowsAsync<NotFoundException>(() => _paymentService.CancelPaymentAsync(requestDto));
    }

    [Fact]
    public async Task CancelPaymentAsync_ShouldThrowConflictException_WhenPaymentAlreadyFailed()
    {
        
        var paymentId = Guid.NewGuid();
        var requestDto = new CancelPaymentRequestDto
        {
            PaymentId = paymentId
        };

        var payment = new Payment
        {
            PaymentId = paymentId,
            Status = PaymentStatus.Failed,
            Booking = new Booking
            {
                Status = BookingStatus.Cancelled
            }
        };

        _mockPaymentRepository.Setup(repo => repo.GetPaymentWithBookingByIdAsync(paymentId))
            .ReturnsAsync(payment);

        
        await Assert.ThrowsAsync<ConflictException>(() => _paymentService.CancelPaymentAsync(requestDto));
    }

    [Fact]
    public async Task GeneratePaymentInvoiceAsync_ShouldReturnPdfBytes_WhenPaymentExistsAndIsSuccessful()
    {
        
        var paymentId = Guid.NewGuid();
        var payment = new Payment
        {
            PaymentId = paymentId,
            Status = PaymentStatus.Success,
            Booking = new Booking
            {
                BookingDetails = new List<BookingDetail>
                {
                    new BookingDetail
                    {
                        Room = new Room()
                    }
                }
            }
        };

        var paymentDto = new PaymentDto();
        var pdfBytes = new byte[] { 0x01, 0x02, 0x03 };

        _mockPaymentRepository.Setup(repo => repo.GetSuccessPaymentWithBookingDetailsByIdAsync(paymentId))
            .ReturnsAsync(payment);
        _mockMapper.Setup(mapper => mapper.Map<PaymentDto>(payment))
            .Returns(paymentDto);
        _mockInvoiceService.Setup(invoice => invoice.GenerateInvoiceAsync(paymentDto))
            .Returns(pdfBytes);

        
        var result = await _paymentService.GeneratePaymentInvoiceAsync(paymentId);

        
        Assert.NotNull(result);
        Assert.Equal(pdfBytes, result);
    }

    [Fact]
    public async Task GeneratePaymentInvoiceAsync_ShouldThrowNotFoundException_WhenPaymentDoesNotExist()
    {
        
        var paymentId = Guid.NewGuid();
        _mockPaymentRepository.Setup(repo => repo.GetSuccessPaymentWithBookingDetailsByIdAsync(paymentId))
            .ReturnsAsync((Payment)null);

        
        await Assert.ThrowsAsync<NotFoundException>(() => _paymentService.GeneratePaymentInvoiceAsync(paymentId));
    }
}