using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using System.Data.Common;

namespace saborGregoNew.Repository
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;

        public DbConnectionFactory(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SQLiteConnection")
                ?? throw new Exception("Connection string 'DefaultConnection' não encontrada.");
        }

        public IDbConnection CreateConnection()
        {
                return new SqlConnection(_connectionString);
        }
        public IDbConnection CreateSqliteConnection()
        {
            return new SqliteConnection(_connectionString);
        }
    }
}

