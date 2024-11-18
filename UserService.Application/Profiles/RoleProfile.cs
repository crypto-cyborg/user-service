using AutoMapper;
using UserService.Application.Data.Dtos;
using UserService.Core.Models;

namespace UserService.Application.Profiles;

public class RoleProfile : Profile
{
    public RoleProfile()
    {
        CreateMap<Role, RoleReadDto>();
    }
}