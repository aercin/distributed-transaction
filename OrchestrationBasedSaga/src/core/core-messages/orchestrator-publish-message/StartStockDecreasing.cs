using core_messages.common_models;

namespace core_messages.orchestrator_publish_message
{
    public class StartStockDecreasing
    {
        public Guid OrderNo { get; set; }
        public List<OrderItem> Items { get; set; }
    }
}
