// <copyright file="TasksController.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Pomodoro.Api.Controllers.Base;
using Pomodoro.Core.Interfaces.IServices;
using Pomodoro.Core.Models.Tasks;
using Pomodoro.DataAccess.Entities;
using Pomodoro.DataAccess.Repositories.Interfaces;
using Pomodoro.Services.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace Pomodoro.Api.Controllers
{
    /// <summary>
    /// API controller for managing the tasks.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : BaseController
    {
        private readonly ITaskService tasksService;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="TasksController"/> class.
        /// </summary>
        /// <param name="mapper">It is an Interface for mapping 2 objects.</param>
        /// <param name="tasksService">The service, which is responsible for manipulating with tasks data.</param>
        public TasksController(
            ITaskService tasksService,
            IMapper mapper)
        {
            this.tasksService = tasksService;
            this.mapper = mapper;
        }

        /// <summary>
        /// Gets task by specific id.
        /// </summary>
        /// <param name="id">The day for which statistics should be returned.</param>
        /// <returns>A <see cref="TaskModel"/> object.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerResponse(200, "The task was found")]
        [SwaggerResponse(400, "Invalid data")]
        [SwaggerResponse(404, "The task with this id wasn`t found")]
        [SwaggerResponse(500, "Something went wrong")]
        public async Task<ActionResult<TaskModel>> GetTaskById(Guid id)
        {
            if (id == Guid.Empty)
            {
                return this.BadRequest();
            }

            var task = await this.tasksService.GetTaskByIdAsync(id);
            if (task == null)
            {
                return this.NotFound();
            }

            return this.Ok(task);
        }

        /// <summary>
        /// Gets all tasks by user.
        /// </summary>
        /// <returns>A <see cref="TaskModel"/> object.</returns>
        [HttpGet("allUserTasks")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerResponse(200, "The tasks were found")]
        [SwaggerResponse(401, "An unauthorized request cannot be processed.")]
        [SwaggerResponse(500, "Something went wrong")]
        public async Task<ActionResult<IReadOnlyList<TaskModel>>> GetTasksByUser()
        {
            var tasks = await this.tasksService.GetAllTasksAsync(this.UserId);
            return this.Ok(tasks);
        }

        /// <summary>
        /// Gets all tasks. The endpoint for test purposes.
        /// </summary>
        /// <returns>A <see cref="TaskModel"/> object.</returns>
        [HttpGet("allTasksTest")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerResponse(200, "The task was found")]
        [SwaggerResponse(500, "Something went wrong")]
        public async Task<ActionResult<IReadOnlyList<TaskModel>>> GetAllTasksTest()
        {
            var tasks = await this.tasksService.GetAllTasksAsyncTest();
            return this.Ok(tasks);
        }

        /// <summary>
        /// Endpoint to create a task.
        /// </summary>
        /// <param name="task">Gets as a parameter Task create view model.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpPost("postTask")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerResponse(201, "The task was created")]
        [SwaggerResponse(401, "An unauthorized request cannot be processed.")]
        [SwaggerResponse(500, "Something went wrong")]
        public async Task<ActionResult<TaskModel>> PostTask([FromBody] TaskModel task)
        {
            task.UserId = this.UserId;
            var result = await this.tasksService.PostTask(task);
            return this.CreatedAtAction(nameof(this.GetTaskById), new { id = result.TaskId }, result);
        }

        /// <summary>
        /// Endpoint to delete a task.
        /// </summary>
        /// <param name="id">Represents an ID of the task that needs to be deleted.</param>
        /// <param name="task">View model of a task that needs to be deleted.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpDelete("deleteTask/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerResponse(200, "The task was created")]
        [SwaggerResponse(400, "The id in the view model and the id in the request are not the same")]
        [SwaggerResponse(401, "An unauthorized request cannot be processed.")]
        [SwaggerResponse(404, "The task you want to delete wan`t found.")]
        [SwaggerResponse(500, "Something went wrong")]
        public async Task<ActionResult> DeleteTask(
            Guid id,
            [FromBody] TaskModel task)
        {
            task.UserId = this.UserId;

            if (id != task.TaskId)
            {
                return this.BadRequest();
            }

            await this.tasksService.DeleteTask(task);
            return this.Ok();
        }

        /// <summary>
        /// Endpoint to update a task.
        /// </summary>
        /// <param name="id">Represents an ID of the task that needs to be updated.</param>
        /// <param name="task">View model of a task that needs to be updated.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpPut("updateTask/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerResponse(200, "The task was created")]
        [SwaggerResponse(400, "The id in the view model and the id in the request are not the same")]
        [SwaggerResponse(401, "An unauthorized request cannot be processed.")]
        [SwaggerResponse(404, "The task you want to update wan`t found.")]
        [SwaggerResponse(500, "Something went wrong")]
        public async Task<ActionResult> UpdateTask(
            Guid id,
            [FromBody] TaskModel task)
        {
            if (id != task.TaskId)
            {
                return this.BadRequest();
            }

            task.UserId = this.UserId;
            await this.tasksService.UpdateTask(task);
            return this.Ok();
        }
    }
}
