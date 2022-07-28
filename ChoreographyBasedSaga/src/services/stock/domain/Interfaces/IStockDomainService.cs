using domain.Dtos;
using domain.Entities;

namespace domain.Interfaces
{
    public interface IStockDomainService
    {
        bool IsStockAvailable(Stock stock, List<OrderItem> orderItems);
    }
}
