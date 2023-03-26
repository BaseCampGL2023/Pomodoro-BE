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
    public class ScheduleController : BaseController<ScheduleService, Schedule, ScheduleModel, IScheduleRepository>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduleController"/> class.
        /// </summary>
        /// <param name="service">Instance of Schedule service.</param>
        public ScheduleController(ScheduleService service)
            : base(service)
        {
        }

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
        [SwaggerResponse(204, "Update successfully")]
        [SwaggerResponse(400, "No schedule with such id for this user")]
        public override async Task<ActionResult> UpdateOne(Guid id, ScheduleModel model)
        {
            return await base.UpdateOne(id, model);
        }
    }
}
