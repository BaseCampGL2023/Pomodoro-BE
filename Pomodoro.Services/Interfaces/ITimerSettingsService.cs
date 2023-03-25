// <copyright file="ITimerSettingsService.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Pomodoro.Dal.Entities;
using Pomodoro.Dal.Repositories.Interfaces;
using Pomodoro.Services.Base;
using Pomodoro.Services.Models;
using Pomodoro.Services.Models.Results;

namespace Pomodoro.Services.Interfaces
{
    public interface ITimerSettingsService
        : IBaseService<TimerSettings, TimerSettingsModel, ITimerSettingRepository>
    {
        /// <summary>
        /// Retrieve current user settings.
        /// </summary>
        /// <param name="userId">Owner Id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public Task<ServiceResponse<TimerSettingsModel>> GetOwnActiveAsync(Guid userId);
    }
}
