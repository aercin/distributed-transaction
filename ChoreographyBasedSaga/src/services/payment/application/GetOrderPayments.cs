using domain.Interfaces;
using MediatR;

namespace application
{
    public static class GetOrderPayments
    {
        #region Query
        public class Query: IRequest<Response>
        { 
        }
        #endregion

        #region Query Handler
        public class QueryHandler : IRequestHandler<Query, Response>
        {
            private readonly IUnitOfWork _uow;
            public QueryHandler(IUnitOfWork uow)
            {
                this._uow = uow;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = this._uow.PaymentRepo.GetAll();
                return await Task.FromResult(new Response
                {
                    Items = result.Select(x => new PaymentItem
                    {
                        OrderNo = x.OrderNo,
                        PaymentDate = x.PaymentDate
                    }).ToList()
                });
            }
        }
        #endregion

        #region Response 
        public class Response
        {
           public List<PaymentItem> Items { get; set; }
        }

        public class PaymentItem
        {
            public Guid OrderNo { get; set; }
            public DateTime PaymentDate { get; set; }
        }
        #endregion
    }
}
