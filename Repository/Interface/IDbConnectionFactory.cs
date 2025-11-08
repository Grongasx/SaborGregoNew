using System.Data;

namespace SaborGregoNew.Repository.Interface;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}