// <copyright file="FrequencyProfile.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using AutoMapper;
using Pomodoro.Core.Enums;
using Pomodoro.Core.Models.Frequency;
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
            CreateMap<Frequency, FrequencyModel>()
                .ForMember(dist => dist.FrequencyValue, act => act.MapFrom(src => GetFrequencyValue(src)));
            CreateMap<FrequencyModel, Frequency>()
                .ForMember(dist => dist.Id, act => act.MapFrom(src => Guid.Empty));

        }

        private FrequencyValue GetFrequencyValue(Frequency frequency)
        {
            if (frequency == null || frequency.FrequencyType == null)
                return 0;

            return frequency.FrequencyType.Value;
        }
    }
}
