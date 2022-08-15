using core_infrastructure.persistence;
using domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace infrastructure.Persistence
{
    public class StockDbContext : BaseDbContext<StockDbContext>
    {
        public StockDbContext(DbContextOptions<StockDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(StockDbContext).Assembly);
        }

        public DbSet<Stock> Stocks { get; set; }   
    }
}
