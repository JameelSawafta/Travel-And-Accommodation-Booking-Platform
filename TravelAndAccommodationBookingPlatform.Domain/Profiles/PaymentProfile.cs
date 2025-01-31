using AutoMapper;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Models.EmailDtos;
using TravelAndAccommodationBookingPlatform.Domain.Models.PaymentDtos;

namespace TravelAndAccommodationBookingPlatform.Domain.Profiles;

public class PaymentProfile : Profile
{
    public PaymentProfile()
    {
        CreateMap<Payment, PaymentDto>()
            .ForMember(dest => dest.Booking, opt => opt.MapFrom(src => src.Booking))
            .ForMember(dest => dest.TransactionDate, opt => opt.MapFrom(src => src.TransactionDate.ToLocalTime()));
        CreateMap<Payment, PaymentResponsetDto>()
            .ForMember(dest => dest.TransactionDate, opt => opt.MapFrom(src => src.TransactionDate.ToLocalTime()));
        CreateMap<Payment, EmailDto>()
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Booking.User.FirstName))
            .ForMember(dest => dest.BookingId, opt => opt.MapFrom(src => src.Booking.BookingId))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
            .ForMember(dest => dest.ToEmail, opt => opt.MapFrom(src => src.Booking.User.Email));
    }
}