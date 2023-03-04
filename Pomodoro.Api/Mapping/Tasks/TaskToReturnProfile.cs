// <copyright file="TaskToReturnProfile.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using AutoMapper;
using Pomodoro.Core.Models.Tasks;
using Pomodoro.Core.Enums;
using Pomodoro.DataAccess.Entities;
using Pomodoro.Core.Models.Frequency;

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
                .ForPath(f => f.FrequencyData.FrequencyTypeValue, o => o.MapFrom(s => s.Frequency.FrequencyType.Value))
                .ForPath(f => f.FrequencyData.IsCustom, o => o.MapFrom(s => s.Frequency.IsCustom))
                .ForPath(f => f.FrequencyData.Every, o => o.MapFrom(s => s.Frequency.Every));
            this.CreateMap<TaskModel, TaskEntity>()
                .ForMember(t => t.Id, o => o.MapFrom(s => s.TaskId));
        }
    }
}