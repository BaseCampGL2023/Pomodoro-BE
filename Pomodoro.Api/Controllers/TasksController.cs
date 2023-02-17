// <copyright file="TasksController.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Pomodoro.Api.Controllers.Base;
using Pomodoro.Api.ViewModels.Tasks;
using Pomodoro.Core.Interfaces.IServices;
using Pomodoro.Core.Models.Tasks;
using Pomodoro.DataAccess.Entities;
using Pomodoro.DataAccess.Repositories.Interfaces;
using Pomodoro.Services.Services;
using Swashbuckle.AspNetCore.Annotations;
using System.Runtime.CompilerServices;

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
        /// <returns>A <see cref="TaskToReturnModel"/> object.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerResponse(200, "The task was found")]
        [SwaggerResponse(400, "Invalid data")]
        [SwaggerResponse(404, "The task with this id wasn`t found")]
        [SwaggerResponse(500, "Something went wrong")]
        public async Task<ActionResult<TaskToReturnModel>> GetTaskById(Guid id)
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

            return this.Ok(this.mapper.Map<TaskModel, TaskToReturnModel>(task));
        }

        /// <summary>
        /// Gets all tasks by user.
        /// </summary>
        /// <returns>A <see cref="TaskToReturnModel"/> object.</returns>
        [HttpGet("allUserTasks")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerResponse(200, "The tasks were found")]
        [SwaggerResponse(401, "An unauthorized request cannot be processed.")]
        [SwaggerResponse(500, "Something went wrong")]
        public async Task<ActionResult<IReadOnlyList<TaskToReturnModel>>> GetTasksByUser()
        {
            /*if (this.UserId == Guid.Empty)
            {
                this.Unauthorized();
            }*/
            var UserId = new Guid("6FF9D0D2-BB26-468C-E841-08DB10C9B2BE");
            var tasks = await this.tasksService.GetAllTasksAsync(UserId);
            return this.Ok(this.mapper.Map<IEnumerable<TaskModel>, IEnumerable<TaskToReturnModel>>(tasks));
        }

        /// <summary>
        /// Gets all tasks. The endpoint for test purposes.
        /// </summary>
        /// <returns>A <see cref="TaskToReturnModel"/> object.</returns>
        [HttpGet("allTasksTest")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerResponse(200, "The task was found")]
        [SwaggerResponse(500, "Something went wrong")]
        public async Task<ActionResult<IReadOnlyList<TaskToReturnModel>>> GetAllTasksTest()
        {
            var tasks = await this.tasksService.GetAllTasksAsyncTest();
            return this.Ok(this.mapper.Map<IEnumerable<TaskModel>, IEnumerable<TaskToReturnModel>>(tasks));
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
        public async Task<ActionResult<TaskToReturnModel>> PostTask(TaskToCreateModel task)
        {
            var data = this.mapper.Map<TaskToCreateModel, TaskModel>(task);
            data.UserId = new Guid("6FF9D0D2-BB26-468C-E841-08DB10C9B2BE");

            /*if (this.UserId == Guid.Empty)
            {
                this.Unauthorized();
            }*/

            var result = await this.tasksService.PostTask(data);
            return this.CreatedAtAction(nameof(this.GetTaskById), new { id = result.TaskId }, result);
        }

        /// <summary>
        /// Endpoint to delete a task.
        /// </summary>
        /// <param name="task">Gets as a parameter Task view model.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpDelete("deleteTask")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerResponse(200, "The task was created")]
        [SwaggerResponse(401, "An unauthorized request cannot be processed.")]
        [SwaggerResponse(404, "The task you want to delete wan`t found.")]
        [SwaggerResponse(500, "Something went wrong")]
        public async Task<ActionResult> DeleteTask(TaskToManipulateModel task)
        {
            var data = this.mapper.Map<TaskToManipulateModel, TaskModel>(task);
            data.UserId = new Guid("6FF9D0D2-BB26-468C-E841-08DB10C9B2BE");

            try
            {
                await this.tasksService.DeleteTask(data);
            }
            catch (Exception ex)
            {
                return this.NotFound(ex);
            }

            return this.Ok();
        }

        /// <summary>
        /// Endpoint to update a task.
        /// </summary>
        /// <param name="task">Gets as a parameter Task view model.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpPut("updateTask")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerResponse(200, "The task was created")]
        [SwaggerResponse(401, "An unauthorized request cannot be processed.")]
        [SwaggerResponse(404, "The task you want to update wan`t found.")]
        [SwaggerResponse(500, "Something went wrong")]
        public async Task<ActionResult> UpdateTask(TaskToManipulateModel task)
        {
            var data = this.mapper.Map<TaskToManipulateModel, TaskModel>(task);
            data.UserId = new Guid("6FF9D0D2-BB26-468C-E841-08DB10C9B2BE");
            await this.tasksService.UpdateTask(data);
            return this.Ok();
        }
    }
}
