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

                    IF @@ROWCOUNT <> 1 RETURN;

                    IF EXISTS (SELECT * FROM inserted WHERE IsActive = 1)
                        BEGIN

                            UPDATE {TimerSettingsTableName}
                            SET {TimerSettingsIsActiveAttribute} = 0
                            WHERE {TimerSettingsUserIdAttribute} = (SELECT {TimerSettingsUserIdAttribute} FROM inserted)
                                AND Id != (SELECT Id FROM inserted)

                        END
                    ELSE
                        BEGIN
                            DECLARE @SettingsId UNIQUEIDENTIFIER,
                                @UserId UNIQUEIDENTIFIER                       

                            SET @UserId = (SELECT {TimerSettingsUserIdAttribute} FROM inserted)
                            IF EXISTS (SELECT * FROM {TimerSettingsTableName} 
                                WHERE IsActive = 1 AND {TimerSettingsUserIdAttribute} = @UserId)
                                RETURN
                        
                            SET @SettingsId = (SELECT Id FROM {TimerSettingsTableName}
                                WHERE {TimerSettingsUserIdAttribute} = @UserId AND
                                CreatedAt = (SELECT MAX(CreatedAt) FROM {TimerSettingsTableName} 
                                    WHERE {TimerSettingsUserIdAttribute} = @UserId))
                            
                            IF @SettingsId IS NOT NULL
                                BEGIN
                                    UPDATE {TimerSettingsTableName}
                                    SET {TimerSettingsIsActiveAttribute} = 1
                                    WHERE Id = @SettingsId
                                END
                        END
                ')");
        }

        /// <summary>
        /// Adds after delete trigger for TimerSettings table which set IsActive = true for latest
        /// added user timer settings if current active settings were deleted.
        /// </summary>
        /// <param name="builder">A builder providing a fluentish API for building migration <see cref="MigrationBuilder"/>.</param>
        public static void CreateTimerSettingsDeleteTrigger(MigrationBuilder builder)
        {
            builder.Sql($@"
                exec(N'
                    CREATE TRIGGER [dbo].[TimerSetttings_DELETE_UPDATE]
                    ON {TimerSettingsTableName}
                    AFTER DELETE
                    AS

                    IF @@ROWCOUNT <> 1 RETURN;
                
                    IF EXISTS (SELECT * FROM deleted WHERE IsActive = 1)
                    BEGIN
                        DECLARE @SettingsId UNIQUEIDENTIFIER,
                            @UserId UNIQUEIDENTIFIER
                        
                        SET @UserId = (SELECT {TimerSettingsUserIdAttribute} FROM deleted)
                        SET @SettingsId = (SELECT Id FROM {TimerSettingsTableName}
                            WHERE {TimerSettingsUserIdAttribute} = @UserId AND
                            CreatedAt = (SELECT MAX(CreatedAt) FROM {TimerSettingsTableName} 
                                WHERE {TimerSettingsUserIdAttribute} = @UserId))
                            
                        IF @SettingsId IS NOT NULL
                            BEGIN
                                UPDATE {TimerSettingsTableName}
                                SET {TimerSettingsIsActiveAttribute} = 1
                                WHERE Id = @SettingsId
                            END
                    END
            ')");
        }

        /// <summary>
        /// Adds after update trigger for TimerSettings table which provides the presence of the only active user settings.
        /// </summary>
        /// <param name="builder">A builder providing a fluentish API for building migration <see cref="MigrationBuilder"/>.</param>
        public static void CreateTimerSettingsUpdateTrigger(MigrationBuilder builder)
        {
            builder.Sql($@"
                exec(N'
                    CREATE TRIGGER [dbo].[TimerSetttings_UPDATE_UPDATE]
                    ON {TimerSettingsTableName}
                    AFTER UPDATE
                    AS

                    IF @@ROWCOUNT <> 1 RETURN;
                
                    DECLARE @prev BIT, @cur BIT
                    SET @prev = (SELECT IsActive FROM deleted)
                    SET @cur = (SELECT IsActive FROM inserted)
                    
                    IF @prev = @cur
                        RETURN;

                    IF @cur = 1
                        BEGIN
                            UPDATE {TimerSettingsTableName}
                         SET {TimerSettingsIsActiveAttribute} = 0
                            WHERE {TimerSettingsUserIdAttribute} = (SELECT {TimerSettingsUserIdAttribute} FROM inserted)
                            AND Id != (SELECT Id FROM inserted)      
                        END
                    ELSE
                        BEGIN
                        DECLARE @SettingsId UNIQUEIDENTIFIER,
                            @UserId UNIQUEIDENTIFIER
                        
                        SET @UserId = (SELECT {TimerSettingsUserIdAttribute} FROM deleted)
                        SET @SettingsId = (SELECT Id FROM {TimerSettingsTableName}
                            WHERE {TimerSettingsUserIdAttribute} = @UserId AND
                            CreatedAt = (SELECT MAX(CreatedAt) FROM {TimerSettingsTableName} 
                                WHERE {TimerSettingsUserIdAttribute} = @UserId AND Id <> (
                            SELECT Id FROM deleted
                        )))
                            
                        IF @SettingsId IS NOT NULL
                            BEGIN
                                UPDATE {TimerSettingsTableName}
                                SET {TimerSettingsIsActiveAttribute} = 1
                                WHERE Id = @SettingsId
                            END
                    END
            ')");
        }

        /// <summary>
        /// Delete timer settings after INSERT trigger.
        /// </summary>
        /// <param name="builder">A builder providing a fluentish API for building migration <see cref="MigrationBuilder"/>.</param>
        public static void DropTimerSettingsInsertTrigger(MigrationBuilder builder)
        {
            builder.Sql($"DROP TRIGGER [dbo].[TimerSettings_INSERT_UPDATE]");
        }

        /// <summary>
        /// Delete timer settings after DELETE trigger.
        /// </summary>
        /// <param name="builder">A builder providing a fluentish API for building migration <see cref="MigrationBuilder"/>.</param>
        public static void DropTimerSettingsDeleteTrigger(MigrationBuilder builder)
        {
            builder.Sql($"DROP TRIGGER [dbo].[TimerSetttings_DELETE_UPDATE]");
        }

        /// <summary>
        /// Delete timer settings after DELETE trigger.
        /// </summary>
        /// <param name="builder">A builder providing a fluentish API for building migration <see cref="MigrationBuilder"/>.</param>
        public static void DropTimerSettingsUpdateTrigger(MigrationBuilder builder)
        {
            builder.Sql($"DROP TRIGGER [dbo].[TimerSetttings_UPDATE_UPDATE]");
        }
    }
}
