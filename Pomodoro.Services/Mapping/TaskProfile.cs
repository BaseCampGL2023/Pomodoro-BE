// <copyright file="TaskProfile.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using AutoMapper;
using Pomodoro.Core.Enums;
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
            CreateMap<TaskModel, TaskEntity>()
                .ForMember(dest => dest.Id, act => act.MapFrom(src => Guid.Empty))
                .AfterMap((src, dist) => dist.Frequency = null);
            CreateMap<TaskEntity, TaskForListModel>()
                .ForMember(dest => dest.Frequency, act => act.MapFrom(src => GetFrequencyValue(src)))
                .ForMember(dest => dest.Progress, act => act.MapFrom(src => GetProgress(src)));
        }

        private byte GetProgress(TaskEntity task)
        {
            if (task == null || task.CompletedTasks == null)
                return 0;

            if (task.AllocatedTime == 0)
                return 100;

            var totalTimeSpent = task.CompletedTasks.Sum(ct => ct.TimeSpent);

            return (byte)(totalTimeSpent * 100 / task.AllocatedTime);
        }

        private FrequencyValue GetFrequencyValue(TaskEntity task)
        {
            if(task == null || task.Frequency == null || task.Frequency.FrequencyType == null)
                return 0;

            return task.Frequency.FrequencyType.Value;
        }
    }
}