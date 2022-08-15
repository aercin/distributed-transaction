namespace domain.Entities
{
    public class StockProduct
    {
        public int Id { get; private set; }
        public int ProductId { get; private set; }
        public int InitialQuantity { get; private set; }
        public int RemainingQuantity { get; private set; }

        private StockProduct() { }
        private StockProduct(int productId, int initialQuantity, int remaningQuantity)
        {
            this.ProductId = productId;
            this.InitialQuantity = initialQuantity;
            this.RemainingQuantity = remaningQuantity;
        }

        public static StockProduct CreateStockProduct(int productId, int initialQuantity, int remainingQuantity)
        {
            return new StockProduct(productId, initialQuantity, remainingQuantity);
        }

        public void UpdateStockProduct(int remainingQuantity)
        {
            this.RemainingQuantity = remainingQuantity;
        }
    }
}
