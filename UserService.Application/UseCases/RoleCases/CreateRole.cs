using LanguageExt.Common;
using UserService.Application.Data.Dtos;
using UserService.Core.Exceptions;
using UserService.Core.Models;
using UserService.Core.Repositories;

namespace UserService.Application.UseCases.RoleCases;

public class CreateRole(IRepository<Role> repository)
{
    public async Task<Result<Role>> Invoke(RoleCreateDto data)
    {
        var exists = await repository.GetAsync(r => r.Name == data.Name);

        if (exists.Any())
        {
            return new Result<Role>(new UserServiceException(
                UserServiceErrorTypes.ENTITY_EXISTS, "Role already exists"));
        }

        var role = new Role { Name = data.Name };

        await repository.InsertAsync(role);
        await repository.SaveAsync();

        return role;
    }
}