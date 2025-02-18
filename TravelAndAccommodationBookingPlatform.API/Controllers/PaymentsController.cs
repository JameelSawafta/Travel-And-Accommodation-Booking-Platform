using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelAndAccommodationBookingPlatform.API.Validators.ModelsValidators.PaymentValidators;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Services;
using TravelAndAccommodationBookingPlatform.Domain.Models.PaymentDtos;

namespace TravelAndAccommodationBookingPlatform.API.Controllers;

[ApiController]
[Route("api/payments")]
[ApiVersion("1.0")]
[Authorize(Policy = "UserOrAdmin")]
public class PaymentsController : Controller
{
    private readonly IPaymentService _paymentService;
    
    public PaymentsController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }
    
    /// <summary>
    /// Get the payment with booking details
    /// </summary>
    /// <param name="paymentId"></param>
    /// <returns> the payment with booking details </returns>
    /// <response code="200">if the payment is found</response>
    /// <response code="401">if the user is not authenticated</response>
    /// <response code="404">if the payment id is not found</response>
    [HttpGet("{paymentId}")]
    public async Task<PaymentDto> GetPaymentWithBookingDetailsAsync(Guid paymentId)
    {
        return await _paymentService.GetPaymentWithBookingDetailsByIdAsync(paymentId);
    }
    
    /// <summary>
    /// Confirm the payment
    /// </summary>
    /// <param name="ConfirmPaymentRequestDto"> the payment confirmation request </param>
    /// <returns> the payment response </returns>
    /// <response code="200">if the payment is confirmed</response>
    /// <response code="401">if the user is not authenticated</response>
    /// <response code="404">if the booking id is not valid</response>
    /// <response code="409">if the payment is already confirmed</response>
    [HttpPost("confirm")]
    public async Task<PaymentResponsetDto> ConfirmPaymentAsync([FromBody] ConfirmPaymentRequestDto requestDto)
    {
        var validator = new ConfirmPaymentValidator();
        await validator.ValidateAndThrowCustomExceptionAsync(requestDto);
        return await _paymentService.ConfirmPaymentAsync(requestDto);
    }
    
    /// <summary>
    /// Cancel the payment
    /// </summary>
    /// <param name="CancelPaymentRequestDto"> the payment cancellation request </param>
    /// <response code="200">if the payment is cancelled</response>
    /// <response code="401">if the user is not authenticated</response>
    /// <response code="404">if the booking id is not valid</response>
    /// <response code="409">if the payment is already cancelled</response>
    [HttpPost("cancel")]
    public async Task<IActionResult> CancelPaymentAsync([FromBody] CancelPaymentRequestDto requestDto)
    {
        var validator = new CancelPaymentValidator();
        await validator.ValidateAndThrowCustomExceptionAsync(requestDto);
        await _paymentService.CancelPaymentAsync(requestDto);
        return Ok("Payment cancelled.");
    }
    
    /// <summary>
    /// Get all payments
    /// </summary>
    /// <param name="paymentId"> the payment id </param>
    /// <returns> pdf file of the payment </returns>
    /// <response code="200">if the payment pdf is generated</response>
    /// <response code="401">if the user is not authenticated</response>
    /// <response code="404">if the payment id is not valid</response>
    [HttpGet("{paymentId}/pdf")]
    public async Task<FileContentResult> DownloadPaymentPdf(Guid paymentId)
    {
        var pdfBytes = await _paymentService.GeneratePaymentInvoiceAsync(paymentId);
        return File(pdfBytes, "application/pdf", $"Payment_{paymentId}.pdf");
    }
}