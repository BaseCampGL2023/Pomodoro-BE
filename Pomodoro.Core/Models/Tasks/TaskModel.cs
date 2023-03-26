// <copyright file="TaskModel.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>
using Pomodoro.Core.Models.Base;
using Pomodoro.Core.Models.Frequency;
using System.Text.Json.Serialization;

namespace Pomodoro.Core.Models.Tasks
{
    /// <summary>
    /// Represents a view model for task.
    /// </summary>
    public class TaskModel : BaseUserOrientedModel
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
        public short AllocatedTime { get; set; }

        /// <summary>
        /// Gets or sets an information about the frequency used in the task.
        /// </summary>
        public FrequencyModel? Frequency { get; set; }

        /// <summary>
        /// Gets or sets task progress.
        /// </summary>
        public byte Progress { get; set; }
    }
}
