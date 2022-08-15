using core_domain.Entitites;
using Microsoft.EntityFrameworkCore;

namespace core_infrastructure.persistence
{
    public abstract class BaseDbContext<T> : DbContext where T : DbContext
    {
        public BaseDbContext(DbContextOptions<T> options) : base(options)
        {

        }
        public DbSet<OutboxMessage> OutboxMessages { get; set; }
    }
}
