using core_domain.Interfaces;

namespace domain.Interfaces
{
    public interface IUnitOfWork : IBaseUnitOfWork
    {   
        IPaymentRepository PaymentRepo { get; }
    }
}
