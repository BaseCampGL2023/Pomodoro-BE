// <copyright file="TimerSettingRepository.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using Pomodoro.Dal.Data;
using Pomodoro.Dal.Entities;
using Pomodoro.Dal.Repositories.Base;
using Pomodoro.Dal.Repositories.Interfaces;

namespace Pomodoro.Dal.Repositories
{
    /// <inheritdoc/>
    public class TimerSettingRepository : BelongRepository<TimerSettings>, ITimerSettingRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimerSettingRepository"/> class.
        /// </summary>
        /// <param name="context">Instance of AppDbContext class <see cref="AppDbContext"/>.</param>
        public TimerSettingRepository(AppDbContext context)
            : base(context)
        {
        }
    }
}
