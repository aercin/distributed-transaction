using core_application.Interfaces;
using domain.Interfaces;
using infrastructure.persistence;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;

namespace application.UnitTests
{
    public class Helper
    {
        public static IUnitOfWork GetUowObject()
        {
            var options = new DbContextOptionsBuilder<OrderDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString("N")).Options;
            var dbContext = new OrderDbContext(options);

            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(x => x.GetService(typeof(IOrderRepository))).Returns(new OrderRepository(dbContext));

            var mockEventDispatcher = new Mock<IEventDispatcher>();
            mockServiceProvider.Setup(x => x.GetService(typeof(IEventDispatcher))).Returns(mockEventDispatcher.Object);

            return new UnitOfWork(dbContext, mockServiceProvider.Object);
        }
    }
}
