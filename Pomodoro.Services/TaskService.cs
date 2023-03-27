﻿// <copyright file="TaskService.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;
using Pomodoro.Dal.Configs;
using Pomodoro.Dal.Entities;
using Pomodoro.Dal.Exceptions;
using Pomodoro.Dal.Repositories.Interfaces;
using Pomodoro.Services.Base;
using Pomodoro.Services.Models;
using Pomodoro.Services.Models.Results;

// TODO: GetToday
// TODO: GetCompleted
// TODO: Pagination
namespace Pomodoro.Services
{
    /// <summary>
    /// Perform operations with tasks.
    /// </summary>
    public class TaskService : BaseService<AppTask, TaskModel, IAppTaskRepository>
    {
        private readonly IScheduleRepository scheduleRepository;
        private readonly IPomoUnitRepository pomoRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskService"/> class.
        /// </summary>
        /// <param name="repo">Implementation of IAppTaskRepository <see cref="IAppTaskRepository"/>.</param>
        /// <param name="logger">Logger instance.</param>
        /// <param name="scheduleRepository">Implementation of IScheduleRepository <see cref="IScheduleRepository"/>.</param>
        /// <param name="pomoRepository">Implementation of IPomoUnitRepository <see cref="IPomoUnitRepository"/>.</param>
        public TaskService(
            IAppTaskRepository repo,
            ILogger<TaskService> logger,
            IScheduleRepository scheduleRepository,
            IPomoUnitRepository pomoRepository)
            : base(repo, logger)
        {
            this.scheduleRepository = scheduleRepository;
            this.pomoRepository = pomoRepository;
        }

        /// <inheritdoc/>
        public override async Task<ServiceResponse<bool>> AddOneOwnAsync(TaskModel model, Guid ownerId)
        {
            if (model.ScheduleId != null || model.SequenceNumber > 1)
            {
                return new ServiceResponse<bool>
                {
                    Result = ResponseType.Error,
                    Message = "Can't create scheduled tasks directly.",
                };
            }

            if (model.FinishDt != null)
            {
                return new ServiceResponse<bool>
                {
                    Result = ResponseType.Error,
                    Message = "Can't create completed tasks.",
                };
            }

            return await base.AddOneOwnAsync(model, ownerId);
        }

        /// <inheritdoc/>
        public override async Task<ServiceResponse<bool>> UpdateOneOwnAsync(TaskModel model, Guid ownerId)
        {
            if (model.ScheduleId == null && model.SequenceNumber > 1)
            {
                return new ServiceResponse<bool>
                {
                    Result = ResponseType.Error,
                    Message = "Non-scheduled task can't have a sequence number",
                };
            }

            if (model.ScheduleId != null)
            {
                var previous = await this.Repo.GetByIdNoTrackingAsync(model.Id);

                if (previous == null)
                {
                    return new ServiceResponse<bool> { Result = ResponseType.Conflict, Message = "Unexistable task." };
                }

                if (previous.AppUserId != model.OwnerId)
                {
                    return new ServiceResponse<bool> { Result = ResponseType.Forbid };
                }

                if (previous.ScheduleId != model.ScheduleId)
                {
                    return new ServiceResponse<bool> { Result = ResponseType.Error, Message = "Can't change schedule." };
                }

                if (previous.SequenceNumber != model.SequenceNumber)
                {
                    var schedule = await this.scheduleRepository.GetByIdWithRelatedNoTrackingAsync(model.ScheduleId.Value);
                    if (schedule == null)
                    {
                        return new ServiceResponse<bool> { Result = ResponseType.Conflict, Message = "Unexistable schedule." };
                    }

                    if (schedule.Tasks != null
                        && schedule.Tasks.Any(t => t.SequenceNumber == model.SequenceNumber))
                    {
                        return new ServiceResponse<bool>
                        {
                            Result = ResponseType.Conflict,
                            Message = "Task with this number already exist in this schedule.",
                        };
                    }
                }
            }

            // TODO: check task intersection

            return await base.UpdateOneOwnAsync(model, ownerId);
        }

        /// <summary>
        /// Add pomodoro to task.
        /// </summary>
        /// <param name="model">Pomodoro model.</param>
        /// <param name="ownerId">OwnerId.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<ServiceResponse<bool>> AddPomodoro(PomoModel model, Guid ownerId)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(model.Comment))
                {
                    var task = await this.Repo.GetByIdAsync(model.TaskId);
                    if (task == null)
                    {
                        return new ServiceResponse<bool> { Result = ResponseType.Conflict, Message = "Task with this Id didn't exist" };
                    }

                    if (task.AppUserId != ownerId)
                    {
                        return new ServiceResponse<bool> { Result = ResponseType.Forbid };
                    }

                    if (task.Description?.Length + model.Comment.Length < PomoConstants.TaskDescriptionMaxLength)
                    {
                        var time = model.StartDt.AddSeconds(model.Duration);
                        task.Description = $"{task.Description} \n[{time}]: {model.Comment}";
                        await this.Repo.UpdateAsync(task);
                    }
                }

                var entity = model.ToDalEntity();
                int result = await this.pomoRepository.AddAsync(entity, true);
                if (result > 0)
                {
                    model.TimerSettingsId = entity.Id;
                    return new ServiceResponse<bool> { Result = ResponseType.Ok, };
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
    }
}
