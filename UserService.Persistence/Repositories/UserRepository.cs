using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UserService.Core.Models;
using UserService.Core.Repositories;
using UserService.Persistence.Contexts;

namespace UserService.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext _context;

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public UserRepository(UserDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetById(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<IEnumerable<User>> Get(
            Expression<Func<User, bool>>? filter = null,
            Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = null,
            string includeProperties = "")
        {
            IQueryable<User> query = _context.Users;

            if (filter is not null)
            {
                query = query.Where(filter);
            }

            foreach (var property in includeProperties.Split([','], StringSplitOptions.RemoveEmptyEntries))
            {
                query.Include(property);
            }

            if (orderBy is not null)
            {
                return await orderBy(query).ToListAsync();
            }

            return await query.ToListAsync();
        }

        public async Task Insert(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public void Delete(User user)
        {
            if (_context.Entry(user).State == EntityState.Detached)
            {
                _context.Users.Attach(user);
            }

            _context.Users.Remove(user);
        }

        public void Delete(Guid id)
        {
            var user = _context.Users.Find(id);

            if (user is null)
            {
                return;
            }

            Delete(user);
        }

        public void Update(User user)
        {
            _context.Users.Attach(user);
            _context.Entry(user).State = EntityState.Modified;
        }
    }
}
