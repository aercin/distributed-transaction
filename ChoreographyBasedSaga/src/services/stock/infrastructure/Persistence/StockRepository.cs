using core_infrastructure.persistence;
using domain.Entities;
using domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace infrastructure.Persistence
{
    public class StockRepository : GenericRepository<Stock>, IStockRepository
    {
        private readonly StockDbContext _context;
        public StockRepository(StockDbContext context) : base(context)
        {
            this._context = context;
        }

        public Stock GetStock()
        {
            return this._context.Stocks.Include(x => x.StockProducts).Single();
        }
    }
}
