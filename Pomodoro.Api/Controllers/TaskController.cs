// <copyright file="TaskController.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pomodoro.Api.Controllers.Base;
using Pomodoro.Dal.Entities;
using Pomodoro.Dal.Repositories.Interfaces;
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
    public class TaskController : BaseCrudController<TaskService, AppTask, TaskModel, IAppTaskRepository>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskController"/> class.
        /// </summary>
        /// <param name="service">Instance of Task service.</param>
        public TaskController(TaskService service)
            : base(service)
        {
        }

        /// <summary>
        /// Persist new belonging to user Task.
        /// </summary>
        /// <param name="model">New task.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(201, "Task created")]
        [SwaggerResponse(400, "The request was invalid")]
        public override async Task<ActionResult<TaskModel>> AddOne([FromBody] TaskModel model)
        {
            if (model.StartDt < this.UserCreatedAt)
            {
                return this.BadRequest("Task start datetime cannot set before user registration date.");
            }

            return await base.AddOne(model);
        }

        /// <summary>
        /// Update existing task.
        /// </summary>
        /// <param name="id">Task id.</param>
        /// <param name="model">Exisitng Task.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpPut("own/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(200, "Update successfully")]
        [SwaggerResponse(400, "No schedule with such id for this user")]
        public override async Task<ActionResult> UpdateOne(Guid id, [FromBody] TaskModel model)
        {
            if (model.StartDt < this.UserCreatedAt)
            {
                return this.BadRequest("Task start datetime cannot set before user registration date.");
            }

            return await base.UpdateOne(id, model);
        }
    }
}
