using System.Data;
using SaborGregoNew.Models;
using saborGregoNew.Repository;

namespace SaborGregoNew.Repository;

public class DashBoardRepository
{
    private readonly IDbConnection _connection;

    public DashBoardRepository(DbConnectionFactory connection)
    {
        _connection = connection.CreateConnection();
    }

    public async Task SelectalesQuantityByCategory(ItensPedido itensPedido)
    {
        using var conn  = _connection;
        conn.Open();
        
        using var cmd = conn.CreateCommand();
        cmd.CommandText = Queries.SalesQuantityByCategory;
        
    }
}