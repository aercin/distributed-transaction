using domain.Enums;
using domain.Interfaces;
using MediatR;

namespace application
{
    public static class CompleteOrder
    {
        #region Command
        public class Command : IRequest<Response>
        {
            public Guid OrderNo { get; set; }

            public OrderStatus Status { get; set; }
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
                var currentOrder = this._uow.OrderRepo.Find(x => x.OrderNo == request.OrderNo).Single();

                currentOrder.MarkOrderCompleted(request.Status);

                await this._uow.CompleteAsync();

                return new Response { isSuccess = true };
            }
        }
        #endregion

        #region Response
        public class Response
        {
            public bool isSuccess { get; set; }
        }
        #endregion
    }
}
