using System.Data;

namespace core_application.Interfaces
{
    public interface IDbConnectionFactory
    {
        string Context { get; }
        IDbConnection GetOpenConnection();
    }
}
