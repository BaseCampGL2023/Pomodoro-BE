using Pomodoro.Core.Models.Base;

namespace Pomodoro.Core.Models.Statistics
{
    public class AnnualStatistics : BaseUserOrientedModel
    {
        public int Year { get; set; }

        public List<AnalyticsPerMonth> AnalyticsPerMonths { get; set; }
            = new List<AnalyticsPerMonth>();
    }
}
