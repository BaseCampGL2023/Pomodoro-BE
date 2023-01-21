// <copyright file="AnalyticsPerHourViewModel.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

namespace Pomodoro.Api.ViewModels.Statistics
{
    /// <summary>
    /// Represents a view model which includes analytics data per one hour.
    /// Used for the <see cref="DailyStatisticsViewModel"/>.
    /// </summary>
    public class AnalyticsPerHourViewModel
    {
        /// <summary>
        /// Gets or sets an hour (in the 24-hour clock format) for which analytics is gathered.
        /// </summary>
        public int Hour { get; set; }

        /// <summary>
        /// Gets or sets a number of pomodoros done per the <see cref="Hour"/>.
        /// </summary>
        public int PomodorosDone { get; set; }

        /// <summary>
        /// Gets or sets time (in minutes) spent on pomodoros during the <see cref="Hour"/>.
        /// </summary>
        public float TimeSpent { get; set; }
    }
}
