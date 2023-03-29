using AutoMapper;
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

        private float GetProgress(TaskEntity task)
        {
            if (task == null || task.Pomodoros == null || task.Pomodoros.Count == 0)
                return 0f;

            if (task.AllocatedTime == 0 || task.Pomodoros.Last().TaskIsDone)
                return 100f;

            var totalTimeSpent = task.Pomodoros.Sum(ct => ct.TimeSpent);

            return totalTimeSpent * 100f / task.AllocatedTime;
        }
    }
}