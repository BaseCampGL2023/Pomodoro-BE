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

        /// <summary>
        /// Adds after insert trigger for TimerSettings table which set IsActive = false for previous user timer settings.
        /// </summary>
        /// <param name="builder">A builder providing a fluentish API for building migration <see cref="MigrationBuilder"/>.</param>
        public static void CreateTimerSettingsInsertTrigger(MigrationBuilder builder)
        {
            builder.Sql($@"
                exec(N'
                    CREATE TRIGGER [dbo].[TimerSettings_INSERT_UPDATE]
                    ON {TimerSettingsTableName}
                    AFTER INSERT
                    AS
                    UPDATE {TimerSettingsTableName}
                    SET {TimerSettingsIsActiveAttribute} = 0
                    WHERE {TimerSettingsUserIdAttribute} = (SELECT {TimerSettingsUserIdAttribute} FROM inserted)
                        AND Id != (SELECT Id FROM inserted)
                ')");
        }

        /// <summary>
        /// Delete timer settings trigger.
        /// </summary>
        /// <param name="builder">A builder providing a fluentish API for building migration <see cref="MigrationBuilder"/>.</param>
        public static void DropTimerSettingsInsertTrigger(MigrationBuilder builder)
        {
            builder.Sql($"DROP TRIGGER [dbo].[TimerSettings_INSERT_UPDATE]");
        }
    }
}
