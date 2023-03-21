// <copyright file="ScheduleController.cs" company="PomodoroGroup_GL_BaseCamp">
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
    /// Manage schedule.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ScheduleController : BaseController
    {
        private readonly ScheduleService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduleController"/> class.
        /// </summary>
        /// <param name="service">Instance of Schedule service.</param>
        public ScheduleController(ScheduleService service)
        {
            this.service = service;
        }

        // TODO: add 401 response

        /// <summary>
        /// Return all user schedules.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerResponse(200, "Retrieved all user's schedules")]
        public async Task<ActionResult<ICollection<ScheduleModel>>> GetOwnAll()
        {
            return this.Ok(await this.service.GetOwnAllAsync(this.UserId));
        }

        /// <summary>
        /// Return schedule by id, or 404 if not exist.
        /// </summary>
        /// <param name="id">Schedule id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerResponse(200, "The execution was successful")]
        [SwaggerResponse(403, "This shedule don't belong to current user")]
        [SwaggerResponse(404, "Schedule not found")]
        public async Task<ActionResult<ScheduleModel>> GetById(Guid id)
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
        /// Persist new user schedule.
        /// </summary>
        /// <param name="model">New schedule.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(201, "Schedule created")]
        [SwaggerResponse(400, "The request was invalid")]
        public async Task<ActionResult<ScheduleModel>> AddOne(ScheduleModel model)
        {
            var result = await this.service.AddOneOwnAsync(model, this.UserId);
            if (result)
            {
                return this.CreatedAtAction(nameof(this.GetById), new { model.Id }, model);
            }

            return this.BadRequest(model);
        }

        /// <summary>
        /// Delete schedule by id.
        /// </summary>
        /// <param name="id">Schedule id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(204, "Delete successfully")]
        [SwaggerResponse(400, "No schedule with such id for this user")]
        public async Task<ActionResult> DeleteOne(Guid id)
        {
            var result = await this.service.DeleteOneOwnAsync(id, this.UserId);
            return result ? this.NoContent() : this.BadRequest(id);
        }

        /// <summary>
        /// Update existing schedule.
        /// </summary>
        /// <param name="id">Task id.</param>
        /// <param name="model">Existing task.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(204, "Update successfully")]
        [SwaggerResponse(400, "No schedule with such id for this user")]
        public async Task<ActionResult> UpdateOne(Guid id, ScheduleModel model)
        {
            if (id != model.Id)
            {
                return this.BadRequest();
            }

            var result = await this.service.UpdateOneOwnAsync(model, this.UserId);
            return result ? this.NoContent() : this.BadRequest(id);
        }
    }
}
