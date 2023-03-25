// <copyright file="ITimerSettingRepository.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Pomodoro.Dal.Entities;
using Pomodoro.Dal.Repositories.Base;

namespace Pomodoro.Dal.Repositories.Interfaces
{
    /// <summary>
    /// Providing operations with TimerSettings objects.
    /// </summary>
    public interface ITimerSettingRepository : IBelongRepository<TimerSettings>
    {
    }
}
