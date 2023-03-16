// <copyright file="TaskProfile.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using AutoMapper;
using Pomodoro.Core.Models.Tasks;
using Pomodoro.DataAccess.Entities;

namespace Pomodoro.Services.Mapping
{
    /// <summary>
    /// Provides mapping configuration for task model.
    /// Loaded by the <see cref="AutoMapper"/>.
    /// </summary>
    public class TaskProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskProfile"/> class.
        /// </summary>
        public TaskProfile()
        {
            CreateMap<TaskEntity, TaskModel>();
            CreateMap<TaskModel, TaskEntity>();
        }
    }
}