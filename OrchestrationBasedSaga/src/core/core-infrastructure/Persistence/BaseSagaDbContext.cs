using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;

namespace core_infrastructure.persistence
{
    public abstract class BaseSagaDbContext<T> : SagaDbContext where T : DbContext
    {
        public BaseSagaDbContext(DbContextOptions<T> options) : base(options)
        {

        }
        public DbSet<core_domain.Entitites.OutboxMessage> OutboxMessages { get; set; }
    }
}
