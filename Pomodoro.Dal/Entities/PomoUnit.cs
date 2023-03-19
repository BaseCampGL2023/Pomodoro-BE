// <copyright file="PomoUnit.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Pomodoro.Dal.Entities.Base;

namespace Pomodoro.Dal.Entities
{
    /// <summary>
    /// Describes task performance round.
    /// </summary>
    public class PomoUnit : BaseEntity
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
        public string? Comment { get; set; }

        /// <summary>
        /// Gets or sets foreign key to AppTask entity.
        /// </summary>
        public Guid TaskId { get; set; }

        /// <summary>
        /// Gets or sets navigation property represents AppTask.
        /// </summary>
        public AppTask? Task { get; set; }

        /// <summary>
        /// Gets or sets foreign key to TimerSettings entity.
        /// </summary>
        public Guid? TimerSettingsId { get; set; }

        /// <summary>
        /// Gets or sets navigation property represents TimerSettings.
        /// </summary>
        public TimerSettings? TimerSettings { get; set; }
    }
}
