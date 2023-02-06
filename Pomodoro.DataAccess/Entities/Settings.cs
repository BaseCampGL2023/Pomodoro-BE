// <copyright file="Settings.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

namespace Pomodoro.DataAccess.Entities
{
    public class Settings : BaseEntity
    {
        public Guid UserId { get; set; }
        public byte PomodoroDuration { get; set; }
        public byte ShortBreak { get; set; }
        public byte LongBreak { get; set; }
        public byte PomodorosBeforeLongBreak { get; set; }
        public bool AutostartEnabled { get; set; }

        //
        public AppUser? User { get; set; }
    }
}
