using AutoMapper;
using Pomodoro.Core.Models;
using Pomodoro.DataAccess.Entities;

namespace Pomodoro.Services.Mapping
{
    public class PomodoroProfile : Profile
    {
        public PomodoroProfile()
        {
            CreateMap<PomodoroModel, PomodoroEntity>().ReverseMap();
        }
    }
}
