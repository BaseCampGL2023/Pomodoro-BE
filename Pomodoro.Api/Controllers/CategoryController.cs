// <copyright file="CategoryController.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;
using Pomodoro.Api.Controllers.Base;
using Pomodoro.Dal.Entities;
using Pomodoro.Dal.Repositories.Interfaces;
using Pomodoro.Services;
using Pomodoro.Services.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace Pomodoro.Api.Controllers
{
    /// <summary>
    /// Manage category.
    /// </summary>
    public class CategoryController : BaseCrudController<CategoryService, Category, CategoryModel, ICategoryRepository>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryController"/> class.
        /// </summary>
        /// <param name="service">Instance of Category service.</param>
        public CategoryController(CategoryService service)
            : base(service)
        {
        }

        /// <summary>
        /// Return all categories belonging to user that don't have any tasks or schedules.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpGet("own/empty")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerResponse(200, "all categories belonging to user that don't have any tasks or schedules.")]
        public async Task<ActionResult<ICollection<CategoryModel>>> GetOwnAllEmpty()
        {
            return this.Ok(await this.Service.GetOwnAllEmptyAsync(this.UserId));
        }

        /// <summary>
        /// Retrieve CategoryModel with tasks related to this category owned by autheticated user.
        /// </summary>
        /// <param name="id">Category id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpGet("own/with-tasks/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerResponse(200, "The execution was successful")]
        [SwaggerResponse(403, "This category object don't belong to current user")]
        [SwaggerResponse(404, "Category not found")]
        public async Task<ActionResult<CategoryModel>> GetByIdWithTasks(Guid id)
        {
            var result = await this.Service.GetOwnByIdWithTasksAsync(id, this.UserId);

            return this.MapServiceResponse(result);
        }

        /// <summary>
        /// Retrieve CategoryModel with schedules related to this category owned by autheticated user.
        /// </summary>
        /// <param name="id">Category id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpGet("own/with-schedules/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerResponse(200, "The execution was successful")]
        [SwaggerResponse(403, "This category object don't belong to current user")]
        [SwaggerResponse(404, "Category not found")]
        public async Task<ActionResult<CategoryModel>> GetByIdWithSchedules(Guid id)
        {
            var result = await this.Service.GetOwnByIdWithSchedulesAsync(id, this.UserId);

            return this.MapServiceResponse(result);
        }
    }
}
