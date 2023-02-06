using domain.Entities;
using domain.Interfaces;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace application.UnitTests
{
    public class GetOrdersTests
    {
        IUnitOfWork _uow;

        [SetUp]
        public void Setup()
        {
            this._uow = Helper.GetUowObject();
        }

        [Test]
        public async Task Handle_IsAllOrderTaken_ReturnTrue()
        {
            //Arrange 
            this._uow.OrderRepo.Add(Order.PlaceOrder(new System.Collections.Generic.List<domain.Dtos.OrderItem>
            {
                new domain.Dtos.OrderItem
                {
                    ProductId = 1,
                    Quantity = 2,
                    UnitPrice = 10
                },
                new domain.Dtos.OrderItem
                {
                    ProductId=2,
                    Quantity = 5,
                    UnitPrice = 15
                }
            }));
            this._uow.OrderRepo.Add(Order.PlaceOrder(new System.Collections.Generic.List<domain.Dtos.OrderItem>
            {
                new domain.Dtos.OrderItem
                {
                    ProductId = 1,
                    Quantity = 2,
                    UnitPrice = 20
                },
                new domain.Dtos.OrderItem
                {
                    ProductId=2,
                    Quantity = 5,
                    UnitPrice = 30
                }
            }));

            await this._uow.CompleteAsync();

            var sut = new GetOrders.QueryHandler(this._uow);

            //Act
            var result = await sut.Handle(new GetOrders.Query(), new System.Threading.CancellationToken());

            //Assert
            Assert.IsTrue(result.Items.Count == 2
                       && result.Items.Single(x => x.Id == 1).TotalPrice == 95
                       && result.Items.Single(x => x.Id == 2).TotalPrice == 190);
        }
    }
}
