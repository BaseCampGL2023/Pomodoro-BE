﻿// <copyright file="IAppTaskRepository.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Pomodoro.Dal.Entities;
using Pomodoro.Dal.Repositories.Base;

namespace Pomodoro.Dal.Repositories.Interfaces
{
    /// <summary>
    /// Providing operations with AppTask objects.
    /// </summary>
    public interface IAppTaskRepository : IBelongRepository<AppTask>, IWithRelated<AppTask>
    {
        /// <summary>
        /// Retrieve all belonging to the user finished tasks from database.
        /// </summary>
        /// <param name="ownerId">Owner id.</param>
        /// <returns>ICollection collection of objects.</returns>
        public Task<ICollection<AppTask>> GetBelonginFinishedAllAsync(Guid ownerId);

        /// <summary>
        /// Retrieve all belonging to the user not started tasks from database.
        /// </summary>
        /// <param name="ownerId">Owner id.</param>
        /// <returns>ICollection collection of objects.</returns>
        public Task<ICollection<AppTask>> GetBelonginPrisitineAllAsync(Guid ownerId);

        /// <summary>
        /// Retrieve all belonging to the user started tasks from database.
        /// </summary>
        /// <param name="ownerId">Owner id.</param>
        /// <returns>ICollection collection of objects.</returns>
        public Task<ICollection<AppTask>> GetBelonginStartedAllAsync(Guid ownerId);
    }
}
