// <copyright file="TimerSettingsService.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;
using Pomodoro.Dal.Entities;
using Pomodoro.Dal.Repositories.Interfaces;
using Pomodoro.Services.Base;
using Pomodoro.Services.Interfaces;
using Pomodoro.Services.Models;

namespace Pomodoro.Services
{
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
    }
}
