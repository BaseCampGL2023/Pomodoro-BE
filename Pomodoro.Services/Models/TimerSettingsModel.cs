// <copyright file="TimerSettingsModel.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;
using Pomodoro.Dal.Entities;

namespace Pomodoro.Services.Models
{
    /// <summary>
    /// Represent pomodoro timer settings for client.
    /// </summary>
    public class TimerSettingsModel
    {
        /// <summary>
        /// Gets or sets duration of working period for client.
        /// </summary>
        [Required(ErrorMessage = "Pomodoro duration value is required.")]
        [Range(60, 3600, ErrorMessage = "Should be between 1 minute to 1 hour.")]
        public int Pomodoro { get; set; }

        /// <summary>
        /// Gets or sets duration of short break period for client.
        /// </summary>
        [Required(ErrorMessage = "Short brake duration value is required.")]
        [Range(60, 3600, ErrorMessage = "Should be between 1 minute to 1 hour.")]
        public int ShortBrake { get; set; }

        /// <summary>
        /// Gets or sets duration of long break period for client.
        /// </summary>
        [Required(ErrorMessage = "Long brake duration value is required.")]
        [Range(60, 3600, ErrorMessage = "Should be between 1 minute to 1 hour.")]
        public int LongBrake { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether autostart enable.
        /// </summary>
        [Required(ErrorMessage = "Autostart property is required.")]
        public bool IsAutoStart { get; set; }

        /// <summary>
        /// Gets or sets value that defines the sequence of rest intervals.
        /// <example>
        /// If RestSequnce value == 3, then sequence of rest periods: ShortBrake -> ShortBrake -> ShortBrake -> LongBreak
        /// </example>
        /// </summary>
        [Required(ErrorMessage = "Rest sequence value is required.")]
        [Range(1, 20, ErrorMessage = "Should be between 1 to 20.")]
        public int RestSequence { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the configuration data is the current user settings in DB.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Map from model to Dal entity.
        /// </summary>
        /// <param name="userId">Owner id.</param>
        /// <returns>Dal entity.</returns>
        public TimerSettings ToDalEntity(Guid userId)
        {
            return new TimerSettings
            {
                Pomodoro = TimeSpan.FromSeconds(this.Pomodoro),
                ShortBrake = TimeSpan.FromSeconds(this.ShortBrake),
                LongBreak = TimeSpan.FromSeconds(this.LongBrake),
                IsAutoStart = this.IsAutoStart,
                IsActive = this.IsActive,
                AppUserId = userId,
            };
        }

        /// <summary>
        /// Map from Dal entity to model object.
        /// </summary>
        /// <param name="entity">Instance of TimerSettings <see cref="TimerSettings"/>.</param>
        /// <returns>Model object.</returns>
        public static TimerSettingsModel Create(TimerSettings entity)
        {
            return new TimerSettingsModel
            {
                Pomodoro = (int)entity.Pomodoro.TotalSeconds,
                ShortBrake = (int)entity.Pomodoro.TotalSeconds,
                LongBrake = (int)entity.Pomodoro.TotalSeconds,
                IsAutoStart = entity.IsAutoStart,
                RestSequence = entity.RestSequence,
                IsActive = entity.IsActive,
            };
        }
    }
}
