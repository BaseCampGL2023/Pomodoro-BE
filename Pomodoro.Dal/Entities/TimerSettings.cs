// <copyright file="TimerSettings.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Pomodoro.Dal.Entities.Base;

namespace Pomodoro.Dal.Entities
{
    /// <summary>
    /// Represent pomodoro timer settings.
    /// </summary>
    public class TimerSettings : BaseEntity
    {
        /// <summary>
        /// Gets or sets duration of working period.
        /// </summary>
        public TimeSpan Pomodoro { get; set; }

        /// <summary>
        /// Gets or sets duration of short break period.
        /// </summary>
        public TimeSpan ShortBrake { get; set; }

        /// <summary>
        /// Gets or sets duration of long break period.
        /// </summary>
        public TimeSpan LongBreak { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether autostart enable.
        /// </summary>
        public bool IsAutoStart { get; set; }

        /// <summary>
        /// Gets or sets value that defines the sequence of rest intervals.
        /// <example>
        /// If RestSequnce value == 3, then sequence of rest periods: ShortBrake -> ShortBrake -> ShortBrake -> LongBreak
        /// </example>
        /// </summary>
        public byte RestSequence { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the configuration data is the current user settings.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets foreign key to AppUser entity.
        /// </summary>
        public Guid AppUserId { get; set; }

        /// <summary>
        /// Gets or sets navigation property represents application user.
        /// </summary>
        public AppUser? AppUser { get; set; }
    }
}
