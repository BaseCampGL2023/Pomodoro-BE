// <copyright file="ScheduleType.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

namespace Pomodoro.Dal.Enums
{
    /// <summary>
    /// Defines the schedule type.
    /// </summary>
    public enum ScheduleType
    {
        /// <summary>
        /// Every work day: monday, tuesday, wednesday, thursday, friday.
        /// </summary>
        WorkDay = 0,

        /// <summary>
        /// Saturady and sunday.
        /// </summary>
        WeekEnd = 1,

        /// <summary>
        /// Defines a specific date of the year
        /// </summary>
        AnnualOnDate = 2,

        /// <summary>
        /// Template to select specific days of the week.
        /// </summary>
        WeekTemplate = 4,

        /// <summary>
        /// Template to select specific days of the month (dates 0..31).
        /// </summary>
        MonthTemplate = 5,

        /// <summary>
        /// Template to select specific days counting from the first day of the month.
        /// </summary>
        MonthDayForwardTemplate = 7,

        /// <summary>
        /// Template to select specific days counting from the last day of the month.
        /// </summary>
        MonthDayBackwardTemplate = 8,

        /// <summary>
        /// Every N days.
        /// </summary>
        EveryNDay = 11,

        /// <summary>
        /// Defines a certain sequence of days in an interval of a specified duration.
        /// </summary>
        Sequence = 12,
    }
}
