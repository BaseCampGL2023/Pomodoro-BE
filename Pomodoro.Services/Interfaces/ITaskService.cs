// <copyright file="ITaskService.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Pomodoro.Dal.Entities;
using Pomodoro.Dal.Repositories.Interfaces;
using Pomodoro.Services.Base;
using Pomodoro.Services.Models;
using Pomodoro.Services.Models.Query;
using Pomodoro.Services.Models.Results;

namespace Pomodoro.Services.Interfaces
{
    /// <summary>
    /// Perform operations with tasks.
    /// </summary>
    public interface ITaskService : IBaseService<AppTask, TaskModel, IAppTaskRepository>
    {
        /// <summary>
        /// Add pomodoro to task.
        /// </summary>
        /// <param name="model">Pomodoro model.</param>
        /// <param name="ownerId">OwnerId.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<ServiceResponse<bool>> AddPomodoro(PomoModel model, Guid ownerId);

        /// <summary>
        /// Retrive tasks with query filter.
        /// </summary>
        /// <param name="ownerId">Owner id.</param>
        /// <param name="query">Query filter <see cref="TaskQueryModel"/>.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<ICollection<TaskModel>> GetOwnByQueryAsync(Guid ownerId, TaskQueryModel query);

        /// <summary>
        /// Return pagination result.
        /// </summary>
        /// <param name="ownerId">Owner id.</param>
        /// <param name="query">Pagination query.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public Task<PaginResult<TaskModel>> GetPaginatedOwnAsync(Guid ownerId, PaginQueryModel query);
    }
}