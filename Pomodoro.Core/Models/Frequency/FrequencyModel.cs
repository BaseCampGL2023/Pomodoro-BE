// <copyright file="FrequencyModel.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>
using Pomodoro.Core.Enums;
using Pomodoro.Core.Models.Tasks;

namespace Pomodoro.Core.Models.Frequency
{
    /// <summary>
    /// Represents a view model for Frequency information.
    /// Used for the <see cref="TaskModel"/>.
    /// </summary>
    public class FrequencyModel
    {
        /// <summary>
        /// Gets or sets a value of the id of the frequency.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets a type of frequency that is used by user in this task.
        /// </summary>
        public FrequencyValue FrequencyType { get; set; } = FrequencyValue.None;

        /// <summary>
        /// Gets or sets a value indicating whether it is a custom frequency or not.
        /// </summary>
        public bool IsCustom { get; set; }

        /// <summary>
        /// Gets or sets a value of short type indicating how often the task should repeat.
        /// </summary>
        public short Every { get; set; }
    }
}
