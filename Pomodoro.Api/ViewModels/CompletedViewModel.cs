// <copyright file="CompletedViewModel.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

namespace Pomodoro.Api.ViewModels
{
    /// <summary>
    /// Represents a view model for pomodoro.
    /// </summary>
    public class CompletedViewModel
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
        public int TimeSpent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether pomodoro is done.
        /// </summary>
        public bool IsDone { get; set; }
    }
}
