using core_infrastructure.persistence;
using domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace infrastructure.Persistence
{
    public class UnitOfWork : BaseUnitOfWork, IUnitOfWork
    {
        private readonly IServiceProvider _serviceProvider;

        public UnitOfWork(StockDbContext dbContext, IServiceProvider serviceProvider) : base(dbContext, serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        public IStockRepository StockRepo
        {
            get
            {
                return this._serviceProvider.GetRequiredService<IStockRepository>();
            }
        }
    }
}
