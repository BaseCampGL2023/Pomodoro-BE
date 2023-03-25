using Pomodoro.Core.Enums;
using Pomodoro.Core.Interfaces.IServices;
using Pomodoro.Core.Models.Statistics;
using Pomodoro.DataAccess.Repositories.Interfaces;

namespace Pomodoro.Services.Realizations
{
    public class StatisticsService: IStatisticsService
    {
        private readonly IPomodoroRepository pomodoroRepository;

        public StatisticsService(IPomodoroRepository pomodoroRepository)
        {
            this.pomodoroRepository = pomodoroRepository;
        }
        
        public async Task<DailyStatistics> GetDailyStatisticsAsync(Guid userId, DateTime day)
        {
            var dailyStatistics = new DailyStatistics
            {
                UserId = userId,
                Day = day
            };
            var pomodoros = await pomodoroRepository.FindAsync(pomo =>
                pomo.Task != null &&
                pomo.Task.UserId == userId &&
                pomo.ActualDate.Year == day.Year &&
                pomo.ActualDate.Month == day.Month &&
                pomo.ActualDate.Day == day.Day);

            if (!pomodoros.Any())
            {
                return dailyStatistics;
            }

            for (int hour = 0; hour < 23; hour += 2)
            {
                var pomodorosPerHour = pomodoros.Where(pomo => 
                    pomo.ActualDate.Hour >= hour &&
                    pomo.ActualDate.Hour < hour + 2);

                dailyStatistics.AnalyticsPerHours.Add(new AnalyticsPerHour
                {
                    Hour = hour,
                    PomodorosDone = pomodorosPerHour.Count(),
                    TimeSpent = pomodorosPerHour.Sum(pomo => pomo.TimeSpent)
                });
            }

            return dailyStatistics;
        }

        public async Task<MonthlyStatistics> GetMonthlyStatisticsAsync(Guid userId, int year, int month)
        {
            var pomodoros = await pomodoroRepository.FindAsync(pomo =>
                pomo.Task != null &&
                pomo.Task.UserId == userId &&
                pomo.ActualDate.Year == year &&
                pomo.ActualDate.Month == month);

            return new MonthlyStatistics
            {
                UserId = userId,
                Year = year,
                Month = (Month)month,
                TasksCompleted = pomodoros.Count(pomo => pomo.TaskIsDone),
                PomodorosDone = pomodoros.Count(),
                TimeSpent = pomodoros.Sum(pomo => pomo.TimeSpent)
            };
        }

        public async Task<AnnualStatistics> GetAnnualStatisticsAsync(Guid userId, int year)
        {
            var annualStatistics = new AnnualStatistics
            {
                UserId = userId,
                Year = year
            };
            var pomodoros = await pomodoroRepository.FindAsync(pomo =>
                pomo.Task != null &&
                pomo.Task.UserId == userId &&
                pomo.ActualDate.Year == year);

            if (!pomodoros.Any())
            {
                return annualStatistics;
            }

            foreach (int month in Enum.GetValues(typeof(Month)))
            {
                var pomodorosPerMonth = pomodoros
                    .Where(pomo => pomo.ActualDate.Month == month);

                annualStatistics.AnalyticsPerMonths.Add(new AnalyticsPerMonth
                {
                    Month = (Month)month,
                    PomodorosDone = pomodorosPerMonth.Count()
                });
            }

            return annualStatistics;
        }
    }
}