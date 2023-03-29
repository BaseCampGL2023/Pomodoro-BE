// <copyright file="CategoryService.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;
using Pomodoro.Dal.Entities;
using Pomodoro.Dal.Repositories.Interfaces;
using Pomodoro.Services.Base;
using Pomodoro.Services.Interfaces;
using Pomodoro.Services.Models;
using Pomodoro.Services.Models.Results;

namespace Pomodoro.Services
{
    /// <summary>
    /// Perform operations with categories.
    /// </summary>
    public class CategoryService : BaseService<Category, CategoryModel, ICategoryRepository>, ICategoryService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryService"/> class.
        /// </summary>
        /// <param name="repository">Implementation of ICategoryRepository <see cref="ICategoryRepository"/>.</param>
        /// <param name="logger">Logger instance.</param>
        public CategoryService(ICategoryRepository repository, ILogger<CategoryService> logger)
            : base(repository, logger)
        {
        }

        /// <summary>
        /// Add new category to database, Tasks and Schedules collections should be empty.
        /// </summary>
        /// <param name="model">Category for persisting, model updated after persistance.</param>
        /// <param name="ownerId">Category owner id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public override async Task<ServiceResponse<bool>> AddOneOwnAsync(CategoryModel model, Guid ownerId)
        {
            if (model.Tasks.Any() || model.Schedules.Any())
            {
                return new ServiceResponse<bool>
                {
                    Result = ResponseType.Error,
                    Message = "Tasks or schedules shouldn't be add with category.",
                };
            }

            return await base.AddOneOwnAsync(model, ownerId);
        }

        /// <summary>
        /// Update exisiting category, Tasks and Schedules collections should be empty.
        /// </summary>
        /// <param name="model">Category object, value updated after persistance.</param>
        /// <param name="ownerId">Category owner Id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public override async Task<ServiceResponse<bool>> UpdateOneOwnAsync(CategoryModel model, Guid ownerId)
        {
            if (model.Tasks.Any() || model.Schedules.Any())
            {
                return new ServiceResponse<bool>
                {
                    Result = ResponseType.Error,
                    Message = "Tasks or schedules shouldn't be add when updating category.",
                };
            }

            return await this.UpdateOneOwnAsync(model, ownerId);
        }

        /// <summary>
        /// Return ServiceResponse, if result Ok ServiceResponse.Data contain
        /// CategoryModel with tasks related to category.
        /// </summary>
        /// <param name="id">Category id.</param>
        /// <param name="ownerId">Owner id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<ServiceResponse<CategoryModel>> GetOwnByIdWithTasksAsync(Guid id, Guid ownerId)
        {
            var category = await this.Repo.GetByIdWithTasksdNoTrackingAsync(id);
            return this.ReturnOneOwnAsync(category, ownerId);
        }

        /// <summary>
        /// Return ServiceResponse, if result Ok ServiceResponse.Data contain
        /// CategoryModel with tasks related to category.
        /// </summary>
        /// <param name="id">Category id.</param>
        /// <param name="ownerId">Owner id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<ServiceResponse<CategoryModel>> GetOwnByIdWithSchedulesAsync(Guid id, Guid ownerId)
        {
            var category = await this.Repo.GetByIdWithSchedulesNoTrackingAsync(id);
            return this.ReturnOneOwnAsync(category, ownerId);
        }

        /// <summary>
        /// Return collection with Categories that don't have any tasks or schedules.
        /// </summary>
        /// <param name="ownerId">Owner id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<ICollection<CategoryModel>> GetOwnAllEmptyAsync(Guid ownerId)
        {
            var result = await this.Repo.FindAsync(
                c => c.AppUserId == ownerId
                && !c.Schedules.Any()
                && !c.Tasks.Any());

            return this.MapEntitiesToModels(result);
        }
    }
}
