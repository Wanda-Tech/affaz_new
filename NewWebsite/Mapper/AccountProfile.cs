using AutoMapper;
using NewWebsite.Extension;
using NewWebsite.Helpers;
using NewWebsite.Models;

namespace NewWebsite.Mapper;
public class AccountProfile : Profile
{
    public AccountProfile()
    {
        CreateMap<SignUpRequest, User>()
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => PasswordHasher.HashPassword(src.Password)));


        CreateMap<UserRequest, User>()
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow));

        CreateMap<User, UserRequest>()
            .ForMember(dest => dest.ExistingRoles, opt => opt.MapFrom(src => src.UserRoles.Select(q=>q.RoleId).ToArray()));
    }
}