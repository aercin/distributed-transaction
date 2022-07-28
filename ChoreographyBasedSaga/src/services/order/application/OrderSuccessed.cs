using domain.Interfaces;
using MediatR;

namespace application
{
    public static class OrderSuccessed
    {
        #region Command
        public class Command : IRequest<Response>
        {
            public Guid OrderNo { get; set; }
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
                var relatedOrder = this._uow.OrderRepo.Find(x => x.OrderNo == request.OrderNo).Single();
                relatedOrder.MarkOrderAsCompleted();

                await this._uow.CompleteAsync();

                return new Response
                {
                    IsSuccess = true
                };
            }
        }
        #endregion

        #region Response 
        public class Response
        {
            public bool IsSuccess { get; set; }
        }
        #endregion
    }
}
