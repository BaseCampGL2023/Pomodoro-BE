// <copyright file="FrequencyTypeComparer.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using Pomodoro.DataAccess.Entities;

namespace Pomodoro.Tests.EqualityComparers
{
    /// <summary>
    /// Frequency for Task.
    /// </summary>
    public class FrequencyTypeComparer : IEqualityComparer<FrequencyType?>
    {
        /// <inheritdoc/>
        public bool Equals([AllowNull] FrequencyType x, [AllowNull] FrequencyType y)
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
                && x.Value == y.Value;
        }

        /// <inheritdoc/>
        public int GetHashCode([DisallowNull] FrequencyType obj)
        {
            return obj.GetHashCode();
        }
    }
}
