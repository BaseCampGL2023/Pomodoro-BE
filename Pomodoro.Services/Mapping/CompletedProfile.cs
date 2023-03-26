using AutoMapper;
using Pomodoro.Core.Models;
using Pomodoro.DataAccess.Entities;

namespace Pomodoro.Services.Mapping
{
    public class CompletedProfile : Profile
    {
        public CompletedProfile()
        {
            CreateMap<CompletedModel, Completed>().ReverseMap();
        }
    }
}
