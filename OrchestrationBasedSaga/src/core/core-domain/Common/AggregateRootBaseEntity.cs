using core_domain.Interfaces;
using System.Collections.ObjectModel;

namespace core_domain.Common
{
    public abstract class AggregateRootBaseEntity : IAggregateRoot
    {
        private List<DomainEvent> events;
        public AggregateRootBaseEntity()
        {
            events = new List<DomainEvent>();
        }

        public ReadOnlyCollection<DomainEvent> Events
        {
            get
            {
                return events.AsReadOnly();
            }
        }

        public void AddDomainEvent(DomainEvent domainEvent)
        {
            events.Add(domainEvent);
        }
    }
}
