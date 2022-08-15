using core_domain.Interfaces;

namespace domain.Interfaces
{
    public interface IUnitOfWork : IBaseUnitOfWork
    {  
        IOrderRepository OrderRepo { get; }
    }
}
