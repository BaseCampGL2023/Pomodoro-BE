// <copyright file="TimerSettingsService.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;
using Pomodoro.Dal.Entities;
using Pomodoro.Dal.Repositories.Interfaces;
using Pomodoro.Services.Base;
using Pomodoro.Services.Interfaces;
using Pomodoro.Services.Models;
using Pomodoro.Services.Models.Results;

namespace Pomodoro.Services
{
    // TODO: check to not insert same settings
    // TODO: check IsActive in AddOne

    /// <summary>
    /// Perform operations with timer settings.
    /// </summary>
    public class TimerSettingsService : BaseService<TimerSettings, TimerSettingsModel, ITimerSettingRepository>, ITimerSettingsService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimerSettingsService"/> class.
        /// </summary>
        /// <param name="repo">Implementation of ITimerSettingRepository <see cref="ITimerSettingRepository"/>.</param>
        /// <param name="logger">Logger instance.</param>
        public TimerSettingsService(ITimerSettingRepository repo, ILogger<TimerSettingsService> logger)
            : base(repo, logger)
        {
        }

        /// <summary>
        /// Retrieve current user settings.
        /// </summary>
        /// <param name="userId">Owner Id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<ServiceResponse<TimerSettingsModel>> GetOwnActiveAsync(Guid userId)
        {
            var result = await this.Repo.GetCurrentTimerSettingsAsync(userId);
            if (result is null)
            {
                return new ServiceResponse<TimerSettingsModel> { Result = ResponseType.NotFound };
            }

            var data = new TimerSettingsModel();
            data.Assign(result);
            return new ServiceResponse<TimerSettingsModel>
            {
                Result = ResponseType.Ok,
                Data = data,
            };
        }
    }
}
