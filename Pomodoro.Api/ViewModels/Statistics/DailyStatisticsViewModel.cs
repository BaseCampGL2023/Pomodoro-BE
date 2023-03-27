// <copyright file="DailyStatisticsViewModel.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Pomodoro.Api.ViewModels.Base;

namespace Pomodoro.Api.ViewModels.Statistics
{
    /// <summary>
    /// Represents a view model which includes daily statistics for a certain user.
    /// </summary>
    public class DailyStatisticsViewModel : BaseUserOrientedViewModel
    {
        /// <summary>
        /// Gets or sets a day for which statistics is gathered.
        /// </summary>
        public DateOnly Day { get; set; }

        /// <summary>
        /// Gets or sets a list of analytics data per every hour during the <see cref="Day"/>.
        /// </summary>
        public List<AnalyticsPerHourViewModel>? AnalyticsPerHours { get; set; }
    }
}
