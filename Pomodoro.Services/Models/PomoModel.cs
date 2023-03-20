// <copyright file="PomoModel.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;
using Pomodoro.Dal.Entities;

namespace Pomodoro.Services.Models
{
    /// <summary>
    /// Represent pomodoro for client.
    /// </summary>
    public class PomoModel
    {
        /// <summary>
        /// Gets or sets DateTime when task performing started.
        /// </summary>
        [Required(ErrorMessage = "Start date and time is required.")]
        public DateTime StartDt { get; set; }

        /// <summary>
        /// Gets or sets duration of tasks performing round.
        /// </summary>
        [Required(ErrorMessage = "Duration is required.")]
        public int Duration { get; set; }

        /// <summary>
        /// Gets or sets optional comment.
        /// </summary>
        public string? Comment { get; set; }

        /// <summary>
        /// Gets or sets foreign key to AppTask entity.
        /// </summary>
        [Required(ErrorMessage = "Task id is required.")]
        public Guid TaskId { get; set; }

        /// <summary>
        /// Gets or sets foreign key to TimerSettings entity.
        /// </summary>
        public Guid? TimerSettingsId { get; set; }

        /// <summary>
        /// Map from Dal entity to model object.
        /// </summary>
        /// <param name="pomodoro">Instance of PomoUnit <see cref="PomoUnit"/>.</param>
        /// <returns>Model object.</returns>
        public static PomoModel Create(PomoUnit pomodoro)
        {
            return new PomoModel
            {
                StartDt = pomodoro.StartDt,
                Duration = (int)pomodoro.Duration.TotalSeconds,
                Comment = pomodoro.Comment,
            };
        }

        /// <summary>
        /// Map from model to Dal entity.
        /// </summary>
        /// <returns>Dal entity.</returns>
        public PomoUnit ToDalEntity()
        {
            return new PomoUnit
            {
                StartDt = this.StartDt,
                Duration = TimeSpan.FromSeconds(this.Duration),
                Comment = this.Comment,
                TaskId = this.TaskId,
                TimerSettingsId = this.TimerSettingsId,
            };
        }
    }
}
