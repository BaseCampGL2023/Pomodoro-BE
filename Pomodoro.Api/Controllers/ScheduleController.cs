// <copyright file="ScheduleController.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Authorization;
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
    /// Manage schedule.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ScheduleController : BaseCrudController<ScheduleService, Schedule, ScheduleModel, IScheduleRepository>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduleController"/> class.
        /// </summary>
        /// <param name="service">Instance of Schedule service.</param>
        public ScheduleController(ScheduleService service)
            : base(service)
        {
        }

        // TODO : Add remarks to describe endpoints.

        /// <summary>
        /// Update schedule only if ScheduleType, template, start and finish date don't change,
        /// otherwise create new Schedule, or delete all related tasks.
        /// </summary>
        /// <param name="id">Object id.</param>
        /// <param name="model">Exisitng object.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpPut("own/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [SwaggerResponse(204, "Update successfully")]
        [SwaggerResponse(400, "No schedule with such id for this user")]
        [SwaggerResponse(403, "This schedule not belong to current user")]
        [SwaggerResponse(409, "Schedule has planned or performed tasks - delete them or create new schedule instead")]
        public override async Task<ActionResult> UpdateOne(Guid id, ScheduleModel model)
        {
            if (model.StartDt < this.UserCreatedAt)
            {
                return this.BadRequest("Schedule start datetime cannot set before user registration date.");
            }

            return await base.UpdateOne(id, model);
        }

        /// <summary>
        /// Persist new belonging to user object.
        /// </summary>
        /// <param name="model">New object.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [SwaggerResponse(201, "Object created")]
        [SwaggerResponse(400, "The request was invalid")]
        [SwaggerResponse(409, "This schedule intersect with tasks from other schedule")]
        public override async Task<ActionResult<ScheduleModel>> AddOne([FromBody] ScheduleModel model)
        {
            if (model.StartDt < this.UserCreatedAt)
            {
                return this.BadRequest("Schedule start datetime cannot set before user registration date.");
            }

            return await base.AddOne(model);
        }

        /// <summary>
        /// Retrieve Schedule owned by autheticated user with tasks. 
        /// </summary>
        /// <param name="id">Schedule id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpGet("own/with-tasks/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerResponse(200, "The execution was successful")]
        [SwaggerResponse(403, "This schedule object don't belong to current user")]
        [SwaggerResponse(404, "Schedule not found")]
        public async Task<ActionResult<CategoryModel>> GetByIdWithTasks(Guid id)
        {
            var result = await this.Service.GetScheduleWithTasksAsync(id, this.UserId);

            return this.MapServiceResponse(result);
        }

        /// <summary>
        /// Return all schedules belonging to user that don't have any tasks.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpGet("own/empty")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerResponse(200, "All schedules belonging to user that don't have any tasks.")]
        public async Task<ActionResult<ICollection<CategoryModel>>> GetOwnAllEmpty()
        {
            return this.Ok(await this.Service.GetEmptySchedulesAsync(this.UserId));
        }

        /// <summary>
        /// Return all schedules belonging to user that don't have any future tasks.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpGet("own/completed")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerResponse(200, "All schedules belonging to user that don't have any future tasks.")]
        public async Task<ActionResult<ICollection<CategoryModel>>> GetOwnAllCompleted()
        {
            return this.Ok(await this.Service.GetCompletedSchedulesAsync(this.UserId));
        }

        /// <summary>
        /// Return all schedules belonging to user that have any future tasks.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpGet("own/active")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerResponse(200, "All schedules belonging to user that have any future tasks.")]
        public async Task<ActionResult<ICollection<CategoryModel>>> GetOwnAllActive()
        {
            return this.Ok(await this.Service.GetActiveSchedulesAsync(this.UserId));
        }
    }
}
