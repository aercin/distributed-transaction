using core_application.Interfaces;
using Npgsql;
using System.Data;

namespace core_infrastructure.Persistence
{
    public class PostgreDbConnectionFactory : IDbConnectionFactory
    {
        private readonly string _context;
        private readonly string _connectionString;
        private IDbConnection _connection;

        public PostgreDbConnectionFactory(string context, string connStr)
        {
            _context = context;
            _connectionString = connStr;
        }

        public string Context
        {
            get
            {
                return this._context;
            }
        } 

        public IDbConnection GetOpenConnection()
        {
            if (this._connection == null || this._connection.State != ConnectionState.Open)
            {
                this._connection = new NpgsqlConnection(_connectionString);
                this._connection.Open();
            }

            return this._connection;
        }

        public void Dispose()
        {
            if (this._connection != null && this._connection.State == ConnectionState.Open)
            {
                this._connection.Dispose();
            }
        }
    }
}
