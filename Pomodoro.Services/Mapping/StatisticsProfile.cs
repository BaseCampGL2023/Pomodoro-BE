using AutoMapper;
using Pomodoro.Core.Models.Statistics;
using Pomodoro.DataAccess.Entities;


namespace Pomodoro.Services.Mapping;

public class StatisticsProfile : Profile
{
    public StatisticsProfile()
    {
        CreateMap<Completed, DailyStatistics>().ReverseMap();
        
        CreateMap<Completed, MonthlyStatistics>().ReverseMap();
        
        CreateMap<Completed, AnnualStatistics>().ReverseMap();
    }
}