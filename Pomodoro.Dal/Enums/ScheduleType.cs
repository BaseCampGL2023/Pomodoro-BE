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
        /// Template for everyday tasks.
        /// </summary>
        EveryDay = 1,

        /// <summary>
        /// Every work day: monday, tuesday, wednesday, thursday, friday.
        /// </summary>
        WorkDay = 2,

        /// <summary>
        /// Saturady and sunday.
        /// </summary>
        WeekEnd = 3,

        /// <summary>
        /// Defines a specific date of the year
        /// </summary>
        AnnualOnDate = 4,

        /// <summary>
        /// Template to select specific days of the week.
        /// </summary>
        WeekTemplate = 5,

        /// <summary>
        /// Template to select specific days of the month (dates 0..31).
        /// </summary>
        MonthTemplate = 6,

        /// <summary>
        /// Every N days.
        /// </summary>
        EveryNDay = 7,

        /// <summary>
        /// Defines a certain sequence of days in an interval of a specified duration.
        /// </summary>
        Sequence = 8,
    }
}
