using Pomodoro.Core.Interfaces.IServices;
using Pomodoro.Core.Models.Statistics;
using Pomodoro.DataAccess.Repositories.Interfaces;
using AutoMapper;
namespace Pomodoro.Services
{
    public class StatisticsService:IStatisticsService
    {
        private readonly IMapper _mapper;
        private readonly ICompletedRepository _completedRepository;

        public StatisticsService(IMapper mapper, ICompletedRepository repository)
        {
            this._mapper = mapper;
            this._completedRepository = repository;
        }
        
        public async Task<DailyStatistics> GetDailyStatisticsAsync(Guid userId, DateOnly day)
        {
            var dailystatistics = await _completedRepository.FindAsync(
                u=>u.Task != null &&
                   u.Task.UserId == userId &&
                   u.ActualDate.Year == day.Year &&
                   u.ActualDate.Month == day.Month && 
                   u.ActualDate.Day == day.Day
            );
            return  _mapper.Map<DailyStatistics>(dailystatistics);
        }

        public async Task<MonthlyStatistics> GetMonthlyStatisticsAsync(Guid userId, int year, int month)
        {
            var monthlystatistics = await _completedRepository.FindAsync(
                m=> m.Task != null &&
                    m.Task.UserId == userId && 
                    m.ActualDate.Year == year && 
                    m.ActualDate.Month == month );

            return _mapper.Map<MonthlyStatistics>(monthlystatistics);
        }

        public async Task<AnnualStatistics> GetAnnualStatisticsAsync(Guid userId, int year)
        {
            var annualstatistics = await _completedRepository.FindAsync(
                a => a.Task != null &&
                     a.Task.UserId == userId && 
                     a.ActualDate.Year == year
            );
            return _mapper.Map<AnnualStatistics>(annualstatistics);
        }
    }
}