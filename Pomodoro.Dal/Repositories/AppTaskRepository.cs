// <copyright file="AppTaskRepository.cs" company="PomodoroGroup_GL_BaseCamp">
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
    public class AppTaskRepository : BelongRepository<AppTask>, IAppTaskRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppTaskRepository"/> class.
        /// </summary>
        /// <param name="context">Instance of AppDbContext class <see cref="AppDbContext"/>.</param>
        public AppTaskRepository(AppDbContext context)
            : base(context)
        {
        }

        /// <inheritdoc/>
        public async Task<ICollection<AppTask>> GetBelonginFinishedAllAsync(Guid ownerId)
        {
            return await this.Table.Where(e => e.FinishDt != null).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<ICollection<AppTask>> GetBelonginPrisitineAllAsync(Guid ownerId)
        {
            return await this.Table.Include(e => e.Pomodoros).Where(e => !e.Pomodoros.Any()).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<ICollection<AppTask>> GetBelonginStartedAllAsync(Guid ownerId)
        {
            return await this.Table.Include(e => e.Pomodoros).Where(e => e.Pomodoros.Any()).ToListAsync();
        }

        /// <summary>
        /// Get AppTask object by id with TaskAttempts collection.
        /// </summary>
        /// <param name="id">AppTask id.</param>
        /// <returns>Queried object or null, if object with this id belonging to user doesn't exist in database.</returns>
        public async Task<AppTask?> GetByIdWithRelatedAsync(Guid id)
        {
            return await this.Table.Include(e => e.Pomodoros)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        /// <inheritdoc/>
        public async Task<ICollection<AppTask>> GetScheduledAllAsync(Guid ownerId, DateTime start, DateTime end)
        {
            return await this.Table.Where(
                e => e.AppUserId == ownerId
                && e.ScheduleId != null
                && e.StartDt >= start
                && e.StartDt < end).ToListAsync();
        }
    }
}
