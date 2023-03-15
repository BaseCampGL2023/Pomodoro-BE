// <copyright file="IBelongRepository.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Pomodoro.Dal.Entities.Base;

namespace Pomodoro.Dal.Repositories.Base
{
    /// <summary>
    /// Extends the base repository with methods for obtaining objects belonging to a specific user.
    /// </summary>
    /// <typeparam name="T">Instance of type IBelongEntity <see cref="IBelongEntity"/>.</typeparam>
    public interface IBelongRepository<T> : IRepository<T>
        where T : IBelongEntity
    {
        /// <summary>
        /// Retrieve all belonging to the user entities from database.
        /// </summary>
        /// <param name="ownerId">Owner id.</param>
        /// <returns>ICollection collection of objects.</returns>
        public Task<ICollection<T>> GetBelongingAll(Guid ownerId);

        /// <summary>
        /// Retrieve all belonging to the user entities from database without adding them to ChangeTracker.
        /// </summary>
        /// <param name="ownerId">Owner id.</param>
        /// <returns>ICollection collection of objects.</returns>
        public Task<ICollection<T>> GetBelongingAllAsNoTracking(Guid ownerId);

        /// <summary>
        /// Delete entity belonging to user.
        /// </summary>
        /// <param name="id">Entity id.</param>
        /// <param name="ownerId">Owner id.</param>
        /// <param name="persist">Parameter determines whether the repository executes SaveChanges()
        /// immediately when method are called.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public Task<int> DeleteOneBelongingAsync(Guid id, Guid ownerId, bool persist = false);
    }
}
