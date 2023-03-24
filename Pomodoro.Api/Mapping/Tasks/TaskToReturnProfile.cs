// <copyright file="TaskToReturnProfile.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using AutoMapper;
using Pomodoro.Api.ViewModels.Tasks;
using Pomodoro.Core.Models.Tasks;

namespace Pomodoro.Api.Mapping.Tasks
{
    /// <summary>
    /// Provides mapping configuration for task creation model.
    /// Loaded by the <see cref="AutoMapper"/>.
    /// </summary>
    public class TaskToReturnProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskToReturnProfile"/> class.
        /// </summary>
        public TaskToReturnProfile()
        {
            this.CreateMap<TaskViewModel, TaskModel>();
            this.CreateMap<TaskModel, TaskViewModel>();
        }
    }
}