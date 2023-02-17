// <copyright file="TaskToCreateModel.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

namespace Pomodoro.Api.ViewModels.Tasks
{
    /// <summary>
    /// Represents a view model which includes all information for creating a task.
    /// </summary>
    public class TaskToCreateModel
    {
        /// <summary>
        /// Gets or sets an enum value of Frequency that represents how often the task will execute.
        /// </summary>
        public string Frequency { get; set; } = "None";

        /// <summary>
        /// Gets or sets the title of task.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Gets or sets the initial date of start for the task.
        /// </summary>
        public DateTime InitialDate { get; set; }

        /// <summary>
        /// Gets or sets the allocated time for the task.
        /// </summary>
        public short AllocatedTime { get; set; }

        /// <summary>
        /// Gets or sets the value of how often in numeric the task will execute.
        /// </summary>
        public short Every { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether it is a custom frequency or not.
        /// </summary>
        public bool Custom { get; set; } = false;
    }
}
