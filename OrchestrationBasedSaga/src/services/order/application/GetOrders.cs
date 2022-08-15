using domain.Enums;
using domain.Interfaces;
using MediatR;

namespace application
{
    public static class GetOrders
    {
        #region Query
        public class Query : IRequest<Response>
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
                var result = this._uow.OrderRepo.GetAll();

                return new Response
                {
                    Items = result.Select(x => new OrderItem
                    {
                        Id = x.Id,
                        TotalPrice = x.TotalPrice,
                        OrderStatus = x.OrderStatus
                    }).ToList()
                };
            }
        }
        #endregion

        #region Response
        public class Response
        {
            public List<OrderItem> Items { get; set; }
        }

        public class OrderItem
        {
            public int Id { get; set; }
            public decimal TotalPrice { get; set; }
            public OrderStatus OrderStatus { get; set; }
        }

        #endregion
    }
}
