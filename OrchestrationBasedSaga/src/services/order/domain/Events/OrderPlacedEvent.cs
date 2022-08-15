using core_domain.Common;

namespace domain.Events
{
    public class OrderPlacedEvent : DomainEvent
    {
        public Guid OrderNo { get; set; }
        public List<OrderItem> Items { get; set; }
    }

    public class OrderItem
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
