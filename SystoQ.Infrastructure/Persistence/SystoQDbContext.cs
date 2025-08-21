using Microsoft.EntityFrameworkCore;

namespace SystoQ.Infrastructure.Persistence
{
    public class SystoQDbContext : DbContext
    {
        public SystoQDbContext(DbContextOptions<SystoQDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure your entities here
        }
        // Define DbSet properties for your entities
        // public DbSet<YourEntity> YourEntities { get; set; }
    }
}
