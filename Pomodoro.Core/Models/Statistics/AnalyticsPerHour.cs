﻿namespace Pomodoro.Core.Models.Statistics
{
    public class AnalyticsPerHour
    {
        public int Hour { get; set; }
        public int PomodorosDone { get; set; }
        public float TimeSpent { get; set; }
    }
}