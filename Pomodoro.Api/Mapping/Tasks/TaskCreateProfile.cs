// <copyright file="TaskCreateProfile.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using AutoMapper;
using Pomodoro.Api.ViewModels.Tasks;
using Pomodoro.Core.Models.Tasks;
using Pomodoro.DataAccess.Entities;

namespace Pomodoro.Api.Mapping.Tasks
{
    /// <summary>
    /// Provides mapping configuration for task creation model.
    /// Loaded by the <see cref="AutoMapper"/>.
    /// </summary>
    public class TaskCreateProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskCreateProfile"/> class.
        /// </summary>
        public TaskCreateProfile()
        {
            this.CreateMap<TaskToCreateModel, TaskModel>();
            this.CreateMap<TaskToManipulateModel, TaskModel>();
        }
    }
}