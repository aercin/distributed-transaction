namespace core_application.Interfaces
{
    public interface IDomainEventToMessageMapper
    {
        Type GetMessageType(Type domainEventType);
    }
}
