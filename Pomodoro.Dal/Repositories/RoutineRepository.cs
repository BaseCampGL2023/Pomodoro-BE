// <copyright file="RoutineRepository.cs" company="PomodoroGroup_GL_BaseCamp">
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
    public class RoutineRepository : BelongRepository<Routine>, IRoutineRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoutineRepository"/> class.
        /// </summary>
        /// <param name="context">Instance of AppDbContext class <see cref="AppDbContext"/>.</param>
        public RoutineRepository(AppDbContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoutineRepository"/> class  without using DI container.
        /// </summary>
        /// <param name="options">Instance of DbContextOptions to instantiate AppDbContext.</param>
        internal RoutineRepository(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        /// <inheritdoc/>
        public async Task<Routine?> GetByIdWithRelatedAsync(Guid id)
        {
            return await this.Table.Include(e => e.Attempts)
                .FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}
