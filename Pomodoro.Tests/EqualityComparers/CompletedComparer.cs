// <copyright file="CompletedComparer.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using Pomodoro.DataAccess.Entities;

namespace Pomodoro.Tests.EqualityComparers
{
    /// <summary>
    /// Comparer for Completed.
    /// </summary>
    public class CompletedComparer : IEqualityComparer<Completed?>
    {
        /// <inheritdoc/>
        public bool Equals([AllowNull] Completed x, [AllowNull] Completed y)
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
                && x.PomodorosCount == y.PomodorosCount
                && x.IsDone == y.IsDone;
        }

        /// <inheritdoc/>
        public int GetHashCode([DisallowNull] Completed obj)
        {
            return obj.GetHashCode();
        }
    }
}
