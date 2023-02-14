// <copyright file="SettingsProfile.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using AutoMapper;
using Pomodoro.Api.ViewModels;
using Pomodoro.Core.Models;

namespace Pomodoro.Api.Mapping
{
    /// <summary>
    /// Provides mapping configuration for models which represent pomodoro settings.
    /// </summary>
    public class SettingsProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsProfile"/> class.
        /// </summary>
        public SettingsProfile()
        {
            this.CreateMap<SettingsModel, SettingsViewModel>()
                .ReverseMap();
        }
    }
}
