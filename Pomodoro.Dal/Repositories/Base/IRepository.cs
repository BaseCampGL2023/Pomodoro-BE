// <copyright file="IRepository.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using System.Linq.Expressions;
using Pomodoro.Dal.Entities.Base;

namespace Pomodoro.Dal.Repositories.Base
{
    /// <summary>
    /// Basic repository implementing СRUD functionality.
    /// </summary>
    /// <typeparam name="T">Instance of type IEntity <see cref="IEntity"/>.</typeparam>
    public interface IRepository<T>
        where T : IEntity
    {
        /// <summary>
        /// Add one entity in database.
        /// </summary>
        /// <param name="entity">Instance of type IEntity <see cref="IEntity"/>.</param>
        /// <param name="persist">Parameter determines whether the repository executes SaveChanges()
        /// immediately when method are called.</param>
        /// <returns>Number of affected entities.</returns>
        Task<int> AddAsync(T entity, bool persist = false);

        /// <summary>
        /// Add range of entities in database.
        /// </summary>
        /// <param name="entities">Collection of IEntity objects <see cref="IEntity"/>.</param>
        /// <param name="persist">Parameter determines whether the repository executes SaveChanges()
        /// immediately when method are called.</param>
        /// <returns>Number of affected entities.</returns>
        Task<int> AddRangeAsync(IEnumerable<T> entities, bool persist = false);

        /// <summary>
        /// Update one entity in database.
        /// </summary>
        /// <param name="entity">Instance of type IEntity <see cref="IEntity"/>.</param>
        /// <param name="persist">Parameter determines whether the repository executes SaveChanges()
        /// immediately when method are called.</param>
        /// <returns>Number of affected entities.</returns>
        Task<int> UpdateAsync(T entity, bool persist = false);

        /// <summary>
        /// Update range of entities in database.
        /// </summary>
        /// <param name="entities">Collection of IEntity objects <see cref="IEntity"/>.</param>
        /// <param name="persist">Parameter determines whether the repository executes SaveChanges()
        /// immediately when method are called.</param>
        /// <returns>Number of affected entities.</returns>
        Task<int> UpdateRangeAsync(IEnumerable<T> entities, bool persist = false);

        /// <summary>
        /// Delete one entity from database.
        /// </summary>
        /// <param name="entity">Instance of type IEntity <see cref="IEntity"/>.</param>
        /// <param name="persist">Parameter determines whether the repository executes SaveChanges()
        /// immediately when method are called.</param>
        /// <returns>Number of affected entities.</returns>
        Task<int> DeleteAsync(T entity, bool persist = false);

        /// <summary>
        /// Delete one entity from database by id.
        /// </summary>
        /// <param name="id">Entity id.</param>
        /// <param name="persist">Parameter determines whether the repository executes SaveChanges()
        /// immediately when method are called.</param>
        /// <returns>Number of affected entities.</returns>
        Task<int> DeleteAsync(Guid id, bool persist = false);

        /// <summary>
        /// Delete range of entities from database.
        /// </summary>
        /// <param name="entities">Collection of IEntity objects <see cref="IEntity"/>.</param>
        /// <param name="persist">Parameter determines whether the repository executes SaveChanges()
        /// immediately when method are called.</param>
        /// <returns>Number of affected entities.</returns>
        Task<int> DeleteRangeAsync(IEnumerable<T> entities, bool persist = false);

        /// <summary>
        /// Retrive entity from database by id.
        /// </summary>
        /// <param name="id">Entity id.</param>
        /// <returns>Queried object or null, if object with this id doesn't exist in database.</returns>
        Task<T?> GetByIdAsync(Guid id);

        /// <summary>
        /// Retrive entity from database by id without adding to change tracker.
        /// </summary>
        /// <param name="id">Entity id.</param>
        /// <returns>Queried object or null, if object with this id doesn't exist in database.</returns>
        Task<T?> GetByIdNoTrackingAsync(Guid id);

        /// <summary>
        /// Retrieve all entites from database.
        /// </summary>
        /// <returns>ICollection collection of objects.</returns>
        Task<ICollection<T>> GetAllAsync();

        /// <summary>
        /// Retrieve all entites from database without adding them to ChangeTracker.
        /// </summary>
        /// <returns>ICollection collection of objects.</returns>
        Task<ICollection<T>> GetAllAsNoTracking();

        /// <summary>
        /// Retrieve collection of entities that satisfying predicate.
        /// </summary>
        /// <param name="predicate">Expression for query filtration.</param>
        /// <returns>ICollection collection of objects.</returns>
        Task<ICollection<T>> FindAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Check if object with this id already exist in database.
        /// </summary>
        /// <param name="id">Entity id.</param>
        /// <returns>TRUE if object exist, FALSE otherwise.</returns>
        Task<bool> IsExist(Guid id);

        /// <summary>
        /// Persist any changes in the tracked entities.
        /// </summary>
        /// <returns>Number of affected entities.</returns>
        Task<int> SaveChangesAsync();
    }
}
