// <copyright file="AppDbContext.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pomodoro.Dal.Entities;

namespace Pomodoro.Dal.Data
{
    /// <summary>
    /// Database context for application.
    /// </summary>
    public class AppDbContext : IdentityDbContext<AppIdentityUser, IdentityRole<Guid>, Guid>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppDbContext"/> class.
        /// </summary>
        /// <param name="options">The options to be used by a DbContext.</param>
        public AppDbContext(DbContextOptions options)
            : base(options)
        {
        }

        /// <summary>
        /// Gets a DbSet that can be used to query and save AppUser instances.
        /// </summary>
        public DbSet<AppUser> AppUsers => this.Set<AppUser>();

        /// <summary>
        /// Gets a DbSet that can be used to query and save TimerSetting instances.
        /// </summary>
        public DbSet<TimerSettings> TimerSettings => this.Set<TimerSettings>();

        /// <summary>
        /// Gets a DbSet that can be used to query and save AppTask instances.
        /// </summary>
        public DbSet<AppTask> AppTasks => this.Set<AppTask>();

        /// <summary>
        /// Gets a DbSet that can be used to query and save AppTaskAttempt instances.
        /// </summary>
        public DbSet<AppTaskAttempt> AppTaskAttempts => this.Set<AppTaskAttempt>();

        /// <summary>
        /// Gets a DbSet that can be used to query and save Routine instances.
        /// </summary>
        public DbSet<Routine> Routines => this.Set<Routine>();

        /// <summary>
        /// Gets a DbSet that can be used to query and save RoutineAttempt instances.
        /// </summary>
        public DbSet<RoutineAttempt> RoutineAttempts => this.Set<RoutineAttempt>();
    }
}
