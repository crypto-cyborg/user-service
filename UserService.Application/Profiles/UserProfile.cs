using AutoMapper;
using UserService.Application.Data.Dtos;
using UserService.Core.Models;

namespace UserService.Application.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserCreateDto, User>()
                .ForMember(
                    target => target.PasswordHash,
                    opt => opt.MapFrom(target => target.Password));

            CreateMap<User, UserReadDto>();
        }
    }
}
