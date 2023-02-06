using core_domain.Common;
using core_domain.Interfaces;
using domain.Events;

namespace domain.Entities
{
    public class Payment : AggregateRootBaseEntity
    {
        public int Id { get; private set; }
        public Guid OrderNo { get; private set; }
        public DateTime PaymentDate { get; private set; }

        private Payment()
        {

        }

        private Payment(Guid orderNo, DateTime paymentDate)
        {
            this.OrderNo = orderNo;
            this.PaymentDate = paymentDate;
            AddDomainEvent(new PaymentSuccessedEvent
            {
                OrderNo = orderNo,
                CreatedOn = DateTime.Now
            });
        }

        public static Payment CreatePayment(Guid orderNo, DateTime paymentDate)
        {
            return new Payment(orderNo, paymentDate);
        }
    }
}
