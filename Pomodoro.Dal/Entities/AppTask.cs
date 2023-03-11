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
        /// Gets or sets DateTime when task created.
        /// </summary>
        public DateTime CreatedDt { get; set; }

        /// <summary>
        /// Gets or sets planned start time.
        /// </summary>
        public DateTime? StartTime { get; set; }

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
        /// Gets or sets collection of task attempts.
        /// </summary>
        public ICollection<AppTaskAttempt>? Attempts { get; set; }
    }
}
