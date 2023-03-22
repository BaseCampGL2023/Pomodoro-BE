// <copyright file="TaskService.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Pomodoro.Dal.Entities;
using Pomodoro.Dal.Repositories.Interfaces;
using Pomodoro.Services.Base;
using Pomodoro.Services.Models;
using Pomodoro.Services.Models.Results;

namespace Pomodoro.Services
{
    /// <summary>
    /// Perform operations with tasks.
    /// </summary>
    public class TaskService : BaseService<AppTask, TaskModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskService"/> class.
        /// </summary>
        /// <param name="repo">Implementation of IAppTaskRepository <see cref="IAppTaskRepository"/>.</param>
        public TaskService(IAppTaskRepository repo)
            : base(repo)
        {
        }

        /*private readonly IAppTaskRepository taskRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskService"/> class.
        /// </summary>
        /// <param name="repo">Implementation of IAppTaskRepository <see cref="IAppTaskRepository"/>.</param>
        public TaskService(IAppTaskRepository repo)
        {
            this.taskRepo = repo;
        }

        /// <summary>
        /// Return belonging to user task by id.
        /// </summary>
        /// <param name="id">Task id.</param>
        /// <param name="ownerId">Owner id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<ServiceResponse<TaskModel>> GetOwnByIdAsync(Guid id, Guid ownerId)
        {
            var result = await this.taskRepo.GetByIdAsync(id);
            if (result is null)
            {
                return new ServiceResponse<TaskModel> { Result = ResponseType.NotFound };
            }
            else if (result.AppUserId != ownerId)
            {
                return new ServiceResponse<TaskModel> { Result = ResponseType.Forbid };
            }
            else
            {
                return new ServiceResponse<TaskModel> { Result = ResponseType.Ok, Data = TaskModel.Create(result) };
            }
        }

        /// <summary>
        /// Persist new user task.
        /// </summary>
        /// <param name="model">Task view model.</param>
        /// <param name="ownerId">Owner id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<ServiceResponse<bool>> AddOwnTaskAsync(TaskModel model, Guid ownerId)
        {
            var task = model.ToDalEntity(ownerId);
            int result = await this.taskRepo.AddAsync(task, true);
            if (result > 0)
            {
                model.Id = task.Id;
                return new ServiceResponse<bool> { Result = ResponseType.Ok, Data = true };
            }

            return new ServiceResponse<bool> { Data = false };
        }

        /// <summary>
        /// Retrieve all belonging user tasks.
        /// </summary>
        /// <param name="userId">Owner id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<ICollection<TaskModel>> GetOwnAllAsync(Guid userId)
        {
            var tasks = await this.taskRepo.GetBelongingAllAsNoTracking(userId);
            return tasks.Select(e => TaskModel.Create(e)).ToList();
        }

        /// <summary>
        /// Update existing task.
        /// </summary>
        /// <param name="model">Task model.</param>
        /// <param name="userId">Owner id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<bool> UpdateOneOwnAsync(TaskModel model, Guid userId)
        {
            var task = model.ToDalEntity(userId);
            int result = await this.taskRepo.UpdateAsync(task, true);
            if (result > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Delete task from database.
        /// </summary>
        /// <param name="id">Task id.</param>
        /// <param name="userdId">Owner id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<bool> DeleteOneOwnAsync(Guid id, Guid userdId)
        {
            int result = await this.taskRepo.DeleteOneBelongingAsync(id, userdId, true);

            return result > 0;
        }

        // TODO: ModifiedAt*/
    }
}
