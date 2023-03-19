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
        
        public async Task<DailyStatistics> GetDailyStatisticsAsync(Guid userId, DateTime day)
        {
            var dailyStatistics = new DailyStatistics
            {
                UserId = userId,
                Day = day
            };

            var statistics = await _completedRepository.FindAsync(
                u => u.Task != null &&
                   u.Task.UserId == userId &&
                   u.ActualDate.Year == day.Year &&
                   u.ActualDate.Month == day.Month &&
                   u.ActualDate.Day == day.Day
            );
            if (!statistics.Any())
            {
                return dailyStatistics;
            }

            for (int i = 0; i < 24; i+=2) 
            {
                AnalyticsPerHour analytics = new AnalyticsPerHour();
                
                var hours = statistics.Where(t => t.ActualDate.Hour == i || (t.ActualDate.Hour > i && t.ActualDate.Hour < i+2));

                analytics.Hour = i;
                
                float totalPomodorosCount = hours.Sum(c => c.PomodorosCount);

                analytics.PomodorosDone = Convert.ToInt32(totalPomodorosCount);

                analytics.TimeSpent = hours.Sum(c=>c.TimeSpent);

                dailyStatistics.AnalyticsPerHours.Add(analytics);

            }

            return dailyStatistics;

        }

        public async Task<MonthlyStatistics> GetMonthlyStatisticsAsync(Guid userId, int year, int month)
        {
            var statistics = await _completedRepository.FindAsync(
                m => m.Task != null &&
                    m.Task.UserId == userId &&
                    m.ActualDate.Year == year &&
                    m.ActualDate.Month == month
            );

            float totalPomodorosCount = statistics.Sum(c => c.PomodorosCount);

            return new MonthlyStatistics
            {
                UserId = userId,
                Year = year,
                Month = (Month)month,
                TasksCompleted = statistics.Count(),
                PomodorosDone = Convert.ToInt32(totalPomodorosCount),
                TimeSpent = statistics.Sum(c => c.TimeSpent)
            };
        }

        public async Task<AnnualStatistics> GetAnnualStatisticsAsync(Guid userId, int year)
        {
            var annualStatistics = new AnnualStatistics
            {
                UserId = userId,
                Year = year
            };

            var statistics = await _completedRepository.FindAsync(
                a => a.Task != null &&
                     a.Task.UserId == userId &&
                     a.ActualDate.Year == year
            );
            if (!statistics.Any())
            {
                return annualStatistics;
            }

            foreach (int m in Enum.GetValues(typeof(Month)))
            {
                
                AnalyticsPerMonth temp = new AnalyticsPerMonth();

                var months = statistics.Where(c => c.ActualDate.Month == m);
                
                temp.Month = (Month)m;

                float totalPomodorosCount = months.Sum(c=>c.PomodorosCount);

                temp.PomodorosDone = Convert.ToInt32(totalPomodorosCount);

                annualStatistics.AnalyticsPerMonths.Add(temp);
            }

            return annualStatistics;

        }
    }
}