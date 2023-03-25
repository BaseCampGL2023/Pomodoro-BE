// <copyright file="MigrationHelpers.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pomodoro.Dal.Data
{
    /// <summary>
    /// Class for adding triggers, procedures, view, etc. in initial migration.
    /// </summary>
    public static class MigrationHelpers
    {
        /// <summary>
        /// Table name for TimerSetting entity.
        /// </summary>
        public const string TimerSettingsTableName = "TimerSettings";

        /// <summary>
        /// Column name for TimerSetting.IsActive property.
        /// </summary>
        public const string TimerSettingsIsActiveAttribute = "IsActive";

        /// <summary>
        /// Column name for TimerSetting.AppUserId property.
        /// </summary>
        public const string TimerSettingsUserIdAttribute = "AppUserId";
    }
}
