// <copyright file="TaskViewModel.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>
using Pomodoro.Api.ViewModels.Frequency;

namespace Pomodoro.Api.ViewModels.Tasks
{
    /// <summary>
    /// Represents a view model for task.
    /// </summary>
    public class TaskViewModel : BaseUserOrientedViewModel
    {
        /// <summary>
        /// Gets or sets a value of the id of the task.
        /// </summary>
        public Guid TaskId { get; set; }

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
        public short AllocatedTime { get; set; }

        /// <summary>
        /// Gets or sets an information about the frequency used in the task.
        /// </summary>
        public FrequencyViewModel? FrequencyData { get; set; }
    }
}
