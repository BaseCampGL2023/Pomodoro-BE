// <copyright file="CategoryService.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;
using Pomodoro.Dal.Entities;
using Pomodoro.Dal.Repositories.Interfaces;
using Pomodoro.Services.Base;
using Pomodoro.Services.Models;
using Pomodoro.Services.Models.Results;

namespace Pomodoro.Services
{
    /// <summary>
    /// Perform operations with categories.
    /// </summary>
    public class CategoryService : BaseService<Category, CategoryModel, ICategoryRepository>
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
                c => c.Id == ownerId
                && !c.Schedules.Any()
                && !c.Tasks.Any());

            return this.MapEntitiesToModels(result);
        }
    }
}
