using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Models.EmailDtos;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Services;

public interface IEmailService
{
    Task SendEmailAsync(EmailDto emailDto);
}