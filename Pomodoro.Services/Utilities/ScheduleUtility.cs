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
        /// Check if existing tasks intersect with new scheduled.
        /// If intersect - return Guid of intersected task, otherwise empty Guid.
        /// </summary>
        /// <param name="tasks">New tasks.</param>
        /// <param name="existing">Existing Tasks.</param>
        /// <returns>Guid of intersected task.</returns>
        public static Guid GetIntersectedGuid(List<AppTask> tasks, List<AppTask> existing)
        {
            if (!existing.Any())
            {
                return Guid.Empty;
            }

            tasks.Sort((t1, t2) => t1.StartDt.CompareTo(t2.StartDt));
            existing.Sort((t1, t2) => t1.StartDt.CompareTo(t2.StartDt));

            int total = tasks.Count + existing.Count;
            int pt = 0;
            int pe = 0;
            while (total-- > 0)
            {
                if (tasks[pt].StartDt < existing[pe].StartDt)
                {
                    DateTime finish = tasks[pt].StartDt.AddSeconds(
                        tasks[pt].AllocatedDuration.TotalSeconds);
                    if (finish <= existing[pe].StartDt)
                    {
                        if (pt < tasks.Count - 1)
                        {
                            pt++;
                            continue;
                        }

                        return Guid.Empty;
                    }
                    else
                    {
                        return existing[pe].Id;
                    }
                }
                else
                {
                    DateTime finish = existing[pe].StartDt.AddSeconds(
                        existing[pe].AllocatedDuration.TotalSeconds);
                    if (finish <= tasks[pt].StartDt)
                    {
                        if (pe < existing.Count - 1)
                        {
                            pe++;
                            continue;
                        }

                        return Guid.Empty;
                    }
                    else
                    {
                        return existing[pe].Id;
                    }
                }
            }

            return Guid.Empty;
        }

        /// <summary>
        /// Create tasks for given schedule.
        /// </summary>
        /// <param name="model">Schedule model.</param>
        /// <exception cref="NotImplementedException">Throws if ScheduleType unknown.</exception>
        /// <returns>Tasks for given schedule.</returns>
        public static List<AppTask> CreateTasks(ScheduleModel model)
        {
            return model.ScheduleType switch
            {
                ScheduleType.EveryDay => CreateEveryday(model),
                ScheduleType.WorkDay => CreateWorkday(model),
                ScheduleType.WeekEnd => CreateWeekEnd(model),
                ScheduleType.AnnualOnDate => CreateAnnual(model),
                ScheduleType.WeekTemplate => CreateWeek(model),
                ScheduleType.MonthTemplate => CreateMonth(model),
                ScheduleType.EveryNDay => CreateEveryNDay(model),
                ScheduleType.Sequence => CreateSequence(model),
                _ => throw new NotImplementedException(),
            };
        }

        /// <summary>
        /// Check is it possible to create new task.
        /// </summary>
        /// <param name="model">Schedule.</param>
        /// <param name="fromDate">Start Date.</param>
        /// <param name="toDate">End Date.</param>
        /// <returns>Bool.</returns>
        /// <exception cref="ArgumentException">Throws if start date less than schedule startdate.</exception>
        public static bool IsCanCreateTask(ScheduleModel model, DateTime fromDate, DateTime toDate)
        {
            Schedule schedule = model.ToDalEntity(model.OwnerId);
            return IsCanCreateTask(schedule, fromDate, toDate);
        }

        /// <summary>
        /// Check is it possible to create new task.
        /// </summary>
        /// <param name="schedule">Schedule.</param>
        /// <param name="fromDate">Start Date.</param>
        /// <param name="toDate">End Date.</param>
        /// <returns>Bool.</returns>
        /// <exception cref="ArgumentException">Throws if start date less than schedule startdate.</exception>
        public static bool IsCanCreateTask(Schedule schedule, DateTime fromDate, DateTime toDate)
        {
            if (fromDate > toDate)
            {
                return false;
            }

            if (fromDate > schedule.FinishAtDt)
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

        private static List<AppTask> CreateSequence(ScheduleModel model)
        {
            List<AppTask> tasks = new ();
            DateTime current = model.StartDt;
            DateTime finish = model.FinishAt;
            int idx = 1;
            int pos = 0;
            var template = model.Template.ToCharArray();

            while (current <= finish)
            {
                if (template[pos] == '1')
                {
                    AppTask task = new ()
                    {
                        Title = model.Title,
                        Description = model.Description,
                        SequenceNumber = idx,
                        AllocatedDuration = TimeSpan.FromSeconds(model.AllocatedDuration),
                        CategoryId = model.CategoryId,
                        AppUserId = model.OwnerId,
                        StartDt = current,
                    };

                    tasks.Add(task);
                    idx++;
                    pos = pos < template.Length - 1 ? pos + 1 : 0;
                }

                current = current.AddDays(template.Length);
            }

            return tasks;
        }

        private static List<AppTask> CreateEveryNDay(ScheduleModel model)
        {
            List<AppTask> tasks = new ();
            DateTime current = model.StartDt;
            DateTime finish = model.FinishAt;
            int idx = 1;
            var template = model.Template.ToCharArray();

            while (current <= finish)
            {
                AppTask task = new ()
                {
                    Title = model.Title,
                    Description = model.Description,
                    SequenceNumber = idx,
                    AllocatedDuration = TimeSpan.FromSeconds(model.AllocatedDuration),
                    CategoryId = model.CategoryId,
                    AppUserId = model.OwnerId,
                    StartDt = current,
                };

                tasks.Add(task);
                idx++;
                current = current.AddDays(template.Length);
            }

            return tasks;
        }

        private static List<AppTask> CreateMonth(ScheduleModel model)
        {
            List<AppTask> tasks = new ();
            DateTime current = model.StartDt;
            DateTime finish = model.FinishAt;
            int idx = 1;
            var template = model.Template.ToCharArray();

            while (current <= finish)
            {
                if (template[current.Day - 1] == '1')
                {
                    AppTask task = new ()
                    {
                        Title = model.Title,
                        Description = model.Description,
                        SequenceNumber = idx,
                        AllocatedDuration = TimeSpan.FromSeconds(model.AllocatedDuration),
                        CategoryId = model.CategoryId,
                        AppUserId = model.OwnerId,
                        StartDt = current,
                    };
                    tasks.Add(task);
                    idx++;
                }

                current = current.AddDays(1);
            }

            return tasks;
        }

        private static List<AppTask> CreateWeek(ScheduleModel model)
        {
            List<AppTask> tasks = new ();
            DateTime current = model.StartDt;
            DateTime finish = model.FinishAt;
            int idx = 1;
            var template = model.Template.ToCharArray();

            while (current <= finish)
            {
                if (template[(int)current.DayOfWeek] == '1')
                {
                    AppTask task = new ()
                    {
                        Title = model.Title,
                        Description = model.Description,
                        SequenceNumber = idx,
                        AllocatedDuration = TimeSpan.FromSeconds(model.AllocatedDuration),
                        CategoryId = model.CategoryId,
                        AppUserId = model.OwnerId,
                        StartDt = current,
                    };
                    tasks.Add(task);
                    idx++;
                }

                current = current.AddDays(1);
            }

            return tasks;
        }

        private static List<AppTask> CreateAnnual(ScheduleModel model)
        {
            List<AppTask> tasks = new ();
            DateTime current = model.StartDt;
            DateTime finish = model.FinishAt;
            int idx = 1;

            while (current <= finish)
            {
                AppTask task = new ()
                {
                    Title = model.Title,
                    Description = model.Description,
                    SequenceNumber = idx,
                    AllocatedDuration = TimeSpan.FromSeconds(model.AllocatedDuration),
                    CategoryId = model.CategoryId,
                    AppUserId = model.OwnerId,
                    StartDt = current,
                };
                tasks.Add(task);
                current = current.AddYears(1);
                idx++;
            }

            return tasks;
        }

        private static List<AppTask> CreateWeekEnd(ScheduleModel model)
        {
            List<AppTask> tasks = new ();
            DateTime current = model.StartDt;
            DateTime finish = model.FinishAt;
            int idx = 1;

            while (current <= finish)
            {
                if (current.DayOfWeek == DayOfWeek.Sunday
                    || current.DayOfWeek == DayOfWeek.Saturday)
                {
                    AppTask task = new ()
                    {
                        Title = model.Title,
                        Description = model.Description,
                        SequenceNumber = idx,
                        AllocatedDuration = TimeSpan.FromSeconds(model.AllocatedDuration),
                        CategoryId = model.CategoryId,
                        AppUserId = model.OwnerId,
                        StartDt = current,
                    };
                    tasks.Add(task);
                    idx++;
                }

                current = current.AddDays(1);
            }

            return tasks;
        }

        private static List<AppTask> CreateWorkday(ScheduleModel model)
        {
            List<AppTask> tasks = new ();
            DateTime current = model.StartDt;
            DateTime finish = model.FinishAt;
            int idx = 1;

            while (current <= finish)
            {
                if (current.DayOfWeek != DayOfWeek.Sunday
                    && current.DayOfWeek != DayOfWeek.Saturday)
                {
                    AppTask task = new ()
                    {
                        Title = model.Title,
                        Description = model.Description,
                        SequenceNumber = idx,
                        AllocatedDuration = TimeSpan.FromSeconds(model.AllocatedDuration),
                        CategoryId = model.CategoryId,
                        AppUserId = model.OwnerId,
                        StartDt = current,
                    };
                    tasks.Add(task);
                    idx++;
                }

                current = current.AddDays(1);
            }

            return tasks;
        }

        private static List<AppTask> CreateEveryday(ScheduleModel model)
        {
            List<AppTask> tasks = new ();
            DateTime current = model.StartDt;
            DateTime finish = model.FinishAt;
            int idx = 1;

            while (current <= finish)
            {
                AppTask task = new ()
                {
                    Title = model.Title,
                    Description = model.Description,
                    SequenceNumber = idx,
                    AllocatedDuration = TimeSpan.FromSeconds(model.AllocatedDuration),
                    CategoryId = model.CategoryId,
                    AppUserId = model.OwnerId,
                    StartDt = current,
                };
                tasks.Add(task);
                current = current.AddDays(1);
                idx++;
            }

            return tasks;
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
