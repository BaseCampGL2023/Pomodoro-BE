﻿// <copyright file="SettingsViewModel.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Pomodoro.Api.ViewModels.Base;

namespace Pomodoro.Api.ViewModels
{
    /// <summary>
    /// Represents a view model which contains user settings.
    /// </summary>
    public class SettingsViewModel : BaseViewModel
    {
        /// <summary>
        /// Gets or sets user id.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets duration (in minutes) of the pomodoro timer.
        /// </summary>
        public byte PomodoroDuration { get; set; }

        /// <summary>
        /// Gets or sets duration (in minutes) of the short break between pomodoros.
        /// </summary>
        public byte ShortBreak { get; set; }

        /// <summary>
        /// Gets or sets duration (in minutes) of the long break between pomodoros.
        /// </summary>
        public byte LongBreak { get; set; }

        /// <summary>
        /// Gets or sets number of pomodoros after which the long break starts.
        /// </summary>
        public byte PomodorosBeforeLongBreak { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether rounds should start automatically.
        /// </summary>
        public bool AutostartEnabled { get; set; }
    }
}
