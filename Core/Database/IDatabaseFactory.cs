using System.Data;
using System.Data.SqlClient;

namespace HomeCTRL.Backend.Core.Database
{
    public interface IDatabaseFactory
    {
        IDbConnection GetDatabase();
    }
}