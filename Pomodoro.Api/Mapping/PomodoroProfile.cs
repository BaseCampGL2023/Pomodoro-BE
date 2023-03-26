// <copyright file="PomodoroProfile.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using AutoMapper;
using Pomodoro.Api.ViewModels;
using Pomodoro.Core.Models;

namespace Pomodoro.Api.Mapping
{
    /// <summary>
    /// Provides mapping configuration for pomodoro view model.
    /// Loaded by the <see cref="AutoMapper"/>.
    /// </summary>
    public class PomodoroProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PomodoroProfile"/> class.
        /// </summary>
        public PomodoroProfile()
        {
            this.CreateMap<CompletedModel, CompletedViewModel>().ReverseMap();
        }
    }
}
