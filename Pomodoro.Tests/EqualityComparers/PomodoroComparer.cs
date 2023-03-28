// <copyright file="PomodoroComparer.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using Pomodoro.DataAccess.Entities;

namespace Pomodoro.Tests.EqualityComparers
{
    /// <summary>
    /// Comparer for Pomodoro.
    /// </summary>
    public class PomodoroComparer : IEqualityComparer<PomodoroEntity?>
    {
        /// <inheritdoc/>
        public bool Equals([AllowNull] PomodoroEntity x, [AllowNull] PomodoroEntity y)
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
                && x.TaskId == y.TaskId
                && x.ActualDate == y.ActualDate
                && x.TimeSpent == y.TimeSpent
                && x.TaskIsDone == y.TaskIsDone;
        }

        /// <inheritdoc/>
        public int GetHashCode([DisallowNull] PomodoroEntity obj)
        {
            return obj.GetHashCode();
        }
    }
}
