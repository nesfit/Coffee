using Barista.Users.Domain;
using Microsoft.EntityFrameworkCore;

namespace Barista.Users.Repositories
{
    public class UsersDbContext : DbContext
    {
        public UsersDbContext() : base()
        {
        }

        public UsersDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
