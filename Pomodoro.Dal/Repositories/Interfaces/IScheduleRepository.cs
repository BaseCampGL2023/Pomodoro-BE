// <copyright file="IScheduleRepository.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Pomodoro.Dal.Entities;
using Pomodoro.Dal.Repositories.Base;

namespace Pomodoro.Dal.Repositories.Interfaces
{
    /// <summary>
    /// Providing operations with Schedule objects.
    /// </summary>
    public interface IScheduleRepository : IBelongRepository<Schedule>
    {
        /// <summary>
        /// Retrive schedule from database by id tasks and category.
        /// </summary>
        /// <param name="id">Schedule id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public Task<Schedule?> GetByIdWithRelatedAsync(Guid id);

        /// <summary>
        /// Retrive schedule from database by id no tracking with tasks and category.
        /// </summary>
        /// <param name="id">Schedule id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public Task<Schedule?> GetByIdWithRelatedNoTrackingAsync(Guid id);
    }
}
