// <copyright file="AnalyticsPerMonthViewModel.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

namespace Pomodoro.Api.ViewModels.Statistics
{
    /// <summary>
    /// Represents a view model which includes analytics data per one month.
    /// Used for the <see cref="AnnualStatisticsViewModel"/>.
    /// </summary>
    public class AnalyticsPerMonthViewModel
    {
        /// <summary>
        /// Gets or sets a month number (1-12) for which analytics is gathered.
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// Gets or sets a number of pomodoros done per the <see cref="Month"/>.
        /// </summary>
        public int PomodorosDone { get; set; }
    }
}
