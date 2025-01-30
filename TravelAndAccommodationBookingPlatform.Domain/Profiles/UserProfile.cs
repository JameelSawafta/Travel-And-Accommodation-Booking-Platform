using AutoMapper;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Models.UserDtos;

namespace TravelAndAccommodationBookingPlatform.Domain.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<SignupDto, User>()
            .ForMember(dest => dest.UserId, opt => opt.Ignore()) 
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()) 
            .ForMember(dest => dest.Salt, opt => opt.Ignore());
        CreateMap<User, UserDto>();
    }
}