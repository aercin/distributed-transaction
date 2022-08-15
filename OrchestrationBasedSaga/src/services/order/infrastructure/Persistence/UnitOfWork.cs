using core_infrastructure.persistence;
using domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace infrastructure.persistence
{
    public class UnitOfWork : BaseUnitOfWork, IUnitOfWork
    {  
        private readonly IServiceProvider _serviceProvider;

        public UnitOfWork(OrderDbContext context, IServiceProvider serviceProvider) : base(context, serviceProvider)
        { 
            this._serviceProvider = serviceProvider;
        }

        public IOrderRepository OrderRepo
        {
            get
            {
                return this._serviceProvider.GetRequiredService<IOrderRepository>();
            }
        } 
    }
}
