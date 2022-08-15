using core_domain.Common;

namespace domain.Events
{
    public class PaymentFailedEvent : DomainEvent
    {
        public Guid OrderNo { get; set; }
    }
}
