using core_infrastructure.persistence;
using domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace infrastructure.persistence
{
    public class OrderDbContext : BaseDbContext<OrderDbContext>
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { 
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderDbContext).Assembly);
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
    }
}
