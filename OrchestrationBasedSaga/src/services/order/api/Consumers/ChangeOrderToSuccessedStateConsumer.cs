using application;
using core_messages.orchestrator_publish_message;
using MassTransit;
using MediatR;

namespace api.Consumers
{
    public class ChangeOrderToSuccessedStateConsumer : IConsumer<ChangeOrderToSuccessedState>
    {
        private readonly IMediator _mediator;

        public ChangeOrderToSuccessedStateConsumer(IMediator mediator)
        {
            this._mediator = mediator;
        }

        public async Task Consume(ConsumeContext<ChangeOrderToSuccessedState> context)
        {
            await this._mediator.Send(new CompleteOrder.Command
            {
                OrderNo = context.Message.OrderNo,
                Status = domain.Enums.OrderStatus.Successed
            });
        }
    }
}
