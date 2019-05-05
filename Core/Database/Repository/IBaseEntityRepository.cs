using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HomeCTRL.Backend.Core.Database.Entity;
using MicroOrm.Dapper.Repositories;

namespace HomeCTRL.Backend.Core.Database.Repository
{
    public interface IBaseEntityRepository<T> where T : BaseEntity
    {

        /// <summary>
        /// Gets all entities
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<T>> GetAll();

        /// <summary>
        /// Gets all entities that match the expression
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> expression);

        /// <summary>
        /// Gets an entity by it's ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> Get(Guid id);

        /// <summary>
        /// Gets an entity that matches the expression
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        Task<T> Get(Expression<Func<T, bool>> expression);

        /// <summary>
        /// Creates a new instance of this entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> Create(T entity);

        /// <summary>
        /// Creates a new entity and returns it with its ID
        /// </summary>
        /// <param name="entity">The entity to create</param>
        /// <returns>The entity when successfull; null otherwise</returns>
        Task<T> CreateAndReturnEntity(T entity);
        
        /// <summary>
        /// Updates an instance of this entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> Update(T entity);

        /// <summary>
        /// Deletes this entity
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> Delete(Guid id);
        
        /// <summary>
        /// Deletes this entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> Delete(T entity);
    }
}