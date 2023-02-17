// <copyright file="TaskToManipulateModel.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

namespace Pomodoro.Api.ViewModels.Tasks
{
    /// <summary>
    /// Represents a view model for manupulating task data (updating or deleting).
    /// </summary>
    public class TaskToManipulateModel : BaseUserOrientedViewModel
    {
        /// <summary>
        /// Gets or sets the id the of task.
        /// </summary>
        public Guid TaskId { get; set; }

        /// <summary>
        /// Gets or sets the frequency the task starts with.
        /// </summary>
        public string? Frequency { get; set; }

        /// <summary>
        /// Gets or sets the title of task.
        /// </summary>
        public string Title { get; set; }

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
        public bool Custom { get; set; }
    }
}