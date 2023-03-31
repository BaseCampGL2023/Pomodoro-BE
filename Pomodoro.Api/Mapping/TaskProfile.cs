// <copyright file="TaskProfile.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using AutoMapper;
using Pomodoro.Api.ViewModels;
using Pomodoro.Core.Models;

namespace Pomodoro.Api.Mapping.Tasks
{
    /// <summary>
    /// Provides mapping configuration for task view model.
    /// Loaded by the <see cref="AutoMapper"/>.
    /// </summary>
    public class TaskProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskProfile"/> class.
        /// </summary>
        public TaskProfile()
        {
            this.CreateMap<TaskModel, TaskViewModel>().ReverseMap();
        }
    }
}