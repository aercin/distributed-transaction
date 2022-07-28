using core_domain.Common;

namespace domain.Events
{
    public class StockDecreaseFailedEvent : DomainEvent
    {
        public Guid OrderNo { get; set; }
    }
}
