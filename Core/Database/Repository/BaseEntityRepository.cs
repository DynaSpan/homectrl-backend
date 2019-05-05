using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib;
using Dapper.Contrib.Extensions;
using HomeCTRL.Backend.Core.Database.Entity;
using HomeCTRL.Backend.Core.Database.Repository.Dapper;
using MicroOrm.Dapper.Repositories;
using MicroOrm.Dapper.Repositories.SqlGenerator;

namespace HomeCTRL.Backend.Core.Database.Repository 
{
    public class BaseEntityRepository<T> : IBaseEntityRepository<T> where T : BaseEntity
    {
        private IDatabaseFactory DbFactory { get; set; }

        public BaseEntityRepository(IDatabaseFactory dbFactory)
        {
            this.DbFactory = dbFactory;
        }

        public virtual async Task<IEnumerable<T>> GetAll()
        {
            using (var repository = this.GetDapperRepository())
            {
                return await repository.FindAllAsync();
            }
        }

        public virtual async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> predicate)
        {
            using (var repository = this.GetDapperRepository())
            {
                return await repository.FindAllAsync(predicate);
            }
        }

        public virtual async Task<T> Get(Guid id)
        {
            using (var repository = this.GetDapperRepository())
            {
                return await repository.FindByIdAsync(id);
            }
        }

        public virtual async Task<T> Get(Expression<Func<T, bool>> predicate)
        {
            using (var repository = this.GetDapperRepository())
            {
                return await repository.FindAsync(predicate);
            }
        }

        public virtual async Task<bool> Create(T entity)
        {
            using (var repository = this.GetDapperRepository())
            {
                entity.Id = Guid.NewGuid();
                return await repository.InsertAsync(entity);
            }
        }

        public virtual async Task<T> CreateAndReturnEntity(T entity)
        {
            using (var repository = this.GetDapperRepository())
            {
                entity.Id = Guid.NewGuid();
                return await repository.InsertAsync(entity) ? entity : null;
            }
        }

        public virtual async Task<bool> Update(T entity)
        {
            using (var repository = this.GetDapperRepository())
            {
                return await repository.UpdateAsync(entity);
            }
        }

        public virtual async Task<bool> Delete(Guid id)
        {
            using (var repository = this.GetDapperRepository())
            {
                return await repository.DeleteAsync(x => x.Id == id);
            }
        }

        public virtual async Task<bool> Delete(T entity)
        {
            using (var repository = this.GetDapperRepository())
            {
                return await repository.DeleteAsync(entity);
            }
        }

        protected BaseDapperRepository<T> GetDapperRepository()
        {
            return new BaseDapperRepository<T>(this.DbFactory.GetDatabase());
        }
    }
}