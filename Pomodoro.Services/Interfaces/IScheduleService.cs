// <copyright file="IScheduleService.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Pomodoro.Dal.Entities;
using Pomodoro.Dal.Repositories.Interfaces;
using Pomodoro.Services.Base;
using Pomodoro.Services.Models;
using Pomodoro.Services.Models.Results;

namespace Pomodoro.Services.Interfaces
{
    /// <summary>
    /// Perform operations with schedules.
    /// </summary>
    public interface IScheduleService : IBaseService<Schedule, ScheduleModel, IScheduleRepository>
    {
        /// <summary>
        /// Return all active schedules belonging to current user.
        /// "Active" means that schedule has tasks with start date greater than request datetime.
        /// </summary>
        /// <param name="ownerId">Owner id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<ICollection<ScheduleModel>> GetActiveSchedulesAsync(Guid ownerId);

        /// <summary>
        /// Return all completed schedules belonging to current user.
        /// "Completed" means that all schedule tasks has start date less than request datetime.
        /// </summary>
        /// <param name="ownerId">Owner id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<ICollection<ScheduleModel>> GetCompletedSchedulesAsync(Guid ownerId);

        /// <summary>
        /// Return all empty schedules belonging to current user.
        /// "Empty" means that schedule don't have scheduled tasks.
        /// </summary>
        /// <param name="ownerId">Owner id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<ICollection<ScheduleModel>> GetEmptySchedulesAsync(Guid ownerId);

        /// <summary>
        /// Return belonging to current user schedule, with all tasks correponding to this schedule.
        /// </summary>
        /// <param name="id">Schedule id.</param>
        /// <param name="ownerId">Owner id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<ServiceResponse<ScheduleModel>> GetScheduleWithTasksAsync(Guid id, Guid ownerId);
    }
}