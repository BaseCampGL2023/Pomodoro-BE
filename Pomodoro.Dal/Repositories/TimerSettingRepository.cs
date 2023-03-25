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
    public class TimerSettingRepository : BaseRepository<TimerSettings>, ITimerSettingRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimerSettingRepository"/> class.
        /// </summary>
        /// <param name="context">Instance of AppDbContext class <see cref="AppDbContext"/>.</param>
        public TimerSettingRepository(AppDbContext context)
            : base(context)
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
