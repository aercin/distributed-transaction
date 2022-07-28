namespace domain.Entities
{
    public class OrderProduct
    {
        public int Id { get; private set; }
        public int ProductId { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }

        private OrderProduct()
        {
           
        }

        private OrderProduct(int productId, int quantity, decimal unitPrice)
        {
            this.ProductId = productId;
            this.Quantity = quantity;
            this.UnitPrice = unitPrice;
        }

        public static OrderProduct CreateOrderProduct(int productId, int quantity, decimal unitPrice)
        {
            return new OrderProduct(productId, quantity, unitPrice);
        }
    }
}
