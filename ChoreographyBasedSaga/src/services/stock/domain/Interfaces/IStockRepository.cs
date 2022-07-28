using core_domain.Interfaces;
using domain.Entities;

namespace domain.Interfaces
{
    public interface IStockRepository : IGenericRepository<Stock>
    {
        Stock GetStock();
    }
}
