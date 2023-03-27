// <copyright file="IWithRelated.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Pomodoro.Dal.Entities.Base;

// TODO: delete it's usefull
namespace Pomodoro.Dal.Repositories.Interfaces
{
    /// <summary>
    /// Perform downloading entities with related entities or collections.
    /// </summary>
    /// <typeparam name="T">Instance of type IBelongEntity <see cref="IBelongEntity"/>.</typeparam>
    public interface IWithRelated<T>
        where T : IEntity, new()
    {
        /// <summary>
        /// Retrive entity from database by id with all related data.
        /// </summary>
        /// <param name="id">Entity id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public Task<T?> GetByIdWithRelatedAsync(Guid id);

        /// <summary>
        /// Retrive entity from database by id no tracking with all related data.
        /// </summary>
        /// <param name="id">Entity id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public Task<T?> GetByIdWithRelatedNoTrackingAsync(Guid id);
    }
}
