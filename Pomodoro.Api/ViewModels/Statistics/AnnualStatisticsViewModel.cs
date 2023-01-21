// <copyright file="AnnualStatisticsViewModel.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

namespace Pomodoro.Api.ViewModels.Statistics
{
    /// <summary>
    /// Represents a view model which includes annual statistics for a certain user.
    /// </summary>
    public class AnnualStatisticsViewModel : BaseUserOrientedViewModel
    {
        /// <summary>
        /// Gets or sets a year for which statistics is gathered.
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Gets or sets a list of analytics data per every month during the <see cref="Year"/>.
        /// </summary>
        public List<AnalyticsPerMonthViewModel>? AnalyticsPerMonths { get; set; }
    }
}
