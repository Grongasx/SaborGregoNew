using System.Data;

namespace SaborGregoNew.Repository
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
        IDbConnection CreateSqliteConnection();
    }
}