// <copyright file="IAppUserRepository.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Pomodoro.Dal.Entities;
using Pomodoro.Dal.Repositories.Base;

namespace Pomodoro.Dal.Repositories.Interfaces
{
    /// <summary>
    /// Providing operations with AppUser objects.
    /// </summary>
    public interface IAppUserRepository : IRepository<AppUser>
    {
        /// <summary>
        /// Get AppUser object by id with related objects.
        /// </summary>
        /// <param name="id">AppUser id.</param>
        /// <returns>AppUser object with related or NULL if object with this id don't exist.</returns>
        public Task<AppUser?> GetByIdAsyncWithRelatedAsync(Guid id);

        /// <summary>
        /// Get AppUser object by id with setings collection.
        /// </summary>
        /// <param name="id">AppUser id.</param>
        /// <returns>AppUser object with related or NULL if object with this id don't exist.</returns>
        public Task<AppUser?> GetByIdWithSettingsAsync(Guid id);

        /// <summary>
        /// Get AppUser object by id with user tasks collection.
        /// </summary>
        /// <param name="id">AppUser id.</param>
        /// <returns>AppUser object with related or NULL if object with this id don't exist.</returns>
        public Task<AppUser?> GetByIdWithTasksAsync(Guid id);

        /// <summary>
        /// Get AppUser object by id with user routines collection.
        /// </summary>
        /// <param name="id">AppUser id.</param>
        /// <returns>AppUser object with related or NULL if object with this id don't exist.</returns>
        public Task<AppUser?> GetByIdWithRoutinesAsync(Guid id);
    }
}
