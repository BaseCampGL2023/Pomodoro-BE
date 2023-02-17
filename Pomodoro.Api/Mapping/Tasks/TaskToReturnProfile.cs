// <copyright file="TaskToReturnProfile.cs" company="PomodoroGroup_GL_BaseCamp">
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
    public class TaskToReturnProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskToReturnProfile"/> class.
        /// </summary>
        public TaskToReturnProfile()
        {
            this.CreateMap<TaskEntity, TaskModel>()
                .ForMember(t => t.TaskId, o => o.MapFrom(s => s.Id))
                .ForMember(t => t.Frequency, o => o.MapFrom(s => s.Frequency.FrequencyType.Value))
                .ForMember(t => t.Every, o => o.MapFrom(s => s.Frequency.Every))
                .ForMember(t => t.Custom, o => o.MapFrom(s => s.Frequency.IsCustom));
            this.CreateMap<TaskModel, TaskToReturnModel>();
        }
    }
}