// <copyright file="SettingsController.cs" company="PomodoroGroup_GL_BaseCamp">
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
    /// API controller for managing pomodoro settings related to the current user.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SettingsController : BaseController
    {
        private readonly IMapper mapper;
        private readonly ISettingsService settingsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsController"/> class.
        /// </summary>
        /// <param name="mapper">Interface for mapping 2 objects.</param>
        /// <param name="settingsService">Service for managing user settings.</param>
        public SettingsController(IMapper mapper, ISettingsService settingsService)
        {
            this.mapper = mapper;
            this.settingsService = settingsService;
        }

        /// <summary>
        /// Gets pomodoro settings related to the current user.
        /// </summary>
        /// <returns>A <see cref="SettingsViewModel"/> object.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerResponse(200, "Returns an object containing user settings.")]
        [SwaggerResponse(401, "An unauthorized request cannot be processed.")]
        [SwaggerResponse(404, "No settings found for the current user.")]
        [SwaggerResponse(500, "An unhandled exception occurred on the server while executing the request.")]
        public async Task<ActionResult<SettingsViewModel>> GetUserSettings()
        {
            var settingsModel = await this.settingsService
                .GetUserSettingsAsync(this.UserId);

            if (settingsModel is null)
            {
                return this.NotFound("No settings found for the current user.");
            }

            var settingsViewModel = this.mapper.Map<SettingsViewModel>(settingsModel);
            return this.Ok(settingsViewModel);
        }

        /// <summary>
        /// Creates pomodoro settings related to the current user.
        /// </summary>
        /// <param name="settingsViewModel">Represents object to be created.</param>
        /// <returns>Created object represented by <see cref="SettingsViewModel"/>.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerResponse(201, "Returns created object containing user settings.")]
        [SwaggerResponse(400, "The model state is invalid.")]
        [SwaggerResponse(401, "An unauthorized request cannot be processed.")]
        [SwaggerResponse(403, "User cannot create settings for another person.")]
        [SwaggerResponse(409, "Pomodoro settings for the current user already exist.")]
        [SwaggerResponse(500, "An unhandled exception occurred on the server while executing the request.")]
        public async Task<ActionResult<SettingsViewModel>> CreateSettings([FromBody] SettingsViewModel settingsViewModel)
        {
            if (this.UserId != settingsViewModel.UserId)
            {
                return this.Forbid();
            }

            var userSettings = await this.settingsService
                .GetUserSettingsAsync(this.UserId);
            if (userSettings != null)
            {
                return this.Conflict("Pomodoro settings for the current user already exist.");
            }

            var settingsModel = this.mapper.Map<SettingsModel>(settingsViewModel);
            settingsModel = await this.settingsService.CreateSettingsAsync(settingsModel);

            var createdViewModel = this.mapper.Map<SettingsViewModel>(settingsModel);
            return this.CreatedAtAction(nameof(this.GetUserSettings), createdViewModel);
        }

        /// <summary>
        /// Updates pomodoro settings related to the current user.
        /// </summary>
        /// <param name="id">Id of the pomodoro settings.</param>
        /// <param name="settingsViewModel">Represents object to be updated.</param>
        /// <returns>Updated object represented by <see cref="SettingsViewModel"/>.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerResponse(202, "Returns updated object containing user settings.")]
        [SwaggerResponse(400, "The model state is invalid or there is a mismatch.")]
        [SwaggerResponse(401, "An unauthorized request cannot be processed.")]
        [SwaggerResponse(403, "User cannot update settings of another person.")]
        [SwaggerResponse(404, "No settings found by the provided id.")]
        [SwaggerResponse(500, "An unhandled exception occurred on the server while executing the request.")]
        public async Task<ActionResult<SettingsViewModel>> UpdateSettings(
            Guid id,
            [FromBody] SettingsViewModel settingsViewModel)
        {
            if (this.UserId != settingsViewModel.UserId)
            {
                return this.Forbid();
            }

            if (id != settingsViewModel.Id)
            {
                return this.BadRequest("SettingsId mismatch.");
            }

            var modelToUpdate = await this.settingsService.GetSettingsAsync(id);
            if (modelToUpdate is null)
            {
                return this.NotFound("No settings found by the provided id.");
            }

            var model = this.mapper.Map<SettingsModel>(settingsViewModel);
            model = await this.settingsService.UpdateSettingsAsync(model);

            var updated = this.mapper.Map<SettingsViewModel>(model);
            return this.AcceptedAtAction(nameof(this.GetUserSettings), updated);
        }
    }
}
