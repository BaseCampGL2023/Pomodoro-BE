// <copyright file="TaskController.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pomodoro.Api.Controllers.Base;
using Pomodoro.Services;
using Pomodoro.Services.Models;
using Pomodoro.Services.Models.Results;
using Swashbuckle.AspNetCore.Annotations;

namespace Pomodoro.Api.Controllers
{
    /// <summary>
    /// Manage tasks.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TaskController : BaseController
    {
        private readonly TaskService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskController"/> class.
        /// </summary>
        /// <param name="service">Instance of IAppTaskRepository <see cref="TaskService">.</see>/>.</param>
        public TaskController(TaskService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Return all user tasks.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerResponse(200, "Retrieved all user's tasks")]
        public async Task<ActionResult<ICollection<TaskModel>>> GetOwnAll()
        {
            return this.Ok(await this.service.GetOwnAllAsync(this.UserId));
        }

        /// <summary>
        /// Return task by id, or 404 if not exist.
        /// </summary>
        /// <param name="id">Task id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerResponse(200, "The execution was successful")]
        [SwaggerResponse(403, "This task don't belong to current user")]
        [SwaggerResponse(404, "Task not found")]
        public async Task<ActionResult<TaskModel>> GetById(Guid id)
        {
            var result = await this.service.GetOwnByIdAsync(id, this.UserId);

            if (result.Result == ResponseType.NotFound)
            {
                return this.NotFound();
            }

            if (result.Result == ResponseType.Forbid)
            {
                return this.Forbid();
            }

            return this.Ok(result.Data);
        }

        /// <summary>
        /// Persist new user task.
        /// </summary>
        /// <param name="task">New task.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(201, "Task created")]
        [SwaggerResponse(400, "The request was invalid")]
        public async Task<ActionResult<TaskModel>> AddOne(TaskModel task)
        {
            var result = await this.service.AddOwnTaskAsync(task, this.UserId);
            if (result.Success)
            {
                return this.CreatedAtAction(nameof(this.GetById), new { task.Id }, task);
            }

            return this.BadRequest(task);
        }

        /// <summary>
        /// Delete task by id.
        /// </summary>
        /// <param name="id">Task id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(204, "Delete successfully")]
        [SwaggerResponse(400, "No settings with such id for this user")]
        public async Task<ActionResult> DeleteOne(Guid id)
        {
            var result = await this.service.DeleteOneOwnAsync(id, this.UserId);
            return result ? this.NoContent() : this.BadRequest(id);
        }

        /// <summary>
        /// Update existing task.
        /// </summary>
        /// <param name="id">Task id.</param>
        /// <param name="task">Existing task.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(204, "Update successfully")]
        [SwaggerResponse(400, "No settings with such id for this user")]
        public async Task<ActionResult> UpdateOne(Guid id, TaskModel task)
        {
            if (id != task.Id)
            {
                return this.BadRequest();
            }

            var result = await this.service.UpdateOneOwnAsync(task, this.UserId);
            return result ? this.NoContent() : this.BadRequest(id);
        }
    }
}
