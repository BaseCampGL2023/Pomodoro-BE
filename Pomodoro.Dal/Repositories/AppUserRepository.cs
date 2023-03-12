// <copyright file="AppUserRepository.cs" company="PomodoroGroup_GL_BaseCamp">
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
    public class AppUserRepository : BaseRepository<AppUser>, IAppUserRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppUserRepository"/> class.
        /// </summary>
        /// <param name="context">Instance of AppDbContext class <see cref="AppDbContext"/>.</param>
        public AppUserRepository(AppDbContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppUserRepository"/> class  without using DI container.
        /// </summary>
        /// <param name="options">Instance of DbContextOptions to instantiate AppDbContext.</param>
        internal AppUserRepository(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        /// <inheritdoc/>
        public async Task<AppUser?> GetByIdWithRelatedAsync(Guid id)
        {
            return await this.Table.Include(e => e.Routins)
                .Include(e => e.Tasks).Include(e => e.TimerSettings)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        /// <inheritdoc/>
        public async Task<AppUser?> GetByIdWithRoutinesAsync(Guid id)
        {
            return await this.Table.Include(e => e.Routins)
                .ThenInclude(r => r.Attempts).FirstOrDefaultAsync(e => e.Id == id);
        }

        /// <inheritdoc/>
        public async Task<AppUser?> GetByIdWithSettingsAsync(Guid id)
        {
            return await this.Table.Include(e => e.TimerSettings)
                .IgnoreQueryFilters().FirstOrDefaultAsync(e => e.Id == id);
        }

        /// <inheritdoc/>
        public async Task<AppUser?> GetByIdWithTasksAsync(Guid id)
        {
           return await this.Table.Include(e => e.Tasks)
                .ThenInclude(r => r.Attempts).FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}
