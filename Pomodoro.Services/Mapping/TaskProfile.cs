﻿using AutoMapper;
using Pomodoro.Core.Enums;
using Pomodoro.Core.Models;
using Pomodoro.DataAccess.Entities;

namespace Pomodoro.Services.Mapping
{
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            CreateMap<TaskModel, TaskEntity>()
                .ForMember(dest => dest.Id, act => act.MapFrom(src => Guid.Empty))
                .AfterMap((src, dist) => dist.Frequency = null);
            CreateMap<TaskEntity, TaskModel>()
                .ForMember(dest => dest.Frequency, act => act.MapFrom(src => src.Frequency))
                .ForMember(dest => dest.Progress, act => act.MapFrom(src => GetProgress(src)));
        }

        private byte GetProgress(TaskEntity task)
        {
            if (task == null || task.CompletedTasks == null || task.CompletedTasks.Count == 0)
                return 0;

            if (task.AllocatedTime == 0 || task.CompletedTasks.Last().IsDone)
                return 100;

            var totalTimeSpent = task.CompletedTasks.Sum(ct => ct.TimeSpent);

            return (byte)(totalTimeSpent * 100 / task.AllocatedTime);
        }
    }
}