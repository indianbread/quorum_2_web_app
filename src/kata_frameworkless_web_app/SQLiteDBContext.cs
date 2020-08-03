using Microsoft.EntityFrameworkCore;

namespace kata_frameworkless_web_app
{
    public class SqLiteDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source=webapp.db");
        }
    }
}