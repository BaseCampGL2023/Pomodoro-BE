using Pomodoro.Core.Enums;
using Pomodoro.Core.Interfaces.IServices;
using Pomodoro.Core.Models.Statistics;
using Pomodoro.DataAccess.Repositories.Interfaces;

namespace Pomodoro.Services.Realizations
{
    public class StatisticsService:IStatisticsService
    {
    
        private readonly ICompletedRepository _completedRepository;

        public StatisticsService( ICompletedRepository repository)
        {
            this._completedRepository = repository;
        }
        
        public async Task<DailyStatistics> GetDailyStatisticsAsync(Guid userId, DateOnly day)
        {
            DailyStatistics dailyStatistics = new DailyStatistics();

            dailyStatistics.UserId = userId;
            dailyStatistics.Day = day;
            
            var statistics = await _completedRepository.FindAsync(
                u=>u.Task != null &&
                   u.Task.UserId == userId &&
                   u.ActualDate.Year == day.Year &&
                   u.ActualDate.Month == day.Month && 
                   u.ActualDate.Day == day.Day
            );
            
            for (int i = 0; i < 24; i++) 
            {
                AnalyticsPerHour analytics = new AnalyticsPerHour();
                
                var hours = statistics.Where(t => t.ActualDate.Hour == i);

                analytics.Hour = i;

                foreach (var v in hours)
                {
                    analytics.PomodorosDone += v.PomodorosCount;
                    analytics.TimeSpent += v.TimeSpent;
                }
                
                dailyStatistics.AnalyticsPerHours?.Add(analytics);

            }

            return dailyStatistics;

        }

        public async Task<MonthlyStatistics> GetMonthlyStatisticsAsync(Guid userId, int year, int month)
        {
            MonthlyStatistics monthlyStatistics = new MonthlyStatistics();
            
            monthlyStatistics.UserId = userId;
            monthlyStatistics.Year = year;
            monthlyStatistics.Month = (Month)month;

            var statistics = await _completedRepository.FindAsync(
                m=> m.Task != null &&
                    m.Task.UserId == userId && 
                    m.ActualDate.Year == year && 
                    m.ActualDate.Month == month );
            
            monthlyStatistics.TasksCompleted = statistics.Count();
            
            foreach (var p in statistics)
            {
                monthlyStatistics.PomodorosDone += p.PomodorosCount;
                monthlyStatistics.TimeSpent += p.TimeSpent;
            }
            
            return monthlyStatistics;
        }

        public async Task<AnnualStatistics> GetAnnualStatisticsAsync(Guid userId, int year)
        {
            AnnualStatistics annualStatistics = new AnnualStatistics();
            
            annualStatistics.UserId = userId;
            annualStatistics.Year = year;
            
            var statistics = await _completedRepository.FindAsync(
                a => a.Task != null &&
                     a.Task.UserId == userId && 
                     a.ActualDate.Year == year
            );
            
            
            foreach (int m in Enum.GetValues(typeof(Month)))
            {
                
                AnalyticsPerMonth temp = new AnalyticsPerMonth();

                var months = statistics.Where(c => c.ActualDate.Month == m);
                
                temp.Month = (Month)m;
                
                foreach (var d in months)
                {
                    temp.PomodorosDone += d.PomodorosCount;
                }
                
                annualStatistics.AnalyticsPerMonths?.Add(temp);
            }

            return annualStatistics;

        }
    }
}