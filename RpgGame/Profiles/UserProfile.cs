using AutoMapper;
using RpgGame.DTOs.Auth;

namespace RpgGame.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<CreateUserDto, Models.User>();
            CreateMap<Models.User, UserDto>();
        }
    }
}