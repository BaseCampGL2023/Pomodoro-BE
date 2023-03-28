// <copyright file="TaskViewModel.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;

namespace Pomodoro.Api.ViewModels
{
    /// <summary>
    /// Represents a view model for task.
    /// </summary>
    public class TaskViewModel
    {
        /// <summary>
        /// Gets or sets a value of the id of the task.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets a value of the title of the task.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Gets or sets a value of the inital date of the task.
        /// </summary>
        public DateTime InitialDate { get; set; }

        /// <summary>
        /// Gets or sets a value of the allocated time of the task.
        /// </summary>
        [Range(1, byte.MaxValue, ErrorMessage = "The {0} property must be in the range from {1} to {2}.")]
        public short AllocatedTime { get; set; }

        /// <summary>
        /// Gets or sets an information about the frequency used in the task.
        /// </summary>
        public FrequencyViewModel? Frequency { get; set; }

        /// <summary>
        /// Gets or sets task progress.
        /// </summary>
        [Range(0, byte.MaxValue, ErrorMessage = "The {0} property must be in the range from {1} to {2}.")]
        public byte Progress { get; set; }
    }
}
