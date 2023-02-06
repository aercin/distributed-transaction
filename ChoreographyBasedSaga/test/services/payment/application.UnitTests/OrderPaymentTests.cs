using domain.Interfaces;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace application.UnitTests
{
    public class OrderPaymentTests
    {
        IUnitOfWork _uow;

        [SetUp]
        public void Setup()
        {
            this._uow = Helper.GetUowObject();
        }

        [Test]
        public async Task Handle_IsPaymentTakenSuccessfully_ReturnTrue()
        {
            var sut = new OrderPayment.CommandHandler(this._uow);

            var orderPaymentCommand = new OrderPayment.Command
            {
                OrderNo = Guid.NewGuid(),
                PaymentDate = DateTime.Now
            };

            var result = await sut.Handle(orderPaymentCommand, new System.Threading.CancellationToken());

            Assert.IsTrue(this._uow.PaymentRepo.Find(x => x.OrderNo == orderPaymentCommand.OrderNo).Any());
        }
    }
}