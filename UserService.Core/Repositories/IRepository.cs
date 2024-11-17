using System.Linq.Expressions;
using UserService.Core.Models;

namespace UserService.Core.Repositories
{
    public interface IRepository<TEntity>
    {
        Task<bool> SaveAsync();
        Task<TEntity?> GetByIdAsync(object id);

        Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            string includeProperties = "");

        Task InsertAsync(TEntity entity);
        void Delete(object id);
        void Delete(TEntity entity);
    }
}