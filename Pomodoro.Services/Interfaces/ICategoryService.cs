// <copyright file="ICategoryService.cs" company="PomodoroGroup_GL_BaseCamp">
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
    /// Perform operations with categories.
    /// </summary>
    public interface ICategoryService : IBaseService<Category, CategoryModel, ICategoryRepository>
    {
        /// <summary>
        /// Return collection with Categories that don't have any tasks or schedules.
        /// </summary>
        /// <param name="ownerId">Owner id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<ICollection<CategoryModel>> GetOwnAllEmptyAsync(Guid ownerId);

        /// <summary>
        /// Return ServiceResponse, if result Ok ServiceResponse.Data contain
        /// CategoryModel with tasks related to category.
        /// </summary>
        /// <param name="id">Category id.</param>
        /// <param name="ownerId">Owner id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<ServiceResponse<CategoryModel>> GetOwnByIdWithSchedulesAsync(Guid id, Guid ownerId);

        /// <summary>
        /// Return ServiceResponse, if result Ok ServiceResponse.Data contain
        /// CategoryModel with tasks related to category.
        /// </summary>
        /// <param name="id">Category id.</param>
        /// <param name="ownerId">Owner id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<ServiceResponse<CategoryModel>> GetOwnByIdWithTasksAsync(Guid id, Guid ownerId);
    }
}