using core_application.Interfaces;
using core_domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace core_infrastructure.persistence
{
    public class BaseUnitOfWork : IBaseUnitOfWork
    {
        private readonly DbContext _context;
        private readonly IEventDispatcher _eventDispatcher;

        public BaseUnitOfWork(DbContext context, IServiceProvider serviceProvider)
        {
            this._context = context;
            this._eventDispatcher = serviceProvider.GetRequiredService<IEventDispatcher>();
        }

        public IOutboxMessageRepository OutboxMessages { get; }

        public async Task CompleteAsync()
        {
            await this._eventDispatcher.DispatchEventAsync();
            await this._context.SaveChangesAsync();
        }

        public void Dispose()
        {
            this._context.Dispose();
        }
    }
}
