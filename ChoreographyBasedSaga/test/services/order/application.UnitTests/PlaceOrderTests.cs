using domain.Interfaces;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace application.UnitTests
{
    public class PlaceOrderTests
    {
        IUnitOfWork _uow;

        [SetUp]
        public void Setup()
        {
            this._uow = Helper.GetUowObject();
        }

        [Test]
        public async Task Handle_IsOrderCreatedSuccessfully_ReturnTrue()
        {
            //Arrange
            var sut = new PlaceOrder.CommandHandler(this._uow);

            //Act
            var result = await sut.Handle(new PlaceOrder.Command
            {
                OrderItems = new System.Collections.Generic.List<domain.Dtos.OrderItem>
                {
                    new domain.Dtos.OrderItem
                    {
                        ProductId = 1,
                        Quantity = 2,
                        UnitPrice = 15
                    }
                }
            }, new System.Threading.CancellationToken());

            //Assert
            Assert.IsTrue(this._uow.OrderRepo.Find(x => x.OrderNo == result.OrderNo).Any());
        }
    }
}