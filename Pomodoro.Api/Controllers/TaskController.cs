// <copyright file="TaskController.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;
using Pomodoro.Api.Controllers.Base;
using Pomodoro.Dal.Entities;
using Pomodoro.Dal.Repositories.Interfaces;
using Pomodoro.Services;
using Pomodoro.Services.Models;
using Pomodoro.Services.Models.Query;
using Pomodoro.Services.Models.Results;
using Swashbuckle.AspNetCore.Annotations;

namespace Pomodoro.Api.Controllers
{
    /// <summary>
    /// Manage tasks.
    /// </summary>
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

        /// <summary>
        /// Return all tasks corresponds to query (filtering by task execution state, relation to schedules,
        /// task start dateTime).
        /// </summary>
        /// <remarks>
        /// <code>
        /// Execution state enum:
        /// 1 - means any task (started or pristine or finished), default value;
        /// 2 - means not finished tasks (both started and pristine);
        /// 3 - means tasks that already in progress (have pomodoros);
        /// 4 - means tasks that don't be started;
        /// 5 - means task that already performed.
        /// Repeatable enum:
        /// 1 - (Any) both scheduled and standalone tasks, default value;
        /// 2 - (Routine) scheduled task (generated accomplish to existing schedule);
        /// 3 - (Alone) "standalone" task.
        /// Start date, end date:
        /// if provided both start and end datetime - retrieved tasks with startDt between this two datetimes;
        /// if provided only start datetime - retrieved tasks with startDt above start datetime;
        /// if provided only end datetime = retrieved tasks with startDt lower than end datetime;
        /// if nor start or end date time provided - retrieve tasks for current day (utc 0:00 - 24:00).
        /// </code>
        /// </remarks>
        /// <param name="query">Query model <see cref="TaskQueryModel"/>.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpGet("own/filter")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerResponse(200, "Retrieved all user's objects.")]
        public async Task<ActionResult<ICollection<TaskModel>>> GetOwnAll([FromQuery] TaskQueryModel query)
        {
            return this.Ok(await this.Service.GetOwnByQueryAsync(this.UserId, query));
        }

        /// <summary>
        /// Add new pomodoro.
        /// </summary>
        /// <param name="model">New object.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpPost("pomodoro")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(201, "Object created")]
        [SwaggerResponse(400, "The request was invalid")]
        public virtual async Task<ActionResult<PomoModel>> AddPomodoro([FromBody] PomoModel model)
        {
            var result = await this.Service.AddPomodoro(model, this.UserId);
            if (result.Result == ResponseType.Ok)
            {
                return this.CreatedAtAction(nameof(this.GetById), new { model.Id }, model);
            }

            return this.MapServiceResponse(result);
        }
    }
}
