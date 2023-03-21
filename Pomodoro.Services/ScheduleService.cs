// <copyright file="ScheduleService.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Pomodoro.Dal.Entities;
using Pomodoro.Dal.Repositories.Interfaces;
using Pomodoro.Services.Base;
using Pomodoro.Services.Models;
using Pomodoro.Services.Models.Results;

namespace Pomodoro.Services
{
    /// <summary>
    /// Perform operations with schedules.
    /// </summary>
    public class ScheduleService : BaseService<Schedule, ScheduleModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduleService"/> class.
        /// </summary>
        /// <param name="repo">Implementation of IScheduleRepository <see cref="IScheduleRepository"/>.</param>
        public ScheduleService(IScheduleRepository repo)
            : base(repo)
        {
        }
    }
}
