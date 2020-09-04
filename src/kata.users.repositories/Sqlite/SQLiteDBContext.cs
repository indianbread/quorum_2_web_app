using kata.users.shared;
using Microsoft.EntityFrameworkCore;

namespace kata.users.repositories.Sqlite
{
    public class SqLiteDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source=webapp.db");
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FirstName)
                    .HasColumnType("nvarchar")
                    .HasMaxLength(100);
            });

        }
    }
}