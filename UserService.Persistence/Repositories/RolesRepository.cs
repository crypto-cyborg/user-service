using UserService.Core.Models;
using UserService.Persistence.Contexts;

namespace UserService.Persistence.Repositories;

public class RolesRepository(UserDbContext context) : RepositoryBase<Role>(context) { }