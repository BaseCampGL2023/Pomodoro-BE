// <copyright file="FrequencyProfile.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using AutoMapper;
using Pomodoro.Core.Models.Frequency;
using Pomodoro.Core.Models.Tasks;
using Pomodoro.DataAccess.Entities;

namespace Pomodoro.Services.Mapping
{
    /// <summary>
    /// Provides mapping configuration for frequnecy model.
    /// Loaded by the <see cref="AutoMapper"/>.
    /// </summary>
    public class FrequencyProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FrequencyProfile"/> class.
        /// </summary>
        public FrequencyProfile()
        {
            CreateMap<TaskModel, FrequencyModel>()
                .ForMember(f => f.FrequencyTypeValue, o => o.MapFrom(s => s.Frequency.FrequencyTypeValue))
                .ForMember(f => f.IsCustom, o => o.MapFrom(s => s.Frequency.IsCustom))
                .ForMember(f => f.Every, o => o.MapFrom(s => s.Frequency.Every));
            CreateMap<Frequency, FrequencyModel>()
                .ForMember(f => f.FrequencyTypeValue, o => o.MapFrom(s => s.FrequencyType.Value));
            CreateMap<FrequencyModel, Frequency>()
                .ForMember(f => f.FrequencyType.Value, o => o.MapFrom(s => s.FrequencyTypeValue));
        }
    }
}
