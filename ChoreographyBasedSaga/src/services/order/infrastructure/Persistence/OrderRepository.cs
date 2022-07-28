using core_infrastructure.persistence;
using domain.Entities;
using domain.Interfaces;

namespace infrastructure.persistence
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(OrderDbContext context) : base(context)
        {

        }
    }
}
