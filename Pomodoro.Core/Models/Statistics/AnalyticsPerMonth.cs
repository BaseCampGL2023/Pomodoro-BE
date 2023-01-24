using Pomodoro.Core.Enums;

namespace Pomodoro.Core.Models.Statistics
{
    public class AnalyticsPerMonth
    {
        public Month Month { get; set; }
        public int PomodorosDone { get; set; }
    }
}
