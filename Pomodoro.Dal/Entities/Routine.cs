// <copyright file="Routine.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;
using Pomodoro.Dal.Entities.Base;

namespace Pomodoro.Dal.Entities
{
    /// <summary>
    /// Describes user routine.
    /// </summary>
    public class Routine : BaseEntity
    {
        /// <summary>
        /// Gets or sets pattern of frequency of task execution.
        /// </summary>
        public long Template { get; set; }

        /// <summary>
        /// Gets or sets routine title.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets routine description, optional.
        /// </summary>
        [StringLength(1000)]
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets DateTime when routine created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets DateTime when routine finished.
        /// </summary>
        public DateTime? FinishAt { get; set; }

        /// <summary>
        /// Gets or sets planned duration of the routine round.
        /// </summary>
        public TimeSpan? AllocatedDuration { get; set; }

        /// <summary>
        /// Gets or sets planned start time.
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// Gets or sets foreign key to AppUser entity.
        /// </summary>
        public Guid AppUserId { get; set; }

        /// <summary>
        /// Gets or sets navigation property represents application user.
        /// </summary>
        public AppUser? AppUser { get; set; }

        /// <summary>
        /// Gets or sets collection of routine attempts.
        /// </summary>
        public ICollection<RoutineAttempt>? Attempts { get; set; }
    }
}
