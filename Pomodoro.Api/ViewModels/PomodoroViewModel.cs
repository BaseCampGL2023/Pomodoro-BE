// <copyright file="PomodoroViewModel.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;

namespace Pomodoro.Api.ViewModels
{
    /// <summary>
    /// Represents a view model for pomodoro.
    /// </summary>
    public class PomodoroViewModel
    {
        /// <summary>
        /// Gets or sets a value of the id of the pomodoro.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets a value of the id of the task.
        /// </summary>
        public Guid TaskId { get; set; }

        /// <summary>
        /// Gets or sets a value of the actual date of the pomodoro.
        /// </summary>
        public DateTime ActualDate { get; set; }

        /// <summary>
        /// Gets or sets a value of the spent dime of the pomodoro.
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "The {0} property must be in the range from {1} to {2}.")]
        public int TimeSpent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether pomodoro is done.
        /// </summary>
        public bool IsDone { get; set; }
    }
}
