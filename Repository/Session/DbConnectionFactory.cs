using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace SaborGregoNew.Repository
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;

        public DbConnectionFactory(IConfiguration configuration)
        {
            // Busca a string de conexão correta
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        public IDbConnection CreateConnection()
        {
            // Retorna conexão SQL Server
            return new SqlConnection(_connectionString);
        }
        
        public IDbConnection CreateSqliteConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}