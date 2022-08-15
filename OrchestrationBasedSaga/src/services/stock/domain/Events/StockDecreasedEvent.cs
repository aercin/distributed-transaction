using core_domain.Common;

namespace domain.Events
{
    public class StockDecreasedEvent : DomainEvent
    {
        public Guid OrderNo { get; set; }
    }
}
