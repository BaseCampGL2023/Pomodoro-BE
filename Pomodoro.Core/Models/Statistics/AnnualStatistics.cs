namespace Pomodoro.Core.Models.Statistics
{
    public class AnnualStatistics : BaseUserOrientedModel
    {
        public int Year { get; set; }

        public List<AnalyticsPerMonth>? AnalyticsPerMonths { get; set; }
    }
}
