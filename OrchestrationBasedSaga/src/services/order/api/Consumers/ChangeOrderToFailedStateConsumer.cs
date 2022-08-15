using application;
using core_messages.orchestrator_publish_message;
using domain.Enums;
using MassTransit;
using MediatR;

namespace api.Consumers
{
    public class ChangeOrderToFailedStateConsumer : IConsumer<ChangeOrderToFailedState>
    {
        private readonly IMediator _mediator;

        public ChangeOrderToFailedStateConsumer(IMediator mediator)
        {
            this._mediator = mediator;
        }

        public async Task Consume(ConsumeContext<ChangeOrderToFailedState> context)
        {
            await this._mediator.Send(new CompleteOrder.Command
            {
                OrderNo = context.Message.OrderNo,
                Status = OrderStatus.Failed
            });
        }
    }
}
