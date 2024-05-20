using Microsoft.EntityFrameworkCore;

namespace Hal.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> option) : base(option) 
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Evaluation> Evaluations { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}
