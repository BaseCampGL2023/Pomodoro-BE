// <copyright file="TimerSettingsController.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pomodoro.Api.Controllers.Base;
using Pomodoro.Dal.Entities;
using Pomodoro.Dal.Repositories.Base;
using Pomodoro.Dal.Repositories.Interfaces;
using Pomodoro.Services;
using Pomodoro.Services.Base;
using Pomodoro.Services.Interfaces;
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
    public class TimerSettingsController : BaseController<ITimerSettingsService, TimerSettings, TimerSettingsModel, ITimerSettingRepository>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimerSettingsController"/> class.
        /// </summary>
        /// <param name="service">Instance of TimerSettings service.</param>
        public TimerSettingsController(TimerSettingsService service)
            : base(service)
        {
        }

        /// <summary>
        /// Return active settings to specified user, or 404 status if not exist active settings.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpGet("active")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerResponse(200, "The execution was successful")]
        [SwaggerResponse(404, "Settings not found")]
        public async Task<ActionResult<TimerSettingsModel>> GetOwnCurrent()
        {
            var result = await this.Service.GetOwnActiveAsync(this.UserId);
            return this.MapServiceResponse(result);
        }
    }
}
