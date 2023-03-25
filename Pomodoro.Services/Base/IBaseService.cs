// <copyright file="IBaseService.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Pomodoro.Dal.Entities.Base;
using Pomodoro.Dal.Repositories.Base;
using Pomodoro.Services.Models.Interfaces;
using Pomodoro.Services.Models.Results;

namespace Pomodoro.Services.Base
{
    /// <summary>
    /// Describe base service operations.
    /// </summary>
    public interface IBaseService<T, TV, TR>
        where T : IBelongEntity
        where TV : IBaseModel<T>, new()
        where TR : IBelongRepository<T>
    {
        /// <summary>
        /// Return belonging to user object by id.
        /// </summary>
        /// <param name="id">Object id.</param>
        /// <param name="ownerId">Object owner id.</param>
        /// <returns>Return object without related objects.</returns>
        public Task<ServiceResponse<TV>> GetOwnByIdAsync(Guid id, Guid ownerId);

        /// <summary>
        /// Add new object to database.
        /// </summary>
        /// <param name="model">Object for persisting.</param>
        /// <param name="ownerId">Object owner id.</param>
        /// <returns>TRUE if value persist succesfully, FALSE otherwise.</returns>
        public Task<bool> AddOneOwnAsync(TV model, Guid ownerId);

        /// <summary>
        /// Retrieve all belonging to the user object from database without adding them to ChangeTracker.
        /// </summary>
        /// <param name="ownerId">Owner id.</param>
        /// <returns>Belongin to user objects collection.</returns>
        public Task<ICollection<TV>> GetOwnAllAsync(Guid ownerId);

        /// <summary>
        /// Update exisiting object.
        /// </summary>
        /// <param name="model">Model object.</param>
        /// <param name="ownerId">Object owner Id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public Task<bool> UpdateOneOwnAsync(TV model, Guid ownerId);

        /// <summary>
        /// Delete object from database.
        /// </summary>
        /// <param name="id">Object id.</param>
        /// <param name="ownerId">Owner id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public Task<bool> DeleteOneOwnAsync(Guid id, Guid ownerId);
    }
}
