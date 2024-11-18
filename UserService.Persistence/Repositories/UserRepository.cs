using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using UserService.Core.Models;
using UserService.Persistence.Contexts;

namespace UserService.Persistence.Repositories;

public class UserRepository(UserDbContext context) : RepositoryBase<User>(context)
{
    public override async Task<User?> GetByIdAsync(object id)
    {
        var userId = new Guid(id.ToString());

        return await DbSet
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Id == userId);
    }

    public override async Task<IEnumerable<User>> GetAsync(Expression<Func<User, bool>>? filter = null,
        Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = null, string includeProperties = "")
    {
        var query = DbSet.AsQueryable();

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        query = query.Include(u => u.UserRoles).ThenInclude(ur => ur.Role);

        if (orderBy is not null)
        {
            return await orderBy(query).ToListAsync();
        }

        return await query.ToListAsync();
    }
}