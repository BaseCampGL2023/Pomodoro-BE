// <copyright file="AppTask.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Pomodoro.Dal.Entities.Base;

namespace Pomodoro.Dal.Entities
{
    /// <summary>
    /// Describes user task.
    /// </summary>
    public class AppTask : BaseEntity, IBelongEntity
    {
        /// <summary>
        /// Gets or sets task title.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets task description, optional.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets task number in sequence for tasks with a schedule.
        /// Sets 1 for non-periodic tasks.
        /// </summary>
        public int SequenceNumber { get; set; }

        /// <summary>
        /// Gets or sets DateTime when task created.
        /// </summary>
        public DateTime CreatedDt { get; set; }

        /// <summary>
        /// Gets or sets DateTime when task modified.
        /// </summary>
        public DateTime? ModifiedDt { get; set; }

        /// <summary>
        /// Gets or sets planned start time.
        /// </summary>
        public DateTime? StartDt { get; set; }

        /// <summary>
        /// Gets or sets DateTime when task performing completed.
        /// </summary>
        public DateTime? FinishDt { get; set; } = null;

        /// <summary>
        /// Gets or sets planned duration of the task.
        /// </summary>
        public TimeSpan? AllocatedDuration { get; set; }

        /// <summary>
        /// Gets or sets foreign key to AppUser entity.
        /// </summary>
        public Guid AppUserId { get; set; }

        /// <summary>
        /// Gets or sets navigation property represents application user.
        /// </summary>
        public AppUser? AppUser { get; set; }

        /// <summary>
        /// Gets or sets foreign key to Schedule entity.
        /// </summary>
        public Guid? ScheduleId { get; set; }

        /// <summary>
        /// Gets or sets Schedule, if task scheduled, optional.
        /// </summary>
        public Schedule? Schedule { get; set; }

        /// <summary>
        /// Gets or sets foreign key to Category entity.
        /// </summary>
        public Guid? CategoryId { get; set; }

        /// <summary>
        /// Gets or sets category for task, optional.
        /// </summary>
        public Category? Category { get; set; }

        /// <summary>
        /// Gets or sets collection of task attempts.
        /// </summary>
        public ICollection<PomoUnit> Pomodoros { get; set; } = new List<PomoUnit>();
    }
}
