using domain.Entities;
using domain.Interfaces;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace application.UnitTests
{
    public class OrderFailedTests
    {
        IUnitOfWork _uow;

        [SetUp]
        public void Setup()
        {
            this._uow = Helper.GetUowObject();
        }

        [Test]
        public async Task Handle_IsOrderMarkAsFailed_ReturnTrue()
        {
            //Arrange
            var order = Order.PlaceOrder(new System.Collections.Generic.List<domain.Dtos.OrderItem>
            {
                new domain.Dtos.OrderItem
                {
                    ProductId = 1,
                    Quantity = 5,
                    UnitPrice = 10
                }
            });
            this._uow.OrderRepo.Add(order);

            await this._uow.CompleteAsync();

            var sut = new OrderFailed.CommandHandler(this._uow);

            //Act
            await sut.Handle(new OrderFailed.Command
            {
                OrderNo = order.OrderNo
            }, new System.Threading.CancellationToken());

            //Assert 
            Assert.IsTrue(this._uow.OrderRepo.Find(x => x.OrderNo == order.OrderNo && x.OrderStatus == domain.Enums.OrderStatus.Failed).Count() > 0);
        }
    }
}
