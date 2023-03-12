// <copyright file="ScheduleType.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Pomodoro.Services.Enums
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
        /// Every N days.
        /// </summary>
        EveryNDay = 2,

        /// <summary>
        /// Template to select specific days of the week.
        /// </summary>
        WeekTemplate = 3,

        /// <summary>
        /// Template to select specific days of the month (dates 0..28).
        /// </summary>
        MonthTemplate = 4,

        /// <summary>
        /// Template to select specific days counting from the first day of the month.
        /// </summary>
        MonthDayForward = 5,

        /// <summary>
        /// Template to select specific days counting from the last day of the month.
        /// </summary>
        MonthDayBackward = 6,

        /// <summary>
        /// Defines a specific date of the year
        /// </summary>
        AnnualOnDate = 7,

        /// <summary>
        /// Defines a certain sequence of days in an interval of a specified duration.
        /// </summary>
        Sequence = 8,
    }
}
