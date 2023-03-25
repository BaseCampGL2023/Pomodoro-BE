// <copyright file="AppDbContext.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Pomodoro.Dal.Entities;
using Pomodoro.Dal.Enums;
using Pomodoro.Dal.Exceptions;
using System.Threading;

namespace Pomodoro.Dal.Data
{
    /// <summary>
    /// Database context for application.
    /// </summary>
    public class AppDbContext : IdentityDbContext<AppIdentityUser, IdentityRole<Guid>, Guid>
    {
        private readonly ILogger<AppDbContext> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppDbContext"/> class.
        /// </summary>
        /// <param name="options">The options to be used by a DbContext.</param>
        /// <param name="logger">Logger for AppDbContext category.</param>
        public AppDbContext(DbContextOptions options, ILogger<AppDbContext> logger)
            : base(options)
        {
            this.logger = logger;
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
        /// Gets a DbSet that can be used to query and save PomoUnit instances.
        /// </summary>
        public DbSet<PomoUnit> PomoUnits => this.Set<PomoUnit>();

        /// <summary>
        /// Gets a DbSet that can be used to query and save Schedule instances.
        /// </summary>
        public DbSet<Schedule> Schedules => this.Set<Schedule>();

        /// <summary>
        /// Gets a DbSet that can be used to query and save Category instances.
        /// </summary>
        public DbSet<Category> Categories => this.Set<Category>();

        /// <summary>
        /// Saves all changes made in this context to the database. With exception logging.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The number of state entries written to the database.</returns>
        /// <exception cref="PomoConcurrencyException">Wrapped and logged DbUpdateConcurrencyException.</exception>
        /// <exception cref="PomoRetryLimitExceededException">Wrapped and logged RetryLimitExceededException.</exception>
        /// <exception cref="PomoDbUpdateException">Wrapped and logged DbUpdateException.</exception>
        /// <exception cref="PomoException">Wrapped and logged system exception.</exception>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return base.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                this.logger.LogError(ex, "A concurrency error happened.");
                throw new PomoConcurrencyException("A concurrency error happened", ex);
            }
            catch (RetryLimitExceededException ex)
            {
                this.logger.LogError(ex, "There is a problem with SQl Server.");
                throw new PomoRetryLimitExceededException("There is a problem with SQl Server.", ex);
            }
            catch (DbUpdateException ex)
            {
                this.logger.LogError(ex, "An error occurred updating the database");
                throw new PomoDbUpdateException("An error occurred updating the database", ex);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "An error occurred updating the database");
                throw new PomoException("An error occurred updating the database", ex);
            }
        }

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

                entity.Ignore(e => e.ActiveTimerSettings);

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

                entity.HasMany(e => e.Schedules)
                    .WithOne(d => d.AppUser)
                    .HasForeignKey(d => d.AppUserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Schedules_AppUser_AppUserId");

                entity.HasMany(e => e.Tasks)
                    .WithOne(d => d.AppUser)
                    .HasForeignKey(d => d.AppUserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_AppTasks_AppUser_AppUserId");

                entity.HasMany(e => e.Categories)
                    .WithOne(d => d.AppUser)
                    .HasForeignKey(d => d.AppUserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Categories_AppUser_AppUserId");
            });

            builder.Entity<AppIdentityUser>(entity =>
            {
                entity.Navigation(e => e.AppUser).AutoInclude();
            });

            builder.Entity<TimerSettings>(entity =>
            {
                entity.ToTable(MigrationHelpers.TimerSettingsTableName, "dbo");

                entity.HasQueryFilter(e => e.IsActive);

                entity.Property(e => e.IsActive)
                    .HasColumnName(MigrationHelpers.TimerSettingsIsActiveAttribute);

                entity.Property(e => e.AppUserId)
                    .HasColumnName(MigrationHelpers.TimerSettingsUserIdAttribute);

                entity.Property(e => e.Pomodoro)
                    .IsRequired().HasConversion<long>();

                entity.Property(e => e.ShortBrake)
                    .IsRequired().HasConversion<long>();

                entity.Property(e => e.LongBreak)
                    .IsRequired().HasConversion<long>();

                entity.Property(e => e.CreatedAt).IsRequired()
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            builder.Entity<AppTask>(entity =>
            {
                entity.Property(e => e.Title)
                    .IsRequired().HasMaxLength(100);

                entity.Property(e => e.Description)
                    .HasMaxLength(1000);

                entity.Property(e => e.SequenceNumber).IsRequired()
                    .HasDefaultValue(1);

                entity.Property(e => e.AllocatedDuration)
                    .HasConversion<long>();

                entity.Property(e => e.CreatedDt).IsRequired()
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.HasMany(e => e.Pomodoros)
                    .WithOne(d => d.Task)
                    .HasForeignKey(d => d.TaskId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Pomodoros_AppTask_AppTaskId");

                entity.HasOne(e => e.Schedule)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(e => e.ScheduleId)
                    .OnDelete(DeleteBehavior.NoAction)
                    .HasConstraintName("FK_AppTask_Schedule_SheduleId");
            });

            builder.Entity<PomoUnit>(entity =>
            {
                entity.ToTable("Pomodoros");

                entity.Property(e => e.Comment)
                    .HasMaxLength(1000);

                entity.Property(e => e.Duration)
                    .IsRequired().HasConversion<long>();

                entity.HasOne(e => e.TimerSettings)
                    .WithMany()
                    .HasForeignKey(e => e.TimerSettingsId)
                    .OnDelete(DeleteBehavior.NoAction)
                    .HasConstraintName("FK_Pomodoros_TimerSettings_TimerSettingsId");
            });

            builder.Entity<Schedule>(entity =>
            {
                int scheduleTypeLimit = Enum.GetValues(typeof(ScheduleType))
                    .Cast<int>().Max() + 1;

                entity.Property(e => e.Title)
                    .IsRequired().HasMaxLength(100);

                entity.Property(e => e.Template).IsRequired()
                    .HasMaxLength(370);

                entity.Property(e => e.ScheduleType).IsRequired()
                .HasColumnName("ScheduleType").HasConversion<byte>();

                entity.Property(e => e.CreatedDt).IsRequired()
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.AllocatedDuration)
                    .HasConversion<long>();

                entity.HasCheckConstraint(
                    "ScheduleType",
                    $"ScheduleType >= 1 AND ScheduleType < {scheduleTypeLimit}");

                entity.HasOne(e => e.Previous)
                    .WithOne();
            });

            builder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Name).IsRequired().HasMaxLength(60);

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.HasMany(p => p.Tasks)
                    .WithOne(d => d.Category)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.NoAction)
                    .HasConstraintName("FK_Categories_AppTasks_CategoryId");

                entity.HasMany(p => p.Schedules)
                    .WithOne(d => d.Category)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.NoAction)
                    .HasConstraintName("FK_Schedules_Categories_CategoryId");
            });
        }
    }
}
