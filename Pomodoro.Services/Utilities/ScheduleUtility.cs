// <copyright file="ScheduleUtility.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>


using Pomodoro.Services.Models;

namespace Pomodoro.Services.Utilities
{
    /// <summary>
    /// Convert ScheduleModel to long number and vice versa.
    /// </summary>
    internal static class ScheduleUtility
    {
        /// <summary>
        /// Convert ScheduleModel to long number.
        /// </summary>
        /// <param name="model">ScheduleModel instance <see cref="ScheduleModel"/>.</param>
        /// <returns>8 byte number.</returns>
        /*public static long ConvertToLong(ScheduleModel model)
        {
            byte scheduleTemplate = (byte)model.Type;
            int duration;
            long template = 0;

            if (scheduleTemplate < 4)
            {
                return (long)scheduleTemplate;
            }

            duration = model.Days.Length;
            for (int i = 0; i < duration; i++)
            {
                if (model.Days[i] != 0)
                {
                    long day = (long)Math.Pow(2, i);
                    template |= day;
                }
            }

            return (template << 16) | ((uint)duration << 8) | scheduleTemplate;
        }*/

        /// <summary>
        /// Convert long number to ScheduleModel.
        /// </summary>
        /// <param name="schedule">8 byte number.</param>
        /// <returns>Schedule model object.</returns>
        /// <exception cref="ArgumentException">Throws if template can't be parsed.</exception>
        /*public static ScheduleModel ConvertToScheduleModel(long schedule)
        {
            var scheduleType = (ScheduleType)(((ulong)schedule) & 0b_1111_1111);
            ScheduleModel model = new ()
            {
                Type = scheduleType,
            };
            switch (scheduleType)
            {
                case ScheduleType.WeekEnd:
                    model.Days = new int[] { 1, 0, 0, 0, 0, 0, 1 };
                    break;
                case ScheduleType.WorkDay:
                    model.Days = new int[] { 0, 1, 1, 1, 1, 1, 0 };
                    break;
                case ScheduleType.AnnualOnDate:
                    break;
                case ScheduleType.WeekTemplate:
                case ScheduleType.MonthTemplate:
                case ScheduleType.MonthDayForwardTemplate:
                case ScheduleType.MonthDayBackwardTemplate:
                case ScheduleType.EveryNDay:
                case ScheduleType.Sequence:
                    model.Days = ParseTemplate(schedule);
                    break;
                default:
                    throw new ArgumentException("Can't parse scheduleType");
            }

            return model;
        }*/

        private static int[] ParseTemplate(long template)
        {
            template >>= 8;
            var duration = (int)(template & 0b_1111_1111);
            if (duration > 45)
            {
                throw new ArgumentException("Duration of period more than 45");
            }

            int[] arr = new int[duration];
            template >>= 8;
            if (template == 0)
            {
                throw new ArgumentException("Empty template");
            }

            for (int i = 0; i < duration; i++)
            {
                if ((template & (long)Math.Pow(2, i)) != 0)
                {
                    arr[i] = 1;
                }
            }

            return arr;
        }
    }
}
