// <copyright file="AnnualStatisticsProfile.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using AutoMapper;
using Pomodoro.Api.ViewModels.Statistics;
using Pomodoro.Core.Models.Statistics;

namespace Pomodoro.Api.Mapping.Statistics
{
    /// <summary>
    /// Provides mapping configuration for models which represent annual statistics.
    /// Loaded by the <see cref="AutoMapper"/>.
    /// </summary>
    public class AnnualStatisticsProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AnnualStatisticsProfile"/> class.
        /// </summary>
        public AnnualStatisticsProfile()
        {
            this.CreateMap<AnnualStatistics, AnnualStatisticsViewModel>();
            this.CreateMap<AnalyticsPerMonth, AnalyticsPerMonthViewModel>();
        }
    }
}
