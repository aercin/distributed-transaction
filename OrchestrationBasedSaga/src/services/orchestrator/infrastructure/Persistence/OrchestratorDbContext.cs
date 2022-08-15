using core_infrastructure.persistence;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;

namespace infrastructure.Persistence
{
    public class OrchestratorDbContext : BaseSagaDbContext<OrchestratorDbContext>
    {
        public OrchestratorDbContext(DbContextOptions<OrchestratorDbContext> options) : base(options)
        {
        }

        protected override IEnumerable<ISagaClassMap> Configurations
        {
            get
            {
                yield return new OrderStateMap();
            }
        }
    }
}
