namespace core_domain.Interfaces
{
    public interface IBaseUnitOfWork : IDisposable
    {
        Task CompleteAsync();

        IOutboxMessageRepository OutboxMessages { get; }
    }
}
