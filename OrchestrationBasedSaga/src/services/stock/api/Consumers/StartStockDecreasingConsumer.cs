using application;
using core_messages.orchestrator_publish_message;
using MassTransit;
using MediatR;

namespace api.Consumers
{
    public class StartStockDecreasingConsumer : IConsumer<StartStockDecreasing>
    {
        private readonly IMediator _mediator;
        public StartStockDecreasingConsumer(IMediator mediator)
        {
            this._mediator = mediator;
        }

        public async Task Consume(ConsumeContext<StartStockDecreasing> context)
        {
            await this._mediator.Send(new DecreaseStock.Command
            {
                Items = context.Message.Items.Select(x => new domain.Dtos.OrderItem
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity
                }).ToList(),
                OrderNo = context.Message.OrderNo
            });
        }
    }
}
