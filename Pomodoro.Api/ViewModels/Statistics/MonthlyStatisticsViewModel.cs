﻿// <copyright file="MonthlyStatisticsViewModel.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Pomodoro.Core.Enums;

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
        /// Gets or sets a <see cref="Core.Enums.Month"/> for which statistics is gathered.
        /// </summary>
        public Month Month { get; set; }

        /// <summary>
        /// Gets or sets a number of tasks completed during the <see cref="Month"/> of the <see cref="Year"/>.
        /// </summary>
        public int TasksCompleted { get; set; }

        /// <summary>
        /// Gets or sets a number of pomodoros done per the <see cref="Month"/> of the <see cref="Year"/>.
        /// </summary>
        public int PomodorosDone { get; set; }

        /// <summary>
        /// Gets or sets time (in seconds) spent on pomodoros during the <see cref="Month"/> of the <see cref="Year"/>.
        /// </summary>
        public int TimeSpent { get; set; }
    }
}