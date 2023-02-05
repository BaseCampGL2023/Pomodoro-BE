// <copyright file="AppDbContext.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pomodoro.DataAccess.Entities;
using Pomodoro.DataAccess.Extensions;
using System.Runtime.CompilerServices;

namespace Pomodoro.DataAccess.EF
{
    public class AppDbContext : IdentityDbContext<IdentityUser<Guid>, IdentityRole<Guid>, Guid>
    {
        public DbSet<Completed> CompletedTasks => Set<Completed>();
        public DbSet<Frequency> Frequencies => Set<Frequency>();
        public DbSet<FrequencyType> FrequencyTypes => Set<FrequencyType>();
        public DbSet<Settings> Settings => Set<Settings>();
        public DbSet<TaskEntity> Tasks => Set<TaskEntity>();
        public DbSet<User> AppUsers => Set<User>();

        public AppDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            modelBuilder.Seed();
        }
    }
}
