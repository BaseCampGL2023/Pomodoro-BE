// <copyright file="ICategoryRepository.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Pomodoro.Dal.Entities;
using Pomodoro.Dal.Repositories.Base;

namespace Pomodoro.Dal.Repositories.Interfaces
{
    /// <summary>
    /// Perform operations with categories.
    /// </summary>
    public interface ICategoryRepository : IBelongRepository<Category>
    {
        /// <summary>
        /// Retrive category from database by id with tasks no tracking.
        /// </summary>
        /// <param name="id">Entity id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public Task<Category?> GetByIdWithTasksdNoTrackingAsync(Guid id);

        /// <summary>
        /// Retrive category from database by id with schedules no tracking.
        /// </summary>
        /// <param name="id">Entity id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public Task<Category?> GetByIdWithSchedulesNoTrackingAsync(Guid id);
    }
}
