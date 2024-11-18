using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UserService.Core.Repositories;
using UserService.Persistence.Contexts;

namespace UserService.Persistence.Repositories
{
    public class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbSet<TEntity> DbSet;
        private readonly UserDbContext _context;

        protected RepositoryBase(UserDbContext context)
        {
            _context = context;
            DbSet = context.Set<TEntity>();
        }

        public async Task<bool> SaveAsync()
            => await _context.SaveChangesAsync() > 0;

        public virtual async Task<TEntity?> GetByIdAsync(object id)
        {
            return await DbSet.FindAsync(id);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            string includeProperties = "")
        {
            var query = DbSet.AsQueryable();

            if (filter is not null)
            {
                query = query.Where(filter);
            }

            query = includeProperties
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Aggregate(query, (current, property) => current.Include(property));

            if (orderBy is not null)
            {
                return await orderBy(query).ToListAsync();
            }

            return await query.ToListAsync();
        }

        public async Task InsertAsync(TEntity entity)
        {
            await DbSet.AddAsync(entity);
        }

        public void Delete(TEntity entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }

            DbSet.Remove(entity);
        }

        public void Delete(object id)
        {
            var entity = DbSet.Find(id);

            if (entity is null)
            {
                return;
            }

            Delete(entity);
        }
    }
}