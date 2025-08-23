using Microsoft.EntityFrameworkCore;
using SystoQ.Domain.Entities;

namespace SystoQ.Infrastructure.Persistence
{
    public class SystoQDbContext : DbContext
    {
        public SystoQDbContext(DbContextOptions<SystoQDbContext> options) : base(options)
        {
        }
        
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Sale> Sales { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SystoQDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }        
    }
}
