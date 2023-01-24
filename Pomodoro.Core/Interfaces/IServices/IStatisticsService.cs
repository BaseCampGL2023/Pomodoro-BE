using Pomodoro.Core.Models.Statistics;

namespace Pomodoro.Core.Interfaces.IServices
{
    public interface IStatisticsService
    {
        Task<DailyStatistics> GetDailyStatisticsAsync(Guid userId, DateOnly day);
        Task<MonthlyStatistics> GetMonthlyStatisticsAsync(Guid userId, int year, int month);
        Task<AnnualStatistics> GetAnnualStatisticsAsync(Guid userId, int year);
    }
}
