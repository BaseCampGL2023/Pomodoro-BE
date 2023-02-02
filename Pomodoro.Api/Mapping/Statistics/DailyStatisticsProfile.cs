// <copyright file="DailyStatisticsProfile.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using AutoMapper;
using Pomodoro.Api.ViewModels.Statistics;
using Pomodoro.Core.Models.Statistics;

namespace Pomodoro.Api.Mapping.Statistics
{
    /// <summary>
    /// Provides mapping configuration for models which represent daily statistics.
    /// Loaded by the <see cref="AutoMapper"/>.
    /// </summary>
    public class DailyStatisticsProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DailyStatisticsProfile"/> class.
        /// </summary>
        public DailyStatisticsProfile()
        {
            this.CreateMap<DailyStatistics, DailyStatisticsViewModel>();
            this.CreateMap<AnalyticsPerHour, AnalyticsPerHourViewModel>();
        }
    }
}
