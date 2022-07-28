using application;
using core_messages;
using MassTransit;
using MediatR;

namespace api.Consumers
{
    public class PaymentSuccessedConsumer : IConsumer<PaymentSuccessed>
    {
        private readonly IMediator _mediator;
        public PaymentSuccessedConsumer(IMediator mediator)
        {
            this._mediator = mediator;
        }

        public async Task Consume(ConsumeContext<PaymentSuccessed> context)
        {
            await this._mediator.Send(new OrderSuccessed.Command
            {
                OrderNo = context.Message.OrderNo
            });
        }
    }
}
