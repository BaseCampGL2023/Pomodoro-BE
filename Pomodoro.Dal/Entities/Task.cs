// <copyright file="Task.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;
using Pomodoro.Dal.Entities.Base;

namespace Pomodoro.Dal.Entities
{
    /// <summary>
    /// Describes user task.
    /// </summary>
    public class Task : BaseEntity
    {
        /// <summary>
        /// Gets or sets task title.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets task description, optional.
        /// </summary>
        [StringLength(1000)]
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets DateTime when task created.
        /// </summary>
        public DateTime CreatedDt { get; set; }

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
        public ICollection<TaskAttempt>? Attempts { get; set; }
    }
}
