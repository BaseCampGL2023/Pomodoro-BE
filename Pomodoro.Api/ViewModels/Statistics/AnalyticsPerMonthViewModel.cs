// <copyright file="AnalyticsPerMonthViewModel.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Pomodoro.Core.Enums;

namespace Pomodoro.Api.ViewModels.Statistics
{
    /// <summary>
    /// Represents a view model which includes analytics data per one month.
    /// Used for the <see cref="AnnualStatisticsViewModel"/>.
    /// </summary>
    public class AnalyticsPerMonthViewModel
    {
        /// <summary>
        /// Gets or sets a <see cref="Core.Enums.Month"/> for which analytics is gathered.
        /// </summary>
        public Month Month { get; set; }

        /// <summary>
        /// Gets or sets a number of pomodoros done per the <see cref="Month"/>.
        /// </summary>
        public int PomodorosDone { get; set; }
    }
}
