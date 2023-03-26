// <copyright file="ScheduleService.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;
using Pomodoro.Dal.Entities;
using Pomodoro.Dal.Repositories.Interfaces;
using Pomodoro.Services.Base;
using Pomodoro.Services.Models;
using Pomodoro.Services.Models.Results;
using Pomodoro.Services.Utilities;

namespace Pomodoro.Services
{
    /// <summary>
    /// Perform operations with schedules.
    /// </summary>
    public class ScheduleService : BaseService<Schedule, ScheduleModel, IScheduleRepository>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduleService"/> class.
        /// </summary>
        /// <param name="repo">Implementation of IScheduleRepository <see cref="IScheduleRepository"/>.</param>
        /// <param name="logger">ILoggerScheduleService instance.</param>
        public ScheduleService(IScheduleRepository repo, ILogger<ScheduleService> logger)
            : base(repo, logger)
        {
        }

        /// <inheritdoc/>
        public override async Task<ServiceResponse<bool>> AddOneOwnAsync(ScheduleModel model, Guid ownerId)
        {
            // We can't save non-active schedule.
            model.IsActive = true;
            if (model.Tasks.Any())
            {
                // TODO: check sequence number
                if (model.Tasks.All(t => t.StartDt.HasValue)
                    && model.Tasks.All(
                        t => ScheduleUtility.IsCanCreateTask(
                            model,
                            t.StartDt!.Value,
                            t.StartDt!.Value.AddMinutes(1))))
                {
                    return await base.AddOneOwnAsync(model, ownerId);
                }
                else
                {
                    return new ServiceResponse<bool>
                    {
                        Result = ResponseType.Error,
                        Message = "Tasks don't correspond to schedule.",
                    };
                }
            }

            TaskModel firstTask = new ()
            {
                Title = model.Title,
                Description = model.Description,
                SequenceNumber = 1,
                AllocatedDuration = model.AllocatedDuration,
                CategoryId = model.CategoryId,
                OwnerId = ownerId,
                StartDt = model.StartDt,
            };

            model.Tasks.Add(firstTask);

            return await base.AddOneOwnAsync(model, ownerId);
        }

        /// <summary>
        /// Update schedule only if ScheduleType, template, start and finish date don't change,
        /// otherwise create new Schedule, or delete all related tasks.
        /// </summary>
        /// <param name="model">Exisitng schedule.</param>
        /// <param name="ownerId">Owner id.</param>
        /// <returns>Updated schedule.</returns>
        public override async Task<ServiceResponse<bool>> UpdateOneOwnAsync(ScheduleModel model, Guid ownerId)
        {
            var previous = await this.Repo.GetByIdWithRelatedAsync(model.Id);
            if (previous == null)
            {
                return new ServiceResponse<bool> { Result = ResponseType.Error, Message = "Unexistable schedule." };
            }

            if (previous.AppUserId != model.OwnerId)
            {
                return new ServiceResponse<bool> { Result = ResponseType.Forbid };
            }

            if (model.ScheduleType == previous.ScheduleType
                && model.Template == previous.Template
                && model.StartDt == previous.StartDt
                && model.FinishAt == previous.FinishDt
                && model.CategoryId == previous.CategoryId)
            {
                return await base.UpdateOneOwnAsync(model, ownerId);
            }

            if (previous.Tasks.Any())
            {
                if (model.ScheduleType == previous.ScheduleType
                    && model.Template == previous.Template
                    && model.StartDt == previous.StartDt
                    && model.FinishAt == previous.FinishDt)
                {
                    foreach (var task in previous.Tasks)
                    {
                        task.CategoryId = model.CategoryId;
                    }

                    return await base.UpdateOneOwnAsync(model, ownerId);
                }
                else
                {
                    return new ServiceResponse<bool> { Result = ResponseType.Error, Message = "Schedule has planned tasks, create new schedule instead" };
                }
            }

            return await base.UpdateOneOwnAsync(model, ownerId);
        }
    }
}
