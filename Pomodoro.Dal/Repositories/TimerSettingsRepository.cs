// <copyright file="TimerSettingsRepository.cs" company="PomodoroGroup_GL_BaseCamp">
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
    public class TimerSettingsRepository : BaseRepository<TimerSettings>, ITimerSettingsRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimerSettingsRepository"/> class.
        /// </summary>
        /// <param name="context">Instance of AppDbContext class <see cref="AppDbContext"/>.</param>
        public TimerSettingsRepository(AppDbContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerSettingsRepository"/> class without using DI container.
        /// </summary>
        /// <param name="options">>Instance of DbContextOptions to instantiate AppDbContext.</param>
        internal TimerSettingsRepository(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        /// <inheritdoc/>
        public async Task<int> DeleteOneBelongingAsync(Guid id, Guid ownerId, bool persist = false)
        {
            TimerSettings settings = new ()
            {
                Id = id,
                AppUserId = ownerId,
            };
            this.Context.Entry<TimerSettings>(settings).State = EntityState.Deleted;
            return persist ? await this.SaveChangesAsync() : 0;
        }

        /// <inheritdoc/>
        public async Task<ICollection<TimerSettings>> GetBelongingAll(Guid ownerId)
        {
            return await this.Table.IgnoreQueryFilters()
                .Where(e => e.AppUserId == ownerId).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<ICollection<TimerSettings>> GetBelongingAllAsNoTracking(Guid ownerId)
        {
            return await this.Table.IgnoreQueryFilters().Where(e => e.AppUserId == ownerId)
                .AsNoTracking().ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<TimerSettings?> GetCurrentTimerSettingsAsync(Guid ownerId)
        {
            return await this.Table.FirstOrDefaultAsync(e => e.AppUserId == ownerId);
        }
    }
}
