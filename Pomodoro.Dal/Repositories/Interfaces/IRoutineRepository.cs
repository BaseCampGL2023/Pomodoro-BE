// <copyright file="IRoutineRepository.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Pomodoro.Dal.Entities;
using Pomodoro.Dal.Repositories.Base;

namespace Pomodoro.Dal.Repositories.Interfaces
{
    /// <summary>
    /// Providing operations with Routine objects.
    /// </summary>
    public interface IRoutineRepository : IBelongRepository<Routine>
    {
        /// <summary>
        /// Get AppTask object by id with RoutineAttempts collection.
        /// </summary>
        /// <param name="id">AppTask id.</param>
        /// <returns>Queried object or null, if object with this id belonging to user doesn't exist in database.</returns>
        Task<Routine?> GetByIdWithRelatedAsync(Guid id);
    }
}
