using application;
using core_messages;
using MassTransit;
using MediatR;

namespace api.Consumers
{
    public class StockDecreaseFailedConsumer : IConsumer<StockDecreaseFailed>
    {
        private readonly IMediator _mediator;
        public StockDecreaseFailedConsumer(IMediator mediator)
        {
            this._mediator = mediator;
        }

        public async Task Consume(ConsumeContext<StockDecreaseFailed> context)
        {
            await this._mediator.Send(new OrderFailed.Command
            {
                OrderNo = context.Message.OrderNo
            });
        }
    }
}
