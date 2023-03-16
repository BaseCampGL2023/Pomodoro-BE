// <copyright file="TasksController.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pomodoro.Api.Controllers.Base;
using Pomodoro.Core.Interfaces.IServices;
using Pomodoro.Core.Models.Tasks;
using Swashbuckle.AspNetCore.Annotations;

namespace Pomodoro.Api.Controllers
{
    /// <summary>
    /// API controller for managing the tasks.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        /// <param name="id">The id of the task that needs to be returned.</param>
        /// <returns>A <see cref="TaskViewModel"/> object.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerResponse(200, "The task was found")]
        [SwaggerResponse(400, "Invalid data")]
        [SwaggerResponse(404, "The task with this id wasn`t found")]
        [SwaggerResponse(500, "Something went wrong")]
        public async Task<ActionResult<TaskViewModel>> GetTaskById(Guid id)
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

            return this.Ok(this.mapper.Map<TaskViewModel>(task));
        }

        /// <summary>
        /// Gets all tasks by user by specific date or period.
        /// </summary>
        /// <param name="startDate">The start date for which tasks should be found.</param>
        /// <param name="endDate">The end date for which tasks should be found.</param>
        /// <returns>A <see cref="TaskViewModel"/> object.</returns>
        [HttpGet("ByDate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerResponse(200, "The tasks were found")]
        [SwaggerResponse(400, "The date in the url is invalid")]
        [SwaggerResponse(401, "An unauthorized request cannot be processed.")]
        [SwaggerResponse(404, "The tasks by with date weren`t found.")]
        [SwaggerResponse(500, "Something went wrong")]
        public async Task<ActionResult<IReadOnlyList<TaskViewModel>>> GetTasksByDate(DateTime startDate, DateTime endDate)
        {
            var tasks = await this.tasksService.GetAllTasksByDate(this.UserId, startDate, endDate);
            return this.Ok(this.mapper.Map<IReadOnlyList<TaskViewModel>>(tasks));
        }

        /// <summary>
        /// Gets all tasks by user.
        /// </summary>
        /// <returns>A <see cref="TaskViewModel"/> object.</returns>
        [HttpGet("allUserTasks")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerResponse(200, "The tasks were found")]
        [SwaggerResponse(401, "An unauthorized request cannot be processed.")]
        [SwaggerResponse(500, "Something went wrong")]
        public async Task<ActionResult<IReadOnlyList<TaskViewModel>>> GetTasksByUser()
        {
            var tasks = await this.tasksService.GetAllTasksAsync(this.UserId);
            return this.Ok(this.mapper.Map<IReadOnlyList<TaskViewModel>>(tasks));
        }

        /// <summary>
        /// Gets all tasks. The endpoint for test purposes.
        /// </summary>
        /// <returns>A <see cref="TaskViewModel"/> object.</returns>
        [HttpGet("allTasksTest")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerResponse(200, "The task was found")]
        [SwaggerResponse(500, "Something went wrong")]
        public async Task<ActionResult<IReadOnlyList<TaskViewModel>>> GetAllTasksTest()
        {
            var tasks = await this.tasksService.GetAllTasksAsyncTest();
            return this.Ok(this.mapper.Map<IReadOnlyList<TaskViewModel>>(tasks));
        }

        /// <summary>
        /// Endpoint to create a task.
        /// </summary>
        /// <param name="task">Gets as a parameter Task create view model.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerResponse(201, "The task was created")]
        [SwaggerResponse(401, "An unauthorized request cannot be processed.")]
        [SwaggerResponse(500, "Something went wrong")]
        public async Task<ActionResult<TaskViewModel>> PostTask([FromBody] TaskViewModel task)
        {
            task.UserId = this.UserId;
            var result = await this.tasksService.PostTask(this.mapper.Map<TaskModel>(task));
            return this.CreatedAtAction(nameof(this.GetTaskById), new { id = result.Id }, this.mapper.Map<TaskViewModel>(result));
        }

        /// <summary>
        /// Endpoint to delete a task.
        /// </summary>
        /// <param name="id">Represents an ID of the task that needs to be deleted.</param>
        /// <param name="task">View model of a task that needs to be deleted.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpDelete("{id}")]
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
            [FromBody] TaskViewModel task)
        {
            task.UserId = this.UserId;

            if (id != task.TaskId)
            {
                return this.BadRequest();
            }

            await this.tasksService.DeleteTask(this.mapper.Map<TaskModel>(task));
            return this.Ok();
        }

        /// <summary>
        /// Endpoint to update a task.
        /// </summary>
        /// <param name="id">Represents an ID of the task that needs to be updated.</param>
        /// <param name="task">View model of a task that needs to be updated.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpPut("{id}")]
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
            [FromBody] TaskViewModel task)
        {
            if (id != task.TaskId)
            {
                return this.BadRequest();
            }

            task.UserId = this.UserId;
            await this.tasksService.UpdateTask(this.mapper.Map<TaskModel>(task));
            return this.Ok();
        }
    }
}
