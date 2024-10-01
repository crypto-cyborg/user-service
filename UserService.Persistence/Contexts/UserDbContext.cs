using Microsoft.EntityFrameworkCore;
using UserService.Core.Models;

namespace UserService.Persistence.Contexts
{
    public class UserDbContext(DbContextOptions opts) : DbContext(opts)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
    }
}
