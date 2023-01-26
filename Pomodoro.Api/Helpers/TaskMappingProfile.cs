using AutoMapper;
using Pomodoro.Api.ViewModels;
using Pomodoro.DataAccess.Entities;

namespace Pomodoro.Api.Helpers;

public class TaskMappingProfile : Profile
{
    public TaskMappingProfile()
    {
        this.CreateMap<TaskEntity, TaskViewModel>();
        this.CreateMap<TaskViewModel, TaskEntity>();
    }
}
