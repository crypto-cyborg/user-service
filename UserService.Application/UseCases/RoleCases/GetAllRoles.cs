using AutoMapper;
using UserService.Application.Data.Dtos;
using UserService.Application.Profiles;
using UserService.Core.Models;
using UserService.Core.Repositories;

namespace UserService.Application.UseCases.RoleCases;

public class GetAllRoles(IRepository<Role> repository, IMapper mapper)
{
    public async Task<IEnumerable<RoleReadDto>> Invoke()
    {
        var roles = await repository.GetAsync();

        return mapper.Map<IEnumerable<RoleReadDto>>(roles);
    }
}