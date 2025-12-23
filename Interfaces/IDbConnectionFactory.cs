using System.Data;

using System.Data.Common;
namespace saborGregoNew.Repository
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
        IDbConnection CreateSqliteConnection();
    }
}
