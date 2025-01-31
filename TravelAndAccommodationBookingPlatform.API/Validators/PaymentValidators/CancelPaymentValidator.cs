using FluentValidation;
using TravelAndAccommodationBookingPlatform.Domain.Models.PaymentDtos;

namespace TravelAndAccommodationBookingPlatform.API.Validators.PaymentValidators;

public class CancelPaymentValidator : GenericValidator<CancelPaymentRequestDto>
{
    public CancelPaymentValidator()
    {
        RuleFor(x => x.PaymentId).NotEmpty().WithMessage("PaymentId is required");
    }
}