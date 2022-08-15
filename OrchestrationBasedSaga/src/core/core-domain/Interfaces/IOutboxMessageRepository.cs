using core_domain.Entitites;

namespace core_domain.Interfaces
{
    public interface IOutboxMessageRepository : IGenericRepository<OutboxMessage>
    { 
    }
}
