﻿// <copyright file="ScheduleService.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;
using Pomodoro.Dal.Entities;
using Pomodoro.Dal.Exceptions;
using Pomodoro.Dal.Repositories.Interfaces;
using Pomodoro.Services.Base;
using Pomodoro.Services.Models;
using Pomodoro.Services.Models.Results;
using Pomodoro.Services.Utilities;

// TODO: GetActive, GetCompleted, GetEmpty
namespace Pomodoro.Services
{
    /// <summary>
    /// Perform operations with schedules.
    /// </summary>
    public class ScheduleService : BaseService<Schedule, ScheduleModel, IScheduleRepository>
    {
        private readonly IAppTaskRepository taskRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduleService"/> class.
        /// </summary>
        /// <param name="repo">Implementation of IScheduleRepository <see cref="IScheduleRepository"/>.</param>
        /// <param name="logger">ILoggerScheduleService instance.</param>
        /// <param name="taskRepository">Implementation of IAppTaskRepository <see cref="IAppTaskRepository"/>.</param>
        public ScheduleService(IScheduleRepository repo, ILogger<ScheduleService> logger, IAppTaskRepository taskRepository)
            : base(repo, logger)
        {
            this.taskRepository = taskRepository;
        }

        /// <inheritdoc/>
        public override async Task<ServiceResponse<bool>> AddOneOwnAsync(ScheduleModel model, Guid ownerId)
        {
            if (model.Tasks.Any())
            {
                return new ServiceResponse<bool>
                {
                    Result = ResponseType.Error,
                    Message = "Tasks should be generated by application.",
                };
            }

            model.OwnerId = ownerId;
            List<AppTask> tasks = ScheduleUtility.CreateTasks(model);

            if (tasks.Count == 0)
            {
                return new ServiceResponse<bool>
                {
                    Result = ResponseType.Error,
                    Message = $"No tasks could be planned with this schedule.",
                };
            }

            List<AppTask> existingScheduled = (await this.taskRepository.GetScheduledBetweenAsync(
                ownerId,
                model.StartDt,
                tasks.Max(t => t.StartDt).AddSeconds(model.AllocatedDuration))).ToList();

            Guid intersected = ScheduleUtility.GetIntersectedGuid(tasks, existingScheduled);

            if (intersected != Guid.Empty)
            {
                return new ServiceResponse<bool>
                {
                    Result = ResponseType.Conflict,
                    Message = $"Tasks intersects with already planned tasks, ID: {intersected}.",
                };
            }

            try
            {
                var entity = model.ToDalEntity(ownerId);
                entity.Tasks = tasks;
                int result = await this.Repo.AddAsync(entity, true);
                if (result > 0)
                {
                    model.Assign(entity);
                    return new ServiceResponse<bool> { Result = ResponseType.Ok, Data = true };
                }
                else
                {
                    this.Logger.LogCritical("Unpredictable path of execution, entity shouldn't created, exception not thrown.", model);
                    return new ServiceResponse<bool> { Result = ResponseType.Error, Message = "Something went wrong" };
                }
            }
            catch (PomoDbUpdateException)
            {
                return new ServiceResponse<bool> { Result = ResponseType.Error, Message = "Wrong data, check related" };
            }
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
            var previous = await this.Repo.GetByIdWithRelatedNoTrackingAsync(model.Id);
            if (previous == null)
            {
                return new ServiceResponse<bool> { Result = ResponseType.Conflict, Message = "Unexistable schedule." };
            }

            if (previous.AppUserId != model.OwnerId)
            {
                return new ServiceResponse<bool> { Result = ResponseType.Forbid };
            }

            if (model.ScheduleType == previous.ScheduleType
                && model.Template == previous.Template
                && model.StartDt == previous.StartDt
                && model.FinishAt == previous.FinishAtDt
                && model.CategoryId == previous.CategoryId)
            {
                return await base.UpdateOneOwnAsync(model, ownerId);
            }

            if (previous.Tasks.Any())
            {
                if (model.ScheduleType == previous.ScheduleType
                    && model.Template == previous.Template
                    && model.StartDt == previous.StartDt
                    && model.FinishAt == previous.FinishAtDt
                    && model.CategoryId != previous.CategoryId)
                {
                    foreach (var task in previous.Tasks)
                    {
                        task.CategoryId = model.CategoryId;
                    }

                    await this.taskRepository.UpdateRangeAsync(previous.Tasks);

                    return await base.UpdateOneOwnAsync(model, ownerId);
                }
                else if (model.ScheduleType == previous.ScheduleType
                    && model.Template == previous.Template
                    && model.StartDt == previous.StartDt)
                {
                    if (previous.FinishAtDt < model.FinishAt)
                    {
                        var deleted = previous.Tasks.Where(t => t.StartDt > model.FinishAt).ToList();
                        await this.taskRepository.DeleteRangeAsync(deleted);
                        return await base.UpdateOneOwnAsync(model, ownerId);
                    }
                    else
                    {
                        var startAddDt = previous.Tasks.Max(t => t.StartDt);

                        List<AppTask> tasks = ScheduleUtility.AddTasks(
                            model,
                            startAddDt,
                            previous.Tasks.Max(t => t.SequenceNumber));

                        List<AppTask> existingScheduled = (await this.taskRepository.GetScheduledBetweenAsync(
                            ownerId,
                            startAddDt,
                            tasks.Max(t => t.StartDt).AddSeconds(model.AllocatedDuration))).ToList();

                        Guid intersected = ScheduleUtility.GetIntersectedGuid(tasks, existingScheduled);

                        if (intersected != Guid.Empty)
                        {
                            return new ServiceResponse<bool>
                            {
                                Result = ResponseType.Conflict,
                                Message = $"Tasks intersects with already planned tasks, ID: {intersected}.",
                            };
                        }

                        await this.taskRepository.AddRangeAsync(tasks);

                        return await base.UpdateOneOwnAsync(model, ownerId);
                    }
                }
                else
                {
                    return new ServiceResponse<bool>
                    {
                        Result = ResponseType.Conflict,
                        Message = "Schedule has planned or performed tasks - delete them or create new schedule instead",
                    };
                }
            }
            else
            {
                return await base.UpdateOneOwnAsync(model, ownerId);
            }
        }

        /// <inheritdoc/>
        public override async Task<ServiceResponse<bool>> DeleteOneOwnAsync(Guid id, Guid ownerId)
        {
            try
            {
                var schedule = await this.Repo.GetByIdWithRelatedAsync(id);
                if (schedule == null)
                {
                    return new ServiceResponse<bool> { Result = ResponseType.Error, Message = "Wrong data" };
                }

                var deleted = schedule.Tasks.Where(t => t.StartDt > DateTime.UtcNow).ToList();
                await this.taskRepository.DeleteRangeAsync(deleted);
                int result = await this.Repo.DeleteAsync(schedule, true);
                if (result > 0)
                {
                    return new ServiceResponse<bool> { Result = ResponseType.NoContent, Data = true };
                }
                else
                {
                    this.Logger.LogCritical("Unpredictable path of execution, entity shouldn't delete, exception not thrown.");
                    return new ServiceResponse<bool> { Result = ResponseType.Error, Message = "Something went wrong" };
                }
            }
            catch (PomoConcurrencyException)
            {
                return new ServiceResponse<bool> { Result = ResponseType.Error, Message = "Wrong data" };
            }
        }
    }
}
