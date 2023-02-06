using domain.Dtos;
using domain.Entities;
using domain.Interfaces;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace application.UnitTests
{
    public class DecreaseStockTests
    {
        IUnitOfWork _uow;

        [SetUp]
        public async Task Setup()
        {
            this._uow = Helper.GetUowObject();
            this._uow.StockRepo.Add(Stock.CreateStock(new List<StockProduct>
            {
                StockProduct.CreateStockProduct(1,100,100)
            }));
            await this._uow.CompleteAsync();
        }

        [Test]
        public async Task Handle_IsStockDecreasedSuccessfully_ReturnTrue()
        {
            var mockStockDomainService = new Mock<IStockDomainService>();
            mockStockDomainService.Setup(x => x.IsStockAvailable(It.IsAny<Stock>(), It.IsAny<List<OrderItem>>())).Returns(true);

            var sut = new DecreaseStock.CommandHandler(this._uow, mockStockDomainService.Object);

            var result = await sut.Handle(new DecreaseStock.Command
            {
                OrderNo = new System.Guid(),
                Items = new List<OrderItem>
                {
                    new OrderItem {ProductId = 1, Quantity =25}
                }
            }, new System.Threading.CancellationToken());

            Assert.IsTrue(this._uow.StockRepo.GetStock().StockProducts.Single(x => x.ProductId == 1).RemainingQuantity == 75);
        }
    }
}