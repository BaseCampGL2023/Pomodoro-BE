// <copyright file="TaskService.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pomodoro.Dal.Configs;
using Pomodoro.Dal.Entities;
using Pomodoro.Dal.Exceptions;
using Pomodoro.Dal.Repositories.Interfaces;
using Pomodoro.Services.Base;
using Pomodoro.Services.Interfaces;
using Pomodoro.Services.Models;
using Pomodoro.Services.Models.Enums;
using Pomodoro.Services.Models.Query;
using Pomodoro.Services.Models.Results;

namespace Pomodoro.Services
{
    /// <summary>
    /// Perform operations with tasks.
    /// </summary>
    public class TaskService : BaseService<AppTask, TaskModel, IAppTaskRepository>, ITaskService
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

                if (previous.StartDt != model.StartDt
                    || previous.AllocatedDuration.TotalSeconds != model.AllocatedDuration)
                {
                    return new ServiceResponse<bool>
                    {
                        Result = ResponseType.Error,
                        Message = "Instead changing start datetime, delete task from schedule.",
                    };
                }
            }

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

        /// <summary>
        /// Retrive tasks with query filter.
        /// </summary>
        /// <param name="ownerId">Owner id.</param>
        /// <param name="query">Query filter <see cref="TaskQueryModel"/>.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<ICollection<TaskModel>> GetOwnByQueryAsync(Guid ownerId, TaskQueryModel query)
        {
            var result = this.Repo.All.Where(t => t.AppUserId == ownerId);
            switch (query.ExecutionState)
            {
                case TaskExecutionState.All:
                    break;
                case TaskExecutionState.Started:
                    result = result.Where(t => t.Pomodoros.Any() && t.FinishDt == null);
                    break;
                case TaskExecutionState.Pristine:
                    result = result.Where(t => !t.Pomodoros.Any() && t.FinishDt == null);
                    break;
                case TaskExecutionState.Pending:
                    result = result.Where(t => t.FinishDt == null);
                    break;
                case TaskExecutionState.Finished:
                    result = result.Where(t => t.FinishDt != null);
                    break;
            }

            switch (query.Repeatable)
            {
                case TaskRepeatable.Any:
                    break;
                case TaskRepeatable.Routine:
                    result = result.Where(t => t.ScheduleId != null);
                    break;
                case TaskRepeatable.Alone:
                    result = result.Where(t => t.ScheduleId == null);
                    break;
            }

            if (query.StartDate.HasValue && !query.EndDate.HasValue)
            {
                result = result.Where(t => t.StartDt > query.StartDate.Value);
            }
            else if (!query.StartDate.HasValue && query.EndDate.HasValue)
            {
                result = result.Where(t => t.StartDt < query.EndDate.Value);
            }
            else if (query.StartDate.HasValue && query.EndDate.HasValue)
            {
                result = result.Where(t => t.StartDt > query.StartDate.Value
                && t.StartDt < query.EndDate.Value);
            }
            else if (!query.EndDate.HasValue && !query.StartDate.HasValue)
            {
                var now = DateTime.UtcNow;
                query.StartDate = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);

                query.EndDate = query.StartDate.Value.AddDays(1);

                result = result.Where(t => t.StartDt > query.StartDate!.Value
                && t.StartDt < query.EndDate!.Value);
            }

            return this.MapEntitiesToModels(await result.ToListAsync());
        }

        /// <summary>
        /// Return pagination result.
        /// </summary>
        /// <param name="ownerId">Owner id.</param>
        /// <param name="query">Pagination query.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<PaginResult<TaskModel>> GetPaginatedOwnAsync(Guid ownerId, PaginQueryModel query)
        {
            int count = await this.Repo.CountAsync(t => t.AppUserId == ownerId);
            var source = this.Repo.All.Include(t => t.Category)
                .Include(t => t.Pomodoros).Where(t => t.AppUserId == ownerId);
            if (!string.IsNullOrWhiteSpace(query.FilterQuery)
                && !string.IsNullOrWhiteSpace(query.FilterColumn))
            {
                source = query.FilterColumn.ToLower() switch
                {
                    "title" => source.Where(t => t.Title.StartsWith(query.FilterQuery)),
                    "description" => source.Where(t => t.Description != null
                        && t.Description.StartsWith(query.FilterQuery)),
                    "category" => source.Where(t => t.Category != null
                            && t.Category.Name.StartsWith(query.FilterQuery)),
                    _ => source,
                };
            }

            if (!string.IsNullOrWhiteSpace(query.SortColumn))
            {
                query.SortOrder = !string.IsNullOrWhiteSpace(query.SortOrder)
                    && query.SortOrder.ToUpper() == "ASC" ? "ASC" : "DESC";

                source = query.SortOrder.ToLower() switch
                {
                    "title" => query.SortOrder == "ASC"
                        ? source.OrderBy(t => t)
                        : source.OrderByDescending(t => t),
                    "description" => query.SortOrder == "ASC"
                        ? source.OrderBy(t => t)
                        : source.OrderByDescending(t => t),
                    "sequenceNumber" => query.SortOrder == "ASC"
                        ? source.OrderBy(t => t)
                        : source.OrderByDescending(t => t),
                    "startDt" => query.SortOrder == "ASC"
                        ? source.OrderBy(t => t.StartDt)
                        : source.OrderByDescending(t => t.StartDt),
                    "allocatedDuration" => query.SortOrder == "ASC"
                        ? source.OrderBy(t => t.AllocatedDuration)
                        : source.OrderByDescending(t => t.AllocatedDuration),
                    "finishft" => query.SortOrder == "ASC"
                        ? source.OrderBy(t => t.FinishDt)
                        : source.OrderByDescending(t => t.FinishDt),
                    _ => source,
                };
            }

            source = source.Skip(query.PageIndex * query.PageSize).Take(query.PageSize);
            var list = await source.ToListAsync();
            var data = new List<TaskModel>();

            foreach (var item in list)
            {
                var model = new TaskModel();
                model.Assign(item);
                data.Add(model);
            }

            return new PaginResult<TaskModel>
            {
                Data = data,
                TotalCount = count,
                PageIndex = query.PageIndex,
                PageSize = query.PageSize,
                FilterColumn = query.FilterColumn,
                FilterQuery = query.FilterQuery,
                SortColumn = query.SortColumn,
                SortOrder = query.SortOrder,
            };
        }
    }
}
