using core_domain.Common;
using domain.Dtos;
using domain.Events;
using domain.Interfaces;

namespace domain.Entities
{
    public class Stock : AggregateRootBaseEntity
    {
        public int Id { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public List<StockProduct> StockProducts { get; private set; }

        private Stock()
        {
            this.CreatedOn = DateTime.Today;
        }

        private Stock(List<StockProduct> stockProducts) : this()
        {
            this.StockProducts = stockProducts;
        }

        public static Stock CreateStock(List<StockProduct> stockProducts)
        {
            return new Stock(stockProducts);
        }

        public void DecreaseStock(Guid orderNo, List<OrderItem> orderItems, IStockDomainService stockControl)
        {
            var isStockAvailable = stockControl.IsStockAvailable(this, orderItems);
            if (isStockAvailable)
            {
                foreach (var orderItem in orderItems)
                {
                    var stockItem = this.StockProducts.Single(x => x.ProductId == orderItem.ProductId);
                    stockItem.UpdateStockProduct(stockItem.RemainingQuantity - orderItem.Quantity);
                }

                AddDomainEvent(new StockDecreasedEvent
                {
                    OrderNo = orderNo,
                    CreatedOn = DateTime.Now
                });
            }
            else
            {
                AddDomainEvent(new StockDecreaseFailedEvent
                {
                    OrderNo = orderNo,
                    CreatedOn = DateTime.Now
                });
            }
        }
    }
}
