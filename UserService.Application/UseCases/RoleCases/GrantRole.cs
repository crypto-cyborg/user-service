using LanguageExt.Common;
using UserService.Application.Data.Dtos;
using UserService.Application.Services.Interfaces;
using UserService.Core.Exceptions;
using UserService.Core.Models;
using UserService.Core.Repositories;

namespace UserService.Application.UseCases.RoleCases;

public class GrantRole(
    IRepository<User> userRepository,
    IRepository<Role> roleRepository,
    IRoleService roleService)
{
    public async Task<Result<UserReadDto>> Invoke(Guid userId, int roleId)
    {
        var userTask = userRepository.GetByIdAsync(userId);
        var roleTask = roleRepository.GetByIdAsync(roleId);

        var user = await userTask;
        if (user is null)
        {
            return new Result<UserReadDto>(
                new UserServiceException(
                    UserServiceErrorTypes.ENTITY_NOT_FOUND,
                    "User does not exist"));
        }

        var role = await roleTask;
        if (role is null)
        {
            return new Result<UserReadDto>(
                new UserServiceException(
                    UserServiceErrorTypes.ENTITY_NOT_FOUND,
                    "Required role does not exits"));
        }

        return await roleService.Grant(user, role);
    }
}