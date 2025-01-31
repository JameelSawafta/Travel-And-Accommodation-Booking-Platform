using System.Reflection.Metadata;
using AutoMapper;
using TravelAndAccommodationBookingPlatform.Domain.Enums;
using TravelAndAccommodationBookingPlatform.Domain.Exceptions;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Services;
using TravelAndAccommodationBookingPlatform.Domain.Models.EmailDtos;
using TravelAndAccommodationBookingPlatform.Domain.Models.PaymentDtos;

namespace TravelAndAccommodationBookingPlatform.Domain.Services;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IPaymentGatewayService _paymentGatewayService;
    private readonly IInvoiceService _invoiceService;
    private readonly IEmailService _emailService;
    private readonly IMapper _mapper;

    public PaymentService(IPaymentRepository paymentRepository,
        IPaymentGatewayService paymentGatewayService, IInvoiceService invoiceService, IEmailService emailService, IMapper mapper)
    {
        _paymentRepository = paymentRepository;
        _paymentGatewayService = paymentGatewayService;
        _invoiceService = invoiceService;
        _emailService = emailService;
        _mapper = mapper;
    }

    public async Task<PaymentDto> GetPaymentWithBookingDetailsByIdAsync(Guid paymentId)
    {
        var payment = await _paymentRepository.GetPaymentWithBookingDetailsByIdAsync(paymentId);
        if (payment == null)
        {
            throw new NotFoundException("Payment not found.");
        }

        return _mapper.Map<PaymentDto>(payment);
    }

    public async Task<PaymentResponsetDto> ConfirmPaymentAsync(ConfirmPaymentRequestDto requestDto)
    {
        var payment = await _paymentRepository.GetPaymentWithBookingByIdAsync(requestDto.PaymentId);
        if (payment == null)
        {
            throw new NotFoundException("Payment not found.");
        }
        
        var emailDto = _mapper.Map<EmailDto>(payment);
        
        if (payment.Status == PaymentStatus.Success)
        {
            throw new ConflictException("Payment already confirmed.");
        }

        await _paymentGatewayService.ExecutePaymentAsync(payment.TransactionID, requestDto.PayerId);

        payment.Booking.Status = BookingStatus.Confirmed;
        payment.Status = PaymentStatus.Success;
        await _paymentRepository.UpdatePaymentAsync(payment);
        await _emailService.SendEmailAsync(emailDto);
        return _mapper.Map<PaymentResponsetDto>(payment);
    }

    public async Task CancelPaymentAsync(CancelPaymentRequestDto requestDto)
    {
        var payment = await _paymentRepository.GetPaymentWithBookingByIdAsync(requestDto.PaymentId);
        if (payment == null)
        {
            throw new NotFoundException("Payment not found.");
        }
        
        if (payment.Status == PaymentStatus.Failed)
        {
            throw new ConflictException("Payment already cancelled.");
        }

        payment.Booking.Status = BookingStatus.Cancelled;
        payment.Status = PaymentStatus.Failed;
        await _paymentRepository.UpdatePaymentAsync(payment);
    }

    public async Task<byte[]> GeneratePaymentInvoiceAsync(Guid paymentId)
    {
        var payment = await _paymentRepository.GetPaymentWithBookingDetailsByIdAsync(paymentId);
        if (payment == null)
        {
            throw new NotFoundException("Payment not found.");
        }

        if (payment.Status != PaymentStatus.Success)
        {
            throw new ConflictException("Payment not confirmed.");
        }

        var paymentDto = _mapper.Map<PaymentDto>(payment);

        var pdfBytes = _invoiceService.GenerateInvoiceAsync(paymentDto);

        return pdfBytes;
    }
}