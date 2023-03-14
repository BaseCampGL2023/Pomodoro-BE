// <copyright file="TimerSettingsController.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pomodoro.Api.Controllers.Base;
using Pomodoro.Services;
using Pomodoro.Services.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace Pomodoro.Api.Controllers
{
    /// <summary>
    /// Manage tracker settings.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TimerSettingsController : BaseController
    {
        private readonly TimerSettingsService timerSettingsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerSettingsController"/> class.
        /// </summary>
        /// <param name="service">Instance of TimerSettingsService <see cref="TimerSettingsService"/>.</param>
        public TimerSettingsController(TimerSettingsService service)
        {
            this.timerSettingsService = service;
        }

        /// <summary>
        /// Return active settings to specified user, or 404 status if not exist active settings.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpGet("own")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerResponse(200, "The execution was successful")]
        [SwaggerResponse(404, "Settings not found")]
        public async Task<ActionResult<TimerSettingsModel>> GetOwnCurrent()
        {
            var result = await this.timerSettingsService.GetBelongActiveAsync(this.UserId);
            if (result is null)
            {
                return this.NotFound("No active settings for now");
            }

            return this.Ok(result);
        }

        /// <summary>
        /// Persist tracker settings for user.
        /// </summary>
        /// <param name="model">Tracker settings <see cref="TimerSettingsModel"/>.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(200, "The execution was succesful")]
        [SwaggerResponse(400, "The request was invalid")]
        public async Task<ActionResult<TimerSettingsModel>> AddOne(TimerSettingsModel model)
        {
            var result = await this.timerSettingsService.AddSettingsAsync(model, this.UserId);
            if (result)
            {
                return this.CreatedAtAction(nameof(this.GetOwnCurrent), model);
            }

            return this.BadRequest(model);
        }

        /// <summary>
        /// Retrive all user settings.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerResponse(200, "Retrieved all user's settings")]
        public async Task<ActionResult<ICollection<TimerSettingsModel>>> GetOwnAll()
        {
            return this.Ok(await this.timerSettingsService.GetBelongAllAsync(this.UserId));
        }

        /// <summary>
        /// Delete timer settings object belonging to user.
        /// </summary>
        /// <param name="id">TImer settings object.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(204, "Delete successfully")]
        [SwaggerResponse(400, "No settings with such id for this user")]
        public async Task<ActionResult> DeleteOne(Guid id)
        {
            bool result = await this.timerSettingsService.DeleteOneOwnAsync(id, this.UserId);
            return result ? this.NoContent() : this.BadRequest(id);
        }
    }
}
