﻿using Pomodoro.Core.Enums;
using Pomodoro.Core.Models.Base;

namespace Pomodoro.Core.Models.Statistics
{
    public class MonthlyStatistics : BaseUserOrientedModel
    {
        public int Year { get; set; }
        public Month Month { get; set; }

        public int TasksCompleted { get; set; }
        public int TimeSpent { get; set; }
        public int PomodorosDone { get; set; }
    }
}
