using domain.Interfaces;
using MediatR;

namespace application
{
    public static class GetStocks
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
                var result = this._uow.StockRepo.GetStock();

                return await Task.FromResult(new Response
                {
                    Id = result.Id,
                    CreatedOn = result.CreatedOn,
                    Items = result.StockProducts.Select(x => new Product
                    {
                        ProductId = x.ProductId,
                        InitialQuantity = x.InitialQuantity,
                        RemainingQuantity = x.RemainingQuantity
                    }).ToList()
                });
            }
        }
        #endregion

        #region Response
        public class Response
        {
            public int Id { get; set; }
            public DateTime CreatedOn { get; set; }
            public List<Product> Items { get; set; }
        }
        public class Product
        {
            public int ProductId { get; set; }
            public int InitialQuantity { get; set; }
            public int RemainingQuantity { get; set; }
        }
        #endregion
    }
}
