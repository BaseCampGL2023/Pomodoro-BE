// <copyright file="ScheduleService.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Pomodoro.Dal.Entities;
using Pomodoro.Dal.Repositories.Interfaces;
using Pomodoro.Services.Base;
using Pomodoro.Services.Models;
using Pomodoro.Services.Utilities;

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

        /// <inheritdoc/>
        public override async Task<bool> AddOneOwnAsync(ScheduleModel model, Guid ownerId)
        {
            // We can't save non-active schedule.
            model.IsActive = true;
            if (model.Tasks.Any())
            {
               // TODO: how validate task?
               return await base.AddOneOwnAsync(model, ownerId);
            }

            // TODO: check finisdDt, check finishDt in modelValidation

            // TODO: check UTC time
            var task = ScheduleUtility.CreateFirstTask(model, ownerId);
            var schedule = model.ToDalEntity(ownerId);
            schedule.Tasks.Add(task);
            var result = await this.Repo.AddAsync(schedule, true);
            model.Assign(schedule);

            return result > 0;
        }

        // TODO: implement custom exceptions.
    }
}
