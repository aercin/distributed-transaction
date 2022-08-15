using application;
using core_messages.orchestrator_publish_message;
using MassTransit;
using MediatR;

namespace api.Consumers
{
    public class StartPaymentReceivingConsumer : IConsumer<StartPaymentReceiving>
    {
        private readonly IMediator _mediator;

        public StartPaymentReceivingConsumer(IMediator mediator)
        {
            this._mediator = mediator;
        }

        public async Task Consume(ConsumeContext<StartPaymentReceiving> context)
        {
            await this._mediator.Send(new OrderPayment.Command
            {
                OrderNo = context.Message.OrderNo,
                PaymentDate = context.Message.PaymentDate
            });
        }
    }
}
