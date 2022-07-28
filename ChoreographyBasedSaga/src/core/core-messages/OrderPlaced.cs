namespace core_messages
{
    public class OrderPlaced
    {
        public Guid OrderNo { get; set; }
        public List<OrderItem> Items { get; set; } 
    }

    public class OrderItem
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
