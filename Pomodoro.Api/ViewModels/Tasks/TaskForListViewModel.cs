// <copyright file="TaskForListViewModel.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Pomodoro.Core.Enums;

namespace Pomodoro.Api.ViewModels.Tasks
{
    /// <summary>
    /// Represents a view model for task in task list.
    /// </summary>
    public class TaskForListViewModel
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
        /// Gets or sets a value of the allocated time of the task.
        /// </summary>
        public short AllocatedTime { get; set; }

        /// <summary>
        /// Gets or sets an information about the frequency used in the task.
        /// </summary>
        public string? Frequency { get; set; }

        /// <summary>
        /// Gets or sets task progress.
        /// </summary>
        public byte Progress { get; set; }
    }
}
