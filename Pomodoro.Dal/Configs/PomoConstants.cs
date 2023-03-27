// <copyright file="PomoConstants.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

namespace Pomodoro.Dal.Configs
{
    /// <summary>
    /// Store app specific constants.
    /// </summary>
    public static class PomoConstants
    {
        /// <summary>
        /// Defines AppUser entity Name property max length.
        /// </summary>
        public const int AppUserNameMaxLength = 50;

        /// <summary>
        /// Defines Task entity Description property max length.
        /// </summary>
        public const int TaskDescriptionMaxLength = 4000;

        /// <summary>
        /// Defines Task entity Title property max length.
        /// </summary>
        public const int TaskTitleMaxLength = 100;

        /// <summary>
        /// Defines TimerSettings entity Name property max length.
        /// </summary>
        public const int TimerSettingsNameMaxLength = 50;

        /// <summary>
        /// Defines Schedule entity Title property max length.
        /// </summary>
        public const int ScheduleTitleMaxLength = 100;

        /// <summary>
        /// Defines Schedule entity Template property max length.
        /// </summary>
        public const int ScheduleTemplateMaxLength = 365;

        /// <summary>
        /// Defines Schedule entity Description property max length.
        /// </summary>
        public const int ScheduleDescriptionMaxLength = 2000;

        /// <summary>
        /// Defines Category entity Name property max length.
        /// </summary>
        public const int CategoryNameMaxLength = 60;

        /// <summary>
        /// Defines Category entity Description property max length.
        /// </summary>
        public const int CategoryDescriptionMaxLength = 1000;
    }
}
