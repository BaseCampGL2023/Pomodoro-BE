// <copyright file="RoutineAttemptRepository.cs" company="PomodoroGroup_GL_BaseCamp">
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
    public class RoutineAttemptRepository : BaseRepository<RoutineAttempt>, IRoutineAttemptRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoutineAttemptRepository"/> class.
        /// </summary>
        /// <param name="context">Instance of AppDbContext class <see cref="AppDbContext"/>.</param>
        public RoutineAttemptRepository(AppDbContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoutineAttemptRepository"/> class  without using DI container.
        /// </summary>
        /// <param name="options">Instance of DbContextOptions to instantiate AppDbContext.</param>
        internal RoutineAttemptRepository(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
    }
}
