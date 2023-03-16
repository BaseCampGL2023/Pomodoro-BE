// <copyright file="FrequencyComparer.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using Pomodoro.DataAccess.Entities;

namespace Pomodoro.Tests.EqualityComparers
{
    /// <summary>
    /// Frequency for Task.
    /// </summary>
    public class FrequencyComparer : IEqualityComparer<Frequency?>
    {
        /// <inheritdoc/>
        public bool Equals([AllowNull] Frequency x, [AllowNull] Frequency y)
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
                && x.FrequencyTypeId == y.FrequencyTypeId
                && x.Every == y.Every
                && x.IsCustom == y.IsCustom;
        }

        /// <inheritdoc/>
        public int GetHashCode([DisallowNull] Frequency obj)
        {
            return obj.GetHashCode();
        }
    }
}
