// <copyright file="SettingsComparer.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using Pomodoro.DataAccess.Entities;

namespace Pomodoro.Tests.EqualityComparers
{
    /// <summary>
    /// Comparer for Task.
    /// </summary>
    public class SettingsComparer : IEqualityComparer<Settings?>
    {
        /// <inheritdoc/>
        public bool Equals([AllowNull] Settings x, [AllowNull] Settings y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return x.Id == y.Id
                && x.UserId == y.UserId
                && x.PomodoroDuration == y.PomodoroDuration
                && x.ShortBreak == y.ShortBreak
                && x.LongBreak == y.LongBreak
                && x.PomodorosBeforeLongBreak == y.PomodorosBeforeLongBreak
                && x.AutostartEnabled == y.AutostartEnabled;
        }

        /// <inheritdoc/>
        public int GetHashCode([DisallowNull] Settings obj)
        {
            return obj.GetHashCode();
        }
    }
}
