using System.Data;
using Microsoft.Data.SqlClient;
using SaborGregoNew.Repository.Interface;

namespace saborGregoNew.Repository;

public class DbConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public DbConnectionFactory(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
                            ?? throw new Exception("Connection string 'DefaultConnection' não encontrada.");
    }

    public IDbConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }
}