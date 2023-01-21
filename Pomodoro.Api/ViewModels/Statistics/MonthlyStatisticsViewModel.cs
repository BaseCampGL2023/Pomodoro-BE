// <copyright file="MonthlyStatisticsViewModel.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

namespace Pomodoro.Api.ViewModels.Statistics
{
    /// <summary>
    /// Represents a view model which includes monthly statistics for a certain user.
    /// </summary>
    public class MonthlyStatisticsViewModel : BaseUserOrientedViewModel
    {
        /// <summary>
        /// Gets or sets a year for which statistics is gathered.
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Gets or sets a month number (1-12) for which statistics is gathered.
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// Gets or sets a number of tasks completed during the <see cref="Month"/> of the <see cref="Year"/>.
        /// </summary>
        public int TasksCompleted { get; set; }

        /// <summary>
        /// Gets or sets a number of pomodoros done per the <see cref="Month"/> of the <see cref="Year"/>.
        /// </summary>
        public int PomodorosDone { get; set; }

        /// <summary>
        /// Gets or sets time (in minutes) spent on pomodoros during the <see cref="Month"/> of the <see cref="Year"/>.
        /// </summary>
        public int TimeSpent { get; set; }
    }
}
