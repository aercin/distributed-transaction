using domain.Dtos;
using domain.Interfaces;
using MediatR;

namespace application
{
    public static class DecreaseStock
    {
        #region Command
        public class Command : IRequest<Response>
        {
            public Guid OrderNo { get; set; }
            public List<OrderItem> Items { get; set; }
        }

        #endregion

        #region Command Handler
        public class CommandHandler : IRequestHandler<Command, Response>
        {
            private readonly IUnitOfWork _uow;
            private readonly IStockDomainService _stockDomainService;
            public CommandHandler(IUnitOfWork uow, IStockDomainService stockDomainService)
            {
                this._uow = uow;
                this._stockDomainService = stockDomainService;
            }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                var stock = this._uow.StockRepo.GetStock();

                stock.DecreaseStock(request.OrderNo, request.Items, this._stockDomainService);

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
