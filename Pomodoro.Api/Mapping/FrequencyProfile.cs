// <copyright file="FrequencyProfile.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using AutoMapper;
using Pomodoro.Api.ViewModels;
using Pomodoro.Core.Models.Frequency;

namespace Pomodoro.Api.Mapping
{
    /// <summary>
    /// Provides mapping configuration for frequnecy view model.
    /// Loaded by the <see cref="AutoMapper"/>.
    /// </summary>
    public class FrequencyProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FrequencyProfile"/> class.
        /// </summary>
        public FrequencyProfile()
        {
            this.CreateMap<FrequencyViewModel, FrequencyModel>();
            this.CreateMap<FrequencyModel, FrequencyViewModel>();
        }
    }
}
