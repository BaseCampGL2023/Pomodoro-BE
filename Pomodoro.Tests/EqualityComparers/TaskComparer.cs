// <copyright file="TaskComparer.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using Pomodoro.DataAccess.Entities;

namespace Pomodoro.Tests.EqualityComparers
{
    /// <summary>
    /// Comparer for Task.
    /// </summary>
    public class TaskComparer : IEqualityComparer<TaskEntity?>
    {
        /// <inheritdoc/>
        public bool Equals([AllowNull] TaskEntity x, [AllowNull] TaskEntity y)
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
                && x.FrequencyId == y.FrequencyId
                && x.Title == y.Title
                && x.InitialDate == y.InitialDate
                && x.AllocatedTime == y.AllocatedTime;
        }

        /// <inheritdoc/>
        public int GetHashCode([DisallowNull] TaskEntity obj)
        {
            return obj.GetHashCode();
        }
    }
}
