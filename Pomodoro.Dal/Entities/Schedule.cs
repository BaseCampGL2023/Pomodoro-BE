// <copyright file="Schedule.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Pomodoro.Dal.Entities.Base;
using Pomodoro.Dal.Enums;

namespace Pomodoro.Dal.Entities
{
    /// <summary>
    /// Describes user routine.
    /// </summary>
    public class Schedule : BaseEntity, IBelongEntity
    {
        /// <summary>
        /// Gets or sets pattern of frequency of task execution.
        /// </summary>
        public string Template { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets schedule type (WeekDay, WeekEnd, Dayli, Sequence etc.).
        /// </summary>
        public ScheduleType ScheduleType { get; set; }

        /// <summary>
        /// Gets or sets routine title.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the schedule is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets routine description, optional.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets foreign key for Category, optional.
        /// </summary>
        public Guid? CategoryId { get; set; }

        /// <summary>
        /// Gets or sets navigation property represents category, optional.
        /// </summary>
        public Category? Category { get; set; }

        /// <summary>
        /// Gets or sets DateTime when routine created.
        /// </summary>
        public DateTime CreatedDt { get; set; }

        /// <summary>
        /// Gets or sets DateTime when routine modified.
        /// </summary>
        public DateTime? ModifiedDt { get; set; }

        /// <summary>
        /// Gets or sets DateTime when routine finished.
        /// </summary>
        public DateTime? FinishDt { get; set; }

        /// <summary>
        /// Gets or sets planned duration of the routine round.
        /// </summary>
        public TimeSpan? AllocatedDuration { get; set; }

        /// <summary>
        /// Gets or sets planned start time.
        /// </summary>
        public DateTime StartDt { get; set; }

        /// <summary>
        /// Gets or sets foreign key to Schedule entity.
        /// </summary>
        public Guid? PreviousId { get; set; }

        /// <summary>
        /// Gets or sets navigation property represents shedule, optional.
        /// Created if schedule type or schedule template for existing scheduled task
        /// changed.
        /// </summary>
        public Schedule? Previous { get; set; }

        /// <summary>
        /// Gets or sets foreign key to AppUser entity.
        /// </summary>
        public Guid AppUserId { get; set; }

        /// <summary>
        /// Gets or sets navigation property represents application user.
        /// </summary>
        public AppUser? AppUser { get; set; }

        /// <summary>
        /// Gets or sets collection of AppTasks related to this schedule.
        /// </summary>
        public ICollection<AppTask> Tasks { get; set; } = new List<AppTask>();
    }
}
