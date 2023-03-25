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
    /// <summary>
    /// Perform operations with timer settings.
    /// </summary>
    public interface ITimerSettingsService
        : IBaseService<TimerSettings, TimerSettingsModel, ITimerSettingRepository>
    {
    }
}
