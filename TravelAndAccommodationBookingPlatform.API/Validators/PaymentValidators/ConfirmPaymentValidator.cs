using FluentValidation;
using TravelAndAccommodationBookingPlatform.Domain.Models.PaymentDtos;

namespace TravelAndAccommodationBookingPlatform.API.Validators.PaymentValidators;

public class ConfirmPaymentValidator : GenericValidator<ConfirmPaymentRequestDto>
{
    public ConfirmPaymentValidator()
    {
        RuleFor(x => x.PaymentId).NotEmpty().WithMessage("PaymentId is required");
        RuleFor(x => x.PayerId).NotEmpty().WithMessage("PayerId is required");
    }
}