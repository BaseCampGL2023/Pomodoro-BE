// <copyright file="AppUserEqualityComparer.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using Pomodoro.DataAccess.Entities;

namespace Pomodoro.Tests.EqualityComparers
{
    /// <summary>
    /// Comparer for AppUser.
    /// </summary>
    internal class AppUserComparer : IEqualityComparer<AppUser?>
    {
        /// <inheritdoc/>
        public bool Equals([AllowNull] AppUser x, [AllowNull] AppUser y)
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
                && x.Name == y.Name
                && x.Email == y.Email;
        }

        /// <inheritdoc/>
        public int GetHashCode([DisallowNull] AppUser obj)
        {
            return obj.GetHashCode();
        }
    }
}
