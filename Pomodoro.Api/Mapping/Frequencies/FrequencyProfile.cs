// <copyright file="FrequencyProfile.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using AutoMapper;
using Pomodoro.Core.Models.Frequency;
using Pomodoro.Core.Models.Tasks;
using Pomodoro.DataAccess.Entities;

namespace Pomodoro.Api.Mapping.Frequencies
{
    /// <summary>
    /// Provides mapping configuration for frequnecy manipulation model.
    /// Loaded by the <see cref="AutoMapper"/>.
    /// </summary>
    public class FrequencyProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FrequencyProfile"/> class.
        /// </summary>
        public FrequencyProfile()
        {
            this.CreateMap<TaskModel, FrequencyModel>()
                .ForMember(f => f.FrequencyTypeValue, o => o.MapFrom(s => s.FrequencyData.FrequencyTypeValue))
                .ForMember(f => f.IsCustom, o => o.MapFrom(s => s.FrequencyData.IsCustom))
                .ForMember(f => f.Every, o => o.MapFrom(s => s.FrequencyData.Every));
            this.CreateMap<Frequency, FrequencyModel>()
                .ForMember(f => f.FrequencyTypeValue, o => o.MapFrom(s => s.FrequencyType.Value));
            this.CreateMap<FrequencyModel, Frequency>();
            this.CreateMap<FrequencyViewModel, FrequencyModel>();
            this.CreateMap<FrequencyModel, FrequencyViewModel>();
        }
    }
}
