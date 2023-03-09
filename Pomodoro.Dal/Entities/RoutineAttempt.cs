// <copyright file="RoutineAttempt.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;
using Pomodoro.Dal.Entities.Base;

namespace Pomodoro.Dal.Entities
{
    /// <summary>
    /// Describes an attempt to execute a scheduled task.
    /// </summary>
    public class RoutineAttempt : BaseEntity
    {
        /// <summary>
        /// Gets or sets number of current performed routine in scheduled sequence.
        /// </summary>
        public int NumberInSequence { get; set; }

        /// <summary>
        /// Gets or sets DateTime when routine performing started.
        /// </summary>
        public DateTime StartDt { get; set; }

        /// <summary>
        /// Gets or sets DateTime when routine execution round finished.
        /// </summary>
        public DateTime FinishDt { get; set; }

        /// <summary>
        /// Gets or sets optional comment.
        /// </summary>
        [StringLength(1000)]
        public string? Comment { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether sheduled task complete successfully.
        /// </summary>
        public bool IsSuccesful { get; set; }

        /// <summary>
        /// Gets or sets foreign key to Routine entity.
        /// </summary>
        public Guid RoutineId { get; set; }

        /// <summary>
        /// Gets or sets navigation property represents Routine.
        /// </summary>
        public Routine? Routine { get; set; }
    }
}
