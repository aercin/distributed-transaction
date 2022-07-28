using domain.Entities;
using domain.Interfaces;
using MediatR;

namespace application
{
    public static class OrderPayment
    {
        #region Command
        public class Command : IRequest<Response>
        {
            public Guid OrderNo { get; set; }
            public DateTime PaymentDate { get; set; }
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
                this._uow.PaymentRepo.Add(Payment.CreatePayment(request.OrderNo, request.PaymentDate));

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
