using Microsoft.EntityFrameworkCore;
using plato_backend.Model;

namespace plato_backend.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base (options)
        {

        }

        public DbSet<UserModel> User { get; set; }
        public DbSet<BlogModel> Blog { get; set; }
        public DbSet<CommentsModel> Comment { get; set; }
        public DbSet<ReplysModel> Reply { get; set; }
    }
}