using core_messages.common_models;

namespace core_messages
{
    public class OrderPlaced
    {
        public Guid OrderNo { get; set; }
        public List<OrderItem> Items { get; set; } 
    } 
}
