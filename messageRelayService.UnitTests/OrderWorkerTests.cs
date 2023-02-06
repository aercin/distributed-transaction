using core_application.Interfaces;
using core_domain.Entitites;
using Dapper;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Dapper;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace messageRelayService.UnitTests
{
    public class OrderWorkerTests
    {
        public class DummyMessage
        {
            public Guid OrderNo { get; set; }
        }

        [Test]
        public async Task ExecuteAsync_IsOutboxMessageSentToBrokerSuccessfully_ReturnTrue()
        {
            var mockLogger = new Mock<ILogger<OrderWorker>>();
            var mockBus = new Mock<IBus>();

            var mockDbConnection = new Mock<IDbConnection>();
            mockDbConnection.SetupDapperAsync(x => x.QueryAsync<OutboxMessage>(It.IsAny<string>(), null, null, null, null)).ReturnsAsync(new List<OutboxMessage>
                                        {
                                             OutboxMessage.CreateOutboxMessage(typeof(DummyMessage).AssemblyQualifiedName,JsonSerializer.Serialize(new DummyMessage{OrderNo = Guid.NewGuid()}),DateTime.Now)
                                        });

            var mockDbConnectionFactory = new Mock<IDbConnectionFactory>();
            mockDbConnectionFactory.Setup(x => x.Context).Returns("order");
            mockDbConnectionFactory.Setup(x => x.GetOpenConnection()).Returns(mockDbConnection.Object);

            var sut = new OrderWorker(mockLogger.Object, mockBus.Object, new List<IDbConnectionFactory> { mockDbConnectionFactory.Object });

            await sut.SendOutboxMessagesToBroker();

            mockBus.Verify(x => x.Publish(It.IsAny<Object>(), default(CancellationToken)), Times.AtLeastOnce);
        }
    }
}