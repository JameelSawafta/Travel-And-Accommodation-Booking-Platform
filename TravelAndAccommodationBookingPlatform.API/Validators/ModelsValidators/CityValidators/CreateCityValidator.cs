using FluentValidation;
using TravelAndAccommodationBookingPlatform.Domain.Models.CityDtos;

namespace TravelAndAccommodationBookingPlatform.API.Validators.ModelsValidators.CityValidators;

public class CreateCityValidator : GenericValidator<CreateCityDto>
{
    public CreateCityValidator()
    {
        RuleFor(x => x.CityName)
            .NotEmpty()
            .WithMessage("City name is required.")
            .MaximumLength(50)
            .WithMessage("City name must not exceed 50 characters.");

        RuleFor(x => x.Country)
            .NotEmpty()
            .WithMessage("Country is required.")
            .MaximumLength(50)
            .WithMessage("Country must not exceed 50 characters.");

        RuleFor(x => x.PostCode)
            .MaximumLength(10)
            .WithMessage("Post code must not exceed 10 characters.");

        RuleFor(x => x.ThumbnailUrl)
            .MaximumLength(200)
            .WithMessage("Thumbnail URL must not exceed 200 characters.");
    }
}