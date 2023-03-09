﻿// <copyright file="TaskAttempt.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;
using Pomodoro.Dal.Entities.Base;

namespace Pomodoro.Dal.Entities
{
    /// <summary>
    /// Describes task performance round.
    /// </summary>
    public class TaskAttempt : BaseEntity
    {
        /// <summary>
        /// Gets or sets DateTime when task performing started.
        /// </summary>
        public DateTime StartDt { get; set; }

        /// <summary>
        /// Gets or sets duration of tasks performing round.
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Gets or sets optional comment.
        /// </summary>
        [StringLength(1000)]
        public string? Comment { get; set; }

        /// <summary>
        /// Gets or sets foreign key to Task entity.
        /// </summary>
        public Guid TaskId { get; set; }

        /// <summary>
        /// Gets or sets navigation property represents Task.
        /// </summary>
        public Task? Task { get; set; }
    }
}
