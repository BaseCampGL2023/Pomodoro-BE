using Pomodoro.Core.Models.Base;

namespace Pomodoro.Core.Models.Statistics
{
    public class DailyStatistics : BaseUserOrientedModel
    {
        public DateOnly Day { get; set; }

        public List<AnalyticsPerHour>? AnalyticsPerHours { get; set; }
    }
}
