using System;
using System.Data;
using HomeCTRL.Backend.Core.Database.Entity;
using MicroOrm.Dapper.Repositories;
using MicroOrm.Dapper.Repositories.SqlGenerator;

namespace HomeCTRL.Backend.Core.Database.Repository.Dapper
{
    public class BaseDapperRepository<T> : DapperRepository<T>, IDisposable where T : BaseEntity
    {
        public BaseDapperRepository(IDbConnection connection) : base(connection, new SqlGenerator<T>(ESqlConnector.MySQL))
        {

        }

        public void Dispose()
        {
            if (this.Connection != null && this.Connection.State != ConnectionState.Closed)
            {
                this.Connection.Close();
            }
        }
    }
}