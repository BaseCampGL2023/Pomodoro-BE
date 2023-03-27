// <copyright file="ScheduleRepository.cs" company="PomodoroGroup_GL_BaseCamp">
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
    public class ScheduleRepository : BelongRepository<Schedule>, IScheduleRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduleRepository"/> class.
        /// </summary>
        /// <param name="context">Instance of AppDbContext class <see cref="AppDbContext"/>.</param>
        public ScheduleRepository(AppDbContext context)
            : base(context)
        {
        }

        /// <inheritdoc/>
        public async Task<Schedule?> GetByIdWithRelatedAsync(Guid id)
        {
            return await this.Table.Include(s => s.Tasks)
                .Include(c => c.Category)
                .FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}
