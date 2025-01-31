using AutoMapper;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Models.BookingDetailDtos;

namespace TravelAndAccommodationBookingPlatform.Domain.Profiles;

public class BookingDetailProfile : Profile
{
    public BookingDetailProfile()
    {
        CreateMap<BookingDetail, BookingDetailDto>()
            .ForMember(dest => dest.Room, opt => opt.MapFrom(src => src.Room));
    }
}