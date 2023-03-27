// <copyright file="TasksController.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pomodoro.Api.Controllers.Base;
using Pomodoro.Api.ViewModels;
using Pomodoro.Core.Interfaces.IServices;
using Pomodoro.Core.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace Pomodoro.Api.Controllers
{
    /// <summary>
    /// API controller for managing tasks related to the current user.
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
        /// <param name="mapper">Interface for mapping 2 objects.</param>
        /// <param name="tasksService">Service for managing user tasks.</param>
        public TasksController(
            ITaskService tasksService,
            IMapper mapper)
        {
            this.tasksService = tasksService;
            this.mapper = mapper;
        }

        /// <summary>
        /// Gets task related to the current user.
        /// </summary>
        /// <param name="id">Id of the task.</param>
        /// <returns>A <see cref="TaskViewModel"/> object.</returns>
        [HttpGet("getById/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerResponse(200, "Returns object containing user task.")]
        [SwaggerResponse(400, "The model state is invalid.")]
        [SwaggerResponse(401, "An unauthorized request cannot be processed.")]
        [SwaggerResponse(404, "No task found by the provided id.")]
        [SwaggerResponse(500, "An unhandled exception occurred on the server while executing the request.")]
        public async Task<ActionResult<TaskViewModel>> GetTaskById(Guid id)
        {
            if (id == Guid.Empty)
            {
                return this.BadRequest("Task id is empty.");
            }

            var task = await this.tasksService.GetTaskByIdAsync(id);
            if (task == null)
            {
                return this.NotFound("Task wasn`t found.");
            }

            return this.Ok(this.mapper.Map<TaskViewModel>(task));
        }

        /// <summary>
        /// Gets tasks related to the current user by specific date.
        /// </summary>
        /// <param name="date">Date of the tasks.</param>
        /// <returns>A <see cref="IEnumerable{TaskForListViewModel}"/> object.</returns>
        [HttpGet("getByDate/{date}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerResponse(200, "Returns object containing user tasks.")]
        [SwaggerResponse(400, "The model state is invalid.")]
        [SwaggerResponse(401, "An unauthorized request cannot be processed.")]
        [SwaggerResponse(404, "No tasks found by the provided date.")]
        [SwaggerResponse(500, "An unhandled exception occurred on the server while executing the request.")]
        public async Task<ActionResult<IEnumerable<TaskViewModel>>> GetTasksByDate(DateTime date)
        {
            var tasks = await this.tasksService.GetTasksByDateAsync(this.UserId, date);

            if (!tasks.Any())
            {
                return this.NotFound("Tasks weren`t found.");
            }

            return this.Ok(this.mapper.Map<IEnumerable<TaskViewModel>>(tasks));
        }

        /// <summary>
        /// Gets completed tasks related to the current user by specific date.
        /// </summary>
        /// <param name="date">Date of the tasks.</param>
        /// <returns>A <see cref="IEnumerable{TaskForListViewModel}"/> object.</returns>
        [HttpGet("getCompletedByDate/{date}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerResponse(200, "Returns object containing user tasks.")]
        [SwaggerResponse(400, "The model state is invalid.")]
        [SwaggerResponse(401, "An unauthorized request cannot be processed.")]
        [SwaggerResponse(404, "No completed tasks found by the provided date.")]
        [SwaggerResponse(500, "An unhandled exception occurred on the server while executing the request.")]
        public async Task<ActionResult<IEnumerable<TaskViewModel>>> GetCompletedTasksByDate(DateTime date)
        {
            var tasks = await this.tasksService.GetCompletedTasksByDateAsync(this.UserId, date);

            if (!tasks.Any())
            {
                return this.NotFound("Tasks weren`t found.");
            }

            return this.Ok(this.mapper.Map<IEnumerable<TaskViewModel>>(tasks));
        }

        /// <summary>
        /// Creates task related to the current user.
        /// </summary>
        /// <param name="task">Represents object to be created.</param>
        /// <returns>Id of the created object represented by<see cref="ActionResult{Guid}"/>.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerResponse(201, "Returns created object containing task id.")]
        [SwaggerResponse(400, "The model state is invalid.")]
        [SwaggerResponse(401, "An unauthorized request cannot be processed.")]
        [SwaggerResponse(500, "An unhandled exception occurred on the server while executing the request.")]
        public async Task<ActionResult<TaskViewModel>> CreateTask([FromBody] TaskViewModel task)
        {
            var taskModel = this.mapper.Map<TaskModel>(task);

            taskModel.UserId = this.UserId;

            try
            {
                taskModel = await this.tasksService.CreateTaskAsync(taskModel);
            }
            catch (Exception)
            {
                return this.BadRequest();
            }

            var result = this.mapper.Map<TaskViewModel>(taskModel);

            return this.CreatedAtAction(nameof(this.GetTaskById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Deletes task related to the current user by task id.
        /// </summary>
        /// <param name="id">Represents an id of the task that needs to be deleted.</param>
        /// <returns>A <see cref="ActionResult"/> representing the result of task deletion.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerResponse(200, "Returns success result of task deletion.")]
        [SwaggerResponse(400, "The model state is invalid.")]
        [SwaggerResponse(401, "An unauthorized request cannot be processed.")]
        [SwaggerResponse(403, "User cannot delete task of another person.")]
        [SwaggerResponse(404, "No task found by the provided id.")]
        [SwaggerResponse(500, "An unhandled exception occurred on the server while executing the request.")]
        public async Task<ActionResult> DeleteTask(Guid id)
        {
            var task = await this.tasksService.GetTaskByIdAsync(id);

            if (task == null)
            {
                return this.NotFound("Task wasn`t found.");
            }

            if (task.UserId != this.UserId)
            {
                return this.Forbid("Can`t delete the task that doesn`t related to current user.");
            }

            try
            {
                await this.tasksService.DeleteTaskAsync(task);
            }
            catch (Exception)
            {
                return this.BadRequest();
            }

            return this.Ok();
        }

        /// <summary>
        /// Updates task related to the current user.
        /// </summary>
        /// <param name="id">Represents an id of the task that needs to be updated.</param>
        /// <param name="task">View model of a task that needs to be updated.</param>
        /// <returns>A <see cref="ActionResult"/> representing the result of the asynchronous operation.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerResponse(200, "Returns success result of task updation.")]
        [SwaggerResponse(400, "The model state is invalid.")]
        [SwaggerResponse(401, "An unauthorized request cannot be processed.")]
        [SwaggerResponse(403, "User cannot update task of another person.")]
        [SwaggerResponse(404, "No task found by the provided id.")]
        [SwaggerResponse(500, "An unhandled exception occurred on the server while executing the request.")]
        public async Task<ActionResult> UpdateTask(
            Guid id,
            [FromBody] TaskViewModel task)
        {
            var taskModel = await this.tasksService.GetTaskByIdAsync(id);

            if (taskModel == null)
            {
                return this.NotFound("Task wasn`t found.");
            }

            if (taskModel.UserId != this.UserId)
            {
                return this.Forbid("Can`t update the task that doesn`t related to current user.");
            }

            taskModel = this.mapper.Map<TaskModel>(task);

            try
            {
                taskModel = await this.tasksService.UpdateTaskAsync(taskModel);
            }
            catch (Exception)
            {
                return this.BadRequest();
            }

            var result = this.mapper.Map<TaskViewModel>(taskModel);

            return this.AcceptedAtAction(nameof(this.GetTaskById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Completes task related to the current user by task id.
        /// </summary>
        /// <param name="id">Represents an id of the task that needs to be completed.</param>
        /// <returns>A <see cref="ActionResult"/> representing the result of task completion.</returns>
        [HttpPut("{id}/completeTodayTask")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerResponse(200, "Returns success result of task completion.")]
        [SwaggerResponse(400, "The model state is invalid.")]
        [SwaggerResponse(401, "An unauthorized request cannot be processed.")]
        [SwaggerResponse(403, "User cannot complete task of another person.")]
        [SwaggerResponse(404, "No task found by the provided id.")]
        [SwaggerResponse(500, "An unhandled exception occurred on the server while executing the request.")]
        public async Task<ActionResult> CompleteTask(Guid id)
        {
            var task = await this.tasksService.GetTaskByIdAsync(id);

            if (task == null)
            {
                return this.NotFound("Task wasn`t found.");
            }

            if (task.UserId != this.UserId)
            {
                return this.Forbid("Can`t complete the task that doesn`t related to current user.");
            }

            try
            {
                await this.tasksService.CompleteTaskAsync(id);
            }
            catch (Exception)
            {
                return this.BadRequest();
            }

            return this.Ok();
        }

        /// <summary>
        /// Adds pomodoro to task that related to the current user.
        /// </summary>
        /// <param name="pomodoro">Represents object to be created.</param>
        /// <returns>Updated task of the created object represented by<see cref="ActionResult{TaskViewModel}"/>.</returns>
        [HttpPost("pomodoros")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerResponse(201, "Returns created object containing pomodoro id.")]
        [SwaggerResponse(400, "The model state is invalid.")]
        [SwaggerResponse(401, "An unauthorized request cannot be processed.")]
        [SwaggerResponse(500, "An unhandled exception occurred on the server while executing the request.")]
        public async Task<ActionResult<TaskViewModel>> AddPomodoro([FromBody] CompletedViewModel pomodoro)
        {
            var task = await this.tasksService.GetTaskByIdAsync(pomodoro.TaskId);

            if (task == null)
            {
                return this.NotFound("Task wasn`t found.");
            }

            if (task.UserId != this.UserId)
            {
                return this.Forbid("Can`t add pomodoro to task that doesn`t related to current user.");
            }

            var pomoModel = this.mapper.Map<CompletedModel>(pomodoro);

            TaskModel result;

            try
            {
                result = await this.tasksService.AddPomodoroToTaskAsync(pomoModel);
            }
            catch (Exception)
            {
                return this.BadRequest();
            }

            return this.Ok(this.mapper.Map<TaskViewModel>(result));
        }
    }
}
