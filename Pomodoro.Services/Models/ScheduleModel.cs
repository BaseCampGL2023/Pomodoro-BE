// <copyright file="ScheduleModel.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;
using Pomodoro.Services.Enums;

namespace Pomodoro.Services.Models
{
    /// <summary>
    /// Represents the schedule.
    /// </summary>
    public class ScheduleModel
    {
        /// <summary>
        /// Gets or sets schedule type.
        /// </summary>
        [Required]
        public ScheduleType Type { get; set; }

        /// <summary>
        /// Gets or sets array of active days in period.
        /// </summary>
        public int[] Days { get; set; } = Array.Empty<int>();

        // TODO: Add validation schedule
    }
}
