// <copyright file="IAppTaskRepository.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Pomodoro.Dal.Entities;
using Pomodoro.Dal.Repositories.Base;

namespace Pomodoro.Dal.Repositories.Interfaces
{
    /// <summary>
    /// Providing operations with AppTask objects.
    /// </summary>
    public interface IAppTaskRepository : IBelongRepository<AppTask>
    {
        /// <summary>
        /// Get AppTask object by id with TaskAttempts collection.
        /// </summary>
        /// <param name="id">AppTask id.</param>
        /// <param name="ownerId">Owner id.</param>
        /// <returns>Queried object or null, if object with this id belonging to user doesn't exist in database.</returns>
        public Task<AppTask?> GetByIdWithRelatedAsync(Guid id, Guid ownerId);
    }
}
