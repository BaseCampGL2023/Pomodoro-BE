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
    internal static class ScheduleUtility
    {
        /// <summary>
        /// Create first task for corresponding schedule.
        /// </summary>
        /// <param name="model">Schedule.</param>
        /// <param name="ownerId">Owner Id.</param>
        /// <returns>Task.</returns>
        public static AppTask CreateFirstTask(ScheduleModel model, Guid ownerId)
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
                    task.StartDt = model.StartDt.AddYears(1);
                    break;
                case ScheduleType.WeekTemplate:
                    while (template[(int)date.DayOfWeek] != '1')
                    {
                        date = date.AddDays(1);
                    }

                    task.StartDt = date;
                    break;
                case ScheduleType.MonthTemplate:
                    // TODO: Delete month day forward, check new month
                case ScheduleType.MonthDayForwardTemplate:
                    while (template[date.Day] != '1')
                    {
                        date = date.AddDays(1);
                    }

                    task.StartDt = date;
                    break;
                case ScheduleType.MonthDayBackwardTemplate:
                    // TODO: check to don't planning in previous month.
                    int diff = DateTime.DaysInMonth(date.Year, date.Month) - 31;
                    while (template[date.Day] != '1'
                        && date.AddDays(diff) > model.StartDt)
                    {
                        date = date.AddDays(1);
                    }

                    date = date.AddDays(diff);
                    task.StartDt = date;
                    break;

                case ScheduleType.EveryNDay:
                    date = model.StartDt;
                    // TODO: check diff between now and startDate, in Sequance template to.
                    while (date < DateTime.UtcNow)
                    {
                        date = date.AddDays(template.Length);
                    }

                    task.StartDt = date;
                    break;
                case ScheduleType.Sequence:
                    date = model.StartDt;
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

            return task;
        }
    }
}
