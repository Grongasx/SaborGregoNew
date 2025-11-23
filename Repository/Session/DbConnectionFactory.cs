using System.Data;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration; // Necessário para IConfiguration

namespace SaborGregoNew.Repository // Corrigido de "saborGregoNew" para "SaborGregoNew"
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;

        public DbConnectionFactory(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SQLiteConnection")
                ?? "Data Source=SaborGrego.db"; 
        }

        public IDbConnection CreateConnection()
        {
            // Como estamos usando SQLite, o padrão deve retornar SQLite também
            return new SqliteConnection(_connectionString);
        }

        public IDbConnection CreateSqliteConnection()
        {
            return new SqliteConnection(_connectionString);
        }
    }
}