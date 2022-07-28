using core_infrastructure.persistence;
using domain.Entities;
using domain.Interfaces;

namespace infrastructure.Persistence
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(PaymentDbContext context) : base(context)
        {
        }
    }
}
