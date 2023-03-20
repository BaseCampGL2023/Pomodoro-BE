// <copyright file="ScheduleService.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Pomodoro.Dal.Repositories.Interfaces;
using Pomodoro.Services.Models;
using Pomodoro.Services.Models.Results;

namespace Pomodoro.Services
{
    /// <summary>
    /// Perform operations with schedules.
    /// </summary>
    public class ScheduleService
    {
        private readonly IScheduleRepository scheduleRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduleService"/> class.
        /// </summary>
        /// <param name="repo">Implementation of IScheduleRepository <see cref="IScheduleRepository"/>.</param>
        public ScheduleService(IScheduleRepository repo)
        {
            this.scheduleRepo = repo;
        }

        /// <summary>
        /// Return belonging to user schedule by id.
        /// </summary>
        /// <param name="id">Task id.</param>
        /// <param name="ownerId">Owner id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<ServiceResponse<ScheduleModel>> GetOwnByIdAsync(Guid id, Guid ownerId)
        {
            var result = await this.scheduleRepo.GetByIdAsync(id);
            if (result is null)
            {
                return new ServiceResponse<ScheduleModel> { Result = ResponseType.NotFound };
            }
            else if (result.AppUserId != ownerId)
            {
                return new ServiceResponse<ScheduleModel> { Result = ResponseType.Forbid };
            }
            else
            {
                return new ServiceResponse<ScheduleModel> { Result = ResponseType.Ok, Data = ScheduleModel.Create(result) };
            }
        }
    }
}
