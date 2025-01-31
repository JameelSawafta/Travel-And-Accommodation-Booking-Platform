using FluentValidation;
using TravelAndAccommodationBookingPlatform.Domain.Models.UserDtos;

namespace TravelAndAccommodationBookingPlatform.API.Validators.ModelsValidators.AuthValidators;

public class SignupValidator : GenericValidator<SignupDto>
{ 
    public SignupValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required.")
            .MinimumLength(5).WithMessage("Username must be at least 5 characters long.")
            .MaximumLength(20).WithMessage("Username must be at most 20 characters long.")
            .Matches("^[a-zA-Z0-9]*$").WithMessage("Username must contain only alphanumeric characters.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .Must(password => password.Any(char.IsUpper) && password.Any(char.IsLower))
            .WithMessage("Password must contain both uppercase and lowercase characters.")
            .Must(password => password.Any(char.IsDigit))
            .WithMessage("Password must contain at least one digit.")
            .Must(password => password.Any(ch => !char.IsLetterOrDigit(ch)))
            .WithMessage("Password must contain at least one special character.");
        
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email address.");
        
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50).WithMessage("First name must be at most 50 characters long.")
            .Matches("^[a-zA-Z]*$").WithMessage("First name must contain only alphabetic characters.");
        
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(50).WithMessage("Last name must be at most 50 characters long.")
            .Matches("^[a-zA-Z]*$").WithMessage("Last name must contain only alphabetic characters.");
        
        RuleFor(x => x.PhoneNumber)
            .Matches("^[0-9]*$").WithMessage("Phone number must contain only numeric characters.")
            .MinimumLength(10).WithMessage("Phone number must be at least 10 characters long.")
            .MaximumLength(15).WithMessage("Phone number must be at most 15 characters long.");
    }
}