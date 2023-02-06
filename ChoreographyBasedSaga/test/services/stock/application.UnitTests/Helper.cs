using core_application.Interfaces;
using domain.Interfaces;
using infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;

namespace application.UnitTests
{
    public class Helper
    {
        public static IUnitOfWork GetUowObject()
        {
            var options = new DbContextOptionsBuilder<StockDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString("N")).Options;
            var dbContext = new StockDbContext(options);

            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(x => x.GetService(typeof(IStockRepository))).Returns(new StockRepository(dbContext));

            var mockEventDispatcher = new Mock<IEventDispatcher>();
            mockServiceProvider.Setup(x => x.GetService(typeof(IEventDispatcher))).Returns(mockEventDispatcher.Object);

            return new UnitOfWork(dbContext, mockServiceProvider.Object);
        }
    }
}
