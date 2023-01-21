using Pomodoro.Core.Models.Statistics;

namespace Pomodoro.Core.Interfaces.IServices
{
    public interface IStatisticsService
    {
        Task<DailyStatistics> GetDailyStatisticsAsync(int userId, DateOnly day);
        Task<MonthlyStatistics> GetMonthlyStatisticsAsync(int userId, int year, int month);
        Task<AnnualStatistics> GetAnnualStatisticsAsync(int userId, int year);
    }
}
