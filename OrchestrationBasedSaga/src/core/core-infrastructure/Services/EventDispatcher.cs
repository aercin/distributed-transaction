using core_application.Interfaces;
using core_domain.Common;
using core_domain.Entitites;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace core_infrastructure.Services
{
    public class EventDispatcher<T> : IEventDispatcher where T : DbContext
    {
        private readonly T _context;
        private readonly ISystemClock _systemClock;
        private readonly IDomainEventToMessageMapper _domainEventToMessageMapper;

        public EventDispatcher(T context, ISystemClock systemClock, IDomainEventToMessageMapper domainEventToMessageMapper)
        {
            _context = context;
            _systemClock = systemClock;
            _domainEventToMessageMapper = domainEventToMessageMapper;
        }

        public async Task DispatchEventAsync()
        {
            var domainEntities = this._context.ChangeTracker.Entries<AggregateRootBaseEntity>()
                                              .Where(x => x.Entity.Events != null && x.Entity.Events.Any());

            var domainEvents = domainEntities.SelectMany(x => x.Entity.Events).ToList();

            var tasks = domainEvents.Select(async (domainEvent) =>
            {
                await StoreEventsToDB(domainEvent);
            });

            await Task.WhenAll(tasks);
        }

        private async Task StoreEventsToDB(DomainEvent domainEvent)
        {
            var type = this._domainEventToMessageMapper.GetMessageType(domainEvent.GetType());
            var message = JsonSerializer.Serialize(domainEvent, domainEvent.GetType());

            await _context.Set<OutboxMessage>().AddAsync(OutboxMessage.CreateOutboxMessage(type.AssemblyQualifiedName, message, _systemClock.Current));
        }
    }
}
