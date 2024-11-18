using AutoMapper;
using LanguageExt.Common;
using Microsoft.EntityFrameworkCore;
using UserService.Application.Data.Dtos;
using UserService.Application.Services.Interfaces;
using UserService.Core.Exceptions;
using UserService.Core.Models;
using UserService.Core.Repositories;
using UserService.Persistence.Contexts;

namespace UserService.Application.Services;

public class RoleService(
    IRepository<User> userRepository,
    IRepository<Role> roleRepository,
    UserDbContext context,
    IMapper mapper) : IRoleService
{
    public async Task<UserReadDto> Grant(User user, Role role)
    {
        var userRole = new UserRole()
        {
            UserId = user.Id,
            RoleId = role.Id,
        };

        await context.UserRoles.AddAsync(userRole);
        await context.SaveChangesAsync();

        return mapper.Map<UserReadDto>(user);
    }

    public async Task<RoleReadDto> Revoke(User user, Role role)
    {
        var record = await context.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == user.Id && ur.RoleId == role.Id);

        if (record is null)
        {
            return mapper.Map<RoleReadDto>(role);
        }

        context.UserRoles.Remove(record);
        await context.SaveChangesAsync();

        return mapper.Map<RoleReadDto>(role);
    }
}