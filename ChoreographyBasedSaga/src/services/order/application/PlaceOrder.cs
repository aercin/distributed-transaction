using domain.Entities;
using domain.Interfaces;
using MediatR;

namespace application
{
    public static class PlaceOrder
    {
        #region Command
        public class Command : IRequest<Response>
        {
            public List<domain.Dtos.OrderItem> OrderItems { get; set; }
        }

        #endregion

        #region Command Handler
        public class CommandHandler : IRequestHandler<Command, Response>
        {
            private readonly IUnitOfWork _uow;
            public CommandHandler(IUnitOfWork uow)
            {
                this._uow = uow;
            }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                var newOrder = Order.PlaceOrder(request.OrderItems);

                this._uow.OrderRepo.Add(newOrder);

                await this._uow.CompleteAsync();

                return new Response { isSuccess = true, OrderNo = newOrder.OrderNo };
            }
        }
        #endregion

        #region Response
        public class Response
        {
            public bool isSuccess { get; set; }

            public Guid OrderNo { get; set; }
        }
        #endregion
    }
}
