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
        /// <summary>
        /// Return active settings to specified user.
        /// </summary>
        /// <param name="ownerId">Owner id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public Task<TimerSettings?> GetCurrentTimerSettingsAsync(Guid ownerId);
    }
}
