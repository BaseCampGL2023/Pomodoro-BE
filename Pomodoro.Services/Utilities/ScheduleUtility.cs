// <copyright file="ScheduleUtility.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Pomodoro.Dal.Entities;
using Pomodoro.Dal.Enums;
using Pomodoro.Services.Models;

namespace Pomodoro.Services.Utilities
{
    /// <summary>
    /// Working with schedules.
    /// </summary>
    public static class ScheduleUtility
    {
        /// <summary>
        /// Create first task for corresponding schedule.
        /// </summary>
        /// <param name="model">Schedule.</param>
        /// <param name="ownerId">Owner Id.</param>
        /// <returns>Task.</returns>
        public static AppTask? CreateFirstTask(ScheduleModel model, Guid ownerId)
        {
            AppTask task = new ()
            {
                Title = model.Title,
                Description = model.Description,
                SequenceNumber = 1,
                AllocatedDuration = TimeSpan.FromSeconds(model.AllocatedDuration),
                CategoryId = model.CategoryId,
                AppUserId = model.OwnerId != Guid.Empty ? model.OwnerId : ownerId,
            };

            var date = model.StartDt > DateTime.UtcNow ? model.StartDt
                : new DateTime(
                    DateTime.UtcNow.Year,
                    DateTime.UtcNow.Month,
                    DateTime.UtcNow.Day,
                    model.StartDt.Hour,
                    model.StartDt.Minute,
                    model.StartDt.Second);
            var template = model.Template.ToCharArray();

            switch (model.ScheduleType)
            {
                case ScheduleType.EveryDay:
                    task.StartDt = model.StartDt.AddDays(1);
                    break;
                case ScheduleType.WorkDay:
                    while (date.DayOfWeek == DayOfWeek.Sunday
                            || date.DayOfWeek == DayOfWeek.Saturday)
                    {
                        date = date.AddDays(1);
                    }

                    task.StartDt = date;
                    break;
                case ScheduleType.WeekEnd:
                    while (date.DayOfWeek != DayOfWeek.Sunday
                        && date.DayOfWeek != DayOfWeek.Saturday)
                    {
                        date = date.AddDays(1);
                    }

                    task.StartDt = date;
                    break;
                case ScheduleType.AnnualOnDate:
                    task.StartDt = model.StartDt;
                    break;
                case ScheduleType.WeekTemplate:
                    while (template[(int)date.DayOfWeek] != '1')
                    {
                        date = date.AddDays(1);
                    }

                    task.StartDt = date;
                    break;
                case ScheduleType.MonthTemplate:
                    while (template[date.Day - 1] != '1')
                    {
                        date = date.AddDays(1);
                    }

                    task.StartDt = date;
                    break;

                case ScheduleType.EveryNDay:
                    date = model.StartDt;

                    // check this diff between now and startDate, in Sequance template to.
                    while (date < DateTime.UtcNow)
                    {
                        date = date.AddDays(template.Length);
                    }

                    task.StartDt = date;
                    break;
                case ScheduleType.Sequence:
                    date = model.StartDt;

                    // check why using UTC now.
                    while (date < DateTime.UtcNow)
                    {
                        for (int i = 0; i < template.Length; i++)
                        {
                            if (template[i] == '1')
                            {
                                date = date.AddDays(i + 1);
                                if (date > DateTime.UtcNow)
                                {
                                    break;
                                }
                            }
                        }
                    }

                    task.StartDt = date;
                    break;
            }

            if (model.FinishAt != null && task.StartDt > model.FinishAt)
            {
                return null;
            }

            return task;
        }

        public static bool IsCanCreateTask(ScheduleModel model, DateTime fromDate, DateTime toDate)
        {
            Schedule schedule = model.ToDalEntity(model.OwnerId);
            return IsCanCreateTask(schedule, fromDate, toDate);
        }

        public static bool IsCanCreateTask(Schedule schedule, DateTime fromDate, DateTime toDate)
        {
            if (fromDate > toDate)
            {
                return false;
            }

            if (schedule.FinishDt != null && fromDate > schedule.FinishDt)
            {
                return false;
            }

            if (fromDate < schedule.StartDt)
            {
                throw new ArgumentException("From datetime less than schedule start datetime");
            }

            fromDate = fromDate > GetDateWithTaskTime(schedule, fromDate)
                ? GetDateWithTaskTime(schedule, fromDate).AddDays(1)
                : GetDateWithTaskTime(schedule, fromDate);
            bool result = false;
            var template = schedule.Template.ToCharArray();

            switch (schedule.ScheduleType)
            {
                case ScheduleType.EveryDay:
                    result = fromDate < toDate;
                    break;
                case ScheduleType.WorkDay:
                    while (fromDate.DayOfWeek == DayOfWeek.Sunday
                            || fromDate.DayOfWeek == DayOfWeek.Saturday)
                    {
                        fromDate = fromDate.AddDays(1);
                    }

                    result = fromDate < toDate;
                    break;
                case ScheduleType.WeekEnd:
                    while (fromDate.DayOfWeek != DayOfWeek.Sunday
                        && fromDate.DayOfWeek != DayOfWeek.Saturday)
                    {
                        fromDate = fromDate.AddDays(1);
                    }

                    result = fromDate < toDate;
                    break;
                case ScheduleType.AnnualOnDate:
                    DateTime next = schedule.StartDt;
                    while (next < toDate.AddYears(-1))
                    {
                        next = next.AddYears(1);
                    }

                    result = next > fromDate && next < toDate;
                    break;
                case ScheduleType.WeekTemplate:
                    while (template[(int)fromDate.DayOfWeek] != '1')
                    {
                        fromDate = fromDate.AddDays(1);
                    }

                    result = fromDate < toDate;
                    break;
                case ScheduleType.MonthTemplate:
                    while (template[fromDate.Day - 1] != '1')
                    {
                        fromDate = fromDate.AddDays(1);
                    }

                    result = fromDate < toDate;
                    break;

                case ScheduleType.EveryNDay:
                    DateTime current = schedule.StartDt;
                    while (current < fromDate)
                    {
                        current = current.AddDays(template.Length);
                    }

                    result = current < toDate && current > fromDate;
                    break;
                case ScheduleType.Sequence:
                    current = schedule.StartDt;
                    while (current < toDate)
                    {
                        for (int i = 0; i < template.Length; i++)
                        {
                            if (template[i] == '1')
                            {
                                current = current.AddDays(i + 1);
                                if (current > fromDate)
                                {
                                    break;
                                }
                            }
                        }

                        if (current > fromDate)
                        {
                            break;
                        }
                    }

                    result = current > fromDate && current < toDate;
                    break;
            }

            return result;
        }

        private static DateTime GetDateWithTaskTime(Schedule schedule, DateTime date)
        {
            return new DateTime(
                    date.Year,
                    date.Month,
                    date.Day,
                    schedule.StartDt.Hour,
                    schedule.StartDt.Minute,
                    schedule.StartDt.Second,
                    schedule.StartDt.Millisecond);
        }
    }
}
