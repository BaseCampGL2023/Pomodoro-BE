using AutoMapper;
using Pomodoro.Core.Models.Tasks;
using Pomodoro.Core.Enums;
using Pomodoro.DataAccess.Entities;
using Pomodoro.Core.Models.Frequency;

namespace Pomodoro.Serices.Mapping
{
    public class TaskToReturnProfile : Profile
    {
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