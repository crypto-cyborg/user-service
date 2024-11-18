using LanguageExt.Common;
using UserService.Application.Data.Dtos;
using UserService.Core.Models;

namespace UserService.Application.Services.Interfaces;

public interface IRoleService
{
    Task<UserReadDto> Grant(User user, Role role);
    Task<RoleReadDto> Revoke(User user, Role role);
}