using AutoMapper;
using Pomodoro.Core.Models;
using Pomodoro.DataAccess.Entities;

namespace Pomodoro.Services.Mapping
{
    public class SettingsProfile : Profile
    {
        public SettingsProfile()
        {
            CreateMap<Settings, SettingsModel>()
                .ReverseMap();
            //CreateMap<SettingsModel, Settings>()
            //    .ForMember(dest => dest.User, opt => opt.Ignore());
        }
    }
}
