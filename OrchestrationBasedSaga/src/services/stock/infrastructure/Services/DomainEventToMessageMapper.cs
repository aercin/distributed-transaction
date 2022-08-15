using core_application.Interfaces;
using core_messages;
using domain.Events;

namespace infrastructure.Services
{
    public class DomainEventToMessageMapper : IDomainEventToMessageMapper
    {
        private readonly Dictionary<Type, Type> _dicMapping;
        public DomainEventToMessageMapper()
        {
            _dicMapping = new Dictionary<Type, Type>();
            _dicMapping.Add(typeof(StockDecreasedEvent), typeof(StockDecreased));
            _dicMapping.Add(typeof(StockDecreaseFailedEvent), typeof(StockDecreaseFailed));
        }

        public Type GetMessageType(Type domainEventType)
        {
            return _dicMapping[domainEventType];
        }
    }
}
