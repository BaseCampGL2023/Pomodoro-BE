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

        /// <summary>
        /// Configure entities mapping and relations.
        /// </summary>
        /// <param name="builder">Provides API for configuring <see cref="ModelBuilder"/>.</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>(entity =>
            {
                entity.Property(e => e.Name).IsRequired()
                    .HasMaxLength(50);

                entity.HasIndex(e => e.AppIdentityUserId, "IX_Users_AspNetUserId")
                .IsUnique();

                entity.HasOne(d => d.AppIdentityUser)
                .WithOne(p => p.AppUser)
                .HasForeignKey<AppUser>(e => e.AppIdentityUserId)
                .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.TimerSettings)
                    .WithOne(d => d.AppUser)
                    .HasForeignKey(d => d.AppUserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_TimerSettings_AppUser_AppUserId");

                entity.HasMany(e => e.Routins)
                    .WithOne(d => d.AppUser)
                    .HasForeignKey(d => d.AppUserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Routins_AppUser_AppIserId");

                entity.HasMany(e => e.Tasks)
                    .WithOne(d => d.AppUser)
                    .HasForeignKey(d => d.AppUserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_AppTasks_AppUser_AppUserId");
            });

            builder.Entity<AppIdentityUser>(entity =>
            {
                entity.Navigation(e => e.AppUser).AutoInclude();
            });

            builder.Entity<TimerSettings>(entity =>
            {
                entity.Property(e => e.Pomodoro)
                    .IsRequired().HasConversion<long>();

                entity.Property(e => e.ShortBrake)
                    .IsRequired().HasConversion<long>();

                entity.Property(e => e.LongBreak)
                    .IsRequired().HasConversion<long>();
            });

            builder.Entity<AppTask>(entity =>
            {
                entity.Property(e => e.Title)
                    .IsRequired().HasMaxLength(100);

                entity.Property(e => e.Description)
                    .HasMaxLength(1000);

                entity.Property(e => e.AllocatedDuration)
                    .HasConversion<long>();

                entity.HasMany(e => e.Attempts)
                    .WithOne(d => d.Task)
                    .HasForeignKey(d => d.TaskId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_AppTaskAttempts_AppTask_AppTaskId");
            });

            builder.Entity<AppTaskAttempt>(entity =>
            {
                entity.Property(e => e.Comment)
                    .HasMaxLength(1000);

                entity.Property(e => e.Duration)
                    .IsRequired().HasConversion<long>();
            });

            builder.Entity<Routine>(entity =>
            {
                entity.Property(e => e.Title)
                    .IsRequired().HasMaxLength(100);

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.AllocatedDuration)
                    .HasConversion<long>();

                entity.HasMany(e => e.Attempts)
                    .WithOne(d => d.Routine)
                    .HasForeignKey(d => d.RoutineId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_RoutineAttemts_Routine_RoutineId");
            });

            builder.Entity<RoutineAttempt>(entity =>
            {
                entity.Property(e => e.Comment).HasMaxLength(1000);
            });
        }

        // TODO: map timespan (duration) to long
        // TODO: AppUser.Name = restrict length, required
    }
}
