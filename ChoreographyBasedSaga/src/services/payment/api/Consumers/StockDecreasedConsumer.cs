using application;
using core_messages;
using MassTransit;
using MediatR;

namespace api.Consumers
{
    public class StockDecreasedConsumer : IConsumer<StockDecreased>
    {
        private readonly IMediator _mediator;
        public StockDecreasedConsumer(IMediator mediator)
        {
            this._mediator = mediator;
        }

        public async Task Consume(ConsumeContext<StockDecreased> context)
        {
            await this._mediator.Send(new OrderPayment.Command
            {
                OrderNo = context.Message.OrderNo,
                PaymentDate = DateTime.Now
            });
        }
    }
}
