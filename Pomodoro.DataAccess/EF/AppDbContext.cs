// <copyright file="AppDbContext.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using Pomodoro.DataAccess.Entities;
using Pomodoro.DataAccess.Extensions;

namespace Pomodoro.DataAccess.EF
{
    internal class AppDbContext : DbContext
    {
        public DbSet<Completed> CompletedTasks => Set<Completed>();
        public DbSet<Frequency> Frequencies => Set<Frequency>();
        public DbSet<FrequencyType> FrequencyTypes => Set<FrequencyType>();
        public DbSet<Settings> Settings => Set<Settings>();
        public DbSet<Entities.Task> Tasks => Set<Entities.Task>();
        public DbSet<User> Users => Set<User>();

        public AppDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            modelBuilder.Seed();
        }
    }
}
