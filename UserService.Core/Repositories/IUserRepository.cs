using System.Linq.Expressions;
using UserService.Core.Models;

namespace UserService.Core.Repositories
{
    public interface IUserRepository
    {
        public Task<bool> SaveAsync();

        public Task<IEnumerable<User>> GetAllAsync();
        public Task<User?> GetByIdAsync(Guid id);
        public Task<IEnumerable<User>> Get(
            Expression<Func<User, bool>>? filter = null,
            Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = null,
            string includeProperties = "");

        public Task Insert(User user);

        public void Delete(Guid id);
        public void Delete(User user);

        public void Update(User user);
    }
}
