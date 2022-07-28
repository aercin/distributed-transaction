using core_domain.Common;
using domain.Enums;
using domain.Events;

namespace domain.Entities
{
    public class Order : AggregateRootBaseEntity
    {
        public int Id { get; private set; }
        public Guid OrderNo { get; private set; }
        public decimal TotalPrice { get; private set; }
        public OrderStatus OrderStatus { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public List<OrderProduct> Products { get; private set; }

        private Order()
        {
            OrderNo = Guid.NewGuid();
            Products = new List<OrderProduct>();
            CreatedOn = DateTime.Now;
            OrderStatus = OrderStatus.Suspend;
        }

        private Order(List<OrderProduct> products, decimal totalPrice) : this()
        {
            this.Products = products;
            this.TotalPrice = totalPrice;
            AddDomainEvent(new OrderPlacedEvent
            {
                OrderNo = OrderNo,
                CreatedOn = DateTime.Now,
                Items = products.Select(x => new OrderItem
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity
                }).ToList()
            });
        }
        public static Order PlaceOrder(List<Dtos.OrderItem> products)
        {
            var orderProducts = products.Select(x => OrderProduct.CreateOrderProduct(x.ProductId, x.Quantity, x.UnitPrice)).ToList();
            return new Order(orderProducts, orderProducts.Sum(x => x.UnitPrice * x.Quantity));
        }

        public void MarkOrderAsFailed()
        {
            OrderStatus = OrderStatus.Failed;
        }

        public void MarkOrderAsCompleted()
        {
            OrderStatus = OrderStatus.Successed;
        }
    }
}
