using domain.Dtos;
using domain.Entities;
using domain.Events;
using domain.Interfaces;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace domain.UnitTests
{
    public class StockTests
    {
        Stock _stock;
        IStockDomainService _stockDomainService;

        [SetUp]
        public void Setup()
        {
            var mockStockDomainService = new Mock<IStockDomainService>();
            mockStockDomainService.Setup(x => x.IsStockAvailable(It.IsAny<Stock>(), It.IsAny<List<OrderItem>>())).Returns(true);
            _stockDomainService = mockStockDomainService.Object;

            _stock = Stock.CreateStock(new List<StockProduct>
            {
                 StockProduct.CreateStockProduct(1,100,100)
            });
        }

        [Test]
        public void DecreaseStock_IsStockUpdatedIfStockAvailable_ReturnTrue()
        {
            //Arrange 
            var orderNo = new System.Guid();
            var orderItems = new List<OrderItem>();
            orderItems.Add(new OrderItem { ProductId = 1, Quantity = 10 });

            //Act
            _stock.DecreaseStock(orderNo, orderItems, _stockDomainService);

            //Assert
            Assert.IsTrue(_stock.StockProducts.Single(x => x.ProductId == 1).RemainingQuantity == 90);
        }


        [Test]
        public void DecreaseStock_IsStockDecreasedEventAddedToEventCollectionWhenStockIsAvailable_ReturnTrue()
        {
            //Arrange 
            var orderNo = new System.Guid();
            var orderItems = new List<OrderItem>();

            //Act
            _stock.DecreaseStock(orderNo, orderItems, _stockDomainService);

            //Assert
            Assert.IsTrue(_stock.Events.Count(x => x.GetType() == typeof(StockDecreasedEvent)) == 1);
        }
    }
}