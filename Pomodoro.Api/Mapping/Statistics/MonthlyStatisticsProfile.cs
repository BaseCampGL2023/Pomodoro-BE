// <copyright file="MonthlyStatisticsProfile.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using AutoMapper;
using Pomodoro.Api.ViewModels.Statistics;
using Pomodoro.Core.Models.Statistics;

namespace Pomodoro.Api.Mapping.Statistics
{
    /// <summary>
    /// Provides mapping configuration for models which represent monthly statistics.
    /// Loaded by the <see cref="AutoMapper"/>.
    /// </summary>
    public class MonthlyStatisticsProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MonthlyStatisticsProfile"/> class.
        /// </summary>
        public MonthlyStatisticsProfile()
        {
            this.CreateMap<MonthlyStatistics, MonthlyStatisticsViewModel>();
        }
    }
}
