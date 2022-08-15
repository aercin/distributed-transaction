using core_infrastructure.persistence;
using domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace infrastructure.Persistence
{
    public class UnitOfWork : BaseUnitOfWork, IUnitOfWork
    {
        private readonly IServiceProvider _serviceProvider;
        public UnitOfWork(PaymentDbContext dbContext, IServiceProvider serviceProvider) : base(dbContext, serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        public IPaymentRepository PaymentRepo
        {
            get
            {
                return this._serviceProvider.GetRequiredService<IPaymentRepository>();
            }
        }
    }
}
