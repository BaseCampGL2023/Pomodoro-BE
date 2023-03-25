// <copyright file="ScheduleUtilityTest.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Pomodoro.Dal.Entities;
using Pomodoro.Dal.Enums;
using Pomodoro.Services.Utilities;

namespace Pomodoro.Tests.ServiceTests.UtilityTests
{
    /// <summary>
    /// Perform tests for schedule utility class.
    /// </summary>
    public class ScheduleUtilityTest
    {
        /// <summary>
        /// Virefy that IsCanCreateTask method works.
        /// </summary>
        /// <param name="scheduleType">Schedule type.</param>
        /// <param name="template">Schedule template.</param>
        /// <param name="scheduleStart">Schedule start datetime.</param>
        /// <param name="scheduleFinish">Schedule finish datetime.</param>
        /// <param name="fromDate">Start period checking for task creation.</param>
        /// <param name="toDate">End of period checking for task creation.</param>
        /// <param name="expected">Expected result.</param>
        [Theory]
        [InlineData(ScheduleType.EveryDay, "", "2008-05-15 17:34:42Z", "", "2008-05-16 7:34:42Z", "2008-05-17 7:34:42Z", true)]
        [InlineData(ScheduleType.EveryDay, "", "2008-05-15 17:34:42Z", "", "2008-05-16 7:34:42Z", "2008-05-15 7:34:42Z", false)]
        [InlineData(ScheduleType.EveryDay, "", "2008-05-15 17:34:42Z", "", "2008-05-16 7:34:42Z", "2008-05-16 11:34:42Z", false)]
        [InlineData(ScheduleType.EveryDay, "", "2008-05-15 17:34:42Z", "2008-07-15 17:34:42Z", "2008-08-16 7:34:42Z", "2008-08-22 11:34:42Z", false)]
        [InlineData(ScheduleType.WorkDay, "", "2023-03-20 13:34:42Z", "2023-04-20 13:34:42Z", "2023-03-23 13:34:42Z", "2023-03-24 13:34:42Z", true)]
        [InlineData(ScheduleType.WorkDay, "", "2023-03-20 13:34:42Z", "2023-04-20 13:34:42Z", "2023-03-24 17:34:42Z", "2023-03-25 13:34:42Z", false)]
        [InlineData(ScheduleType.WorkDay, "", "2023-03-20 13:34:42Z", "2023-04-20 13:34:42Z", "2023-03-24 12:34:42Z", "2023-03-25 13:34:42Z", true)]
        [InlineData(ScheduleType.WeekEnd, "", "2023-03-20 13:34:42Z", "2023-04-20 13:34:42Z", "2023-03-24 12:34:42Z", "2023-03-25 14:34:42Z", true)]
        [InlineData(ScheduleType.WeekEnd, "", "2023-03-20 13:34:42Z", "2023-04-20 13:34:42Z", "2023-03-25 14:34:42Z", "2023-03-26 14:34:42Z", true)]
        [InlineData(ScheduleType.WeekEnd, "", "2023-03-20 13:34:42Z", "2023-04-20 13:34:42Z", "2023-03-22 12:34:42Z", "2023-03-25 12:34:42Z", false)]
        [InlineData(ScheduleType.AnnualOnDate, "", "2023-03-20 13:34:42Z", "2025-04-20 13:34:42Z", "2023-12-22 12:34:42Z", "2024-03-25 12:34:42Z", true)]
        [InlineData(ScheduleType.AnnualOnDate, "", "2023-03-20 13:34:42Z", "2025-04-20 13:34:42Z", "2023-12-22 12:34:42Z", "2024-02-25 12:34:42Z", false)]
        [InlineData(ScheduleType.AnnualOnDate, "", "2023-03-20 13:34:42Z", "2025-04-20 13:34:42Z", "2023-12-22 12:34:42Z", "2024-01-25 12:34:42Z", false)]
        [InlineData(ScheduleType.WeekTemplate, "0101000", "2023-03-20 13:34:42Z", "2023-04-20 13:34:42Z", "2023-03-23 12:34:42Z", "2023-03-28 12:34:42Z", true)]
        [InlineData(ScheduleType.WeekTemplate, "0101000", "2023-03-20 13:34:42Z", "2023-04-20 13:34:42Z", "2023-03-23 12:34:42Z", "2023-03-26 12:34:42Z", false)]
        [InlineData(ScheduleType.WeekTemplate, "0101000", "2023-03-20 13:34:42Z", "2023-04-20 13:34:42Z", "2023-03-23 12:34:42Z", "2023-03-27 12:34:42Z", false)]
        [InlineData(
            ScheduleType.MonthTemplate,
            "1000001000000000000000000100000",
            "2023-03-20 13:34:42Z",
            "2023-04-20 13:34:42Z",
            "2023-03-23 12:34:42Z",
            "2023-03-27 12:34:42Z",
            true)]
        [InlineData(
            ScheduleType.MonthTemplate,
            "1000001000000000000000000100000",
            "2023-03-20 13:34:42Z",
            "2023-04-20 13:34:42Z",
            "2023-03-23 12:34:42Z",
            "2023-03-26 12:34:42Z",
            false)]
        [InlineData(
            ScheduleType.MonthTemplate,
            "0000001000000000000000000000001",
            "2023-03-20 13:34:42Z",
            "2024-04-20 13:34:42Z",
            "2023-04-26 12:34:42Z",
            "2023-05-03 12:34:42Z",
            false)]
        [InlineData(
            ScheduleType.EveryNDay,
            "1000",
            "2023-03-20 13:34:42Z",
            "2024-04-20 13:34:42Z",
            "2023-03-26 12:34:42Z",
            "2023-03-29 12:34:42Z",
            true)]
        [InlineData(
            ScheduleType.EveryNDay,
            "1000",
            "2023-03-20 13:34:42Z",
            "2024-04-20 13:34:42Z",
            "2023-03-25 12:34:42Z",
            "2023-03-27 12:34:42Z",
            false)]
        [InlineData(
            ScheduleType.Sequence,
            "10010",
            "2023-03-20 13:34:42Z",
            "2024-04-20 13:34:42Z",
            "2023-03-25 12:34:42Z",
            "2023-03-27 12:34:42Z",
            true)]
        [InlineData(
            ScheduleType.Sequence,
            "10010",
            "2023-03-20 13:34:42Z",
            "2024-04-20 13:34:42Z",
            "2023-03-26 12:34:42Z",
            "2023-03-27 12:34:42Z",
            false)]
        public void Verify_Can_Create_Task(
            ScheduleType scheduleType,
            string template,
            string scheduleStart,
            string scheduleFinish,
            string fromDate,
            string toDate,
            bool expected)
        {
            // Arrange
            Schedule schedule = new ()
            {
                ScheduleType = scheduleType,
                Title = "schedule",
                Template = template,
                StartDt = DateTime.Parse(scheduleStart),
                FinishDt = string.IsNullOrWhiteSpace(scheduleFinish) ? null : DateTime.Parse(scheduleFinish),
            };

            // Act
            var result = ScheduleUtility.IsCanCreateTask(schedule, DateTime.Parse(fromDate), DateTime.Parse(toDate));

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
