using core_domain.Common;

namespace domain.Events
{
    public class PaymentSuccessedEvent : DomainEvent
    {
        public Guid OrderNo { get; set; }
    }
}
