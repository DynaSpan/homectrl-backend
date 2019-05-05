using System;
using System.Data;
using HomeCTRL.Backend.Core.Database.Entity;
using MicroOrm.Dapper.Repositories;

namespace HomeCTRL.Backend.Core.Database.Repository.Dapper
{
    public class BaseDapperRepository<T> : DapperRepository<T>, IDisposable where T : BaseEntity
    {
        public BaseDapperRepository(IDbConnection connection) : base(connection)
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