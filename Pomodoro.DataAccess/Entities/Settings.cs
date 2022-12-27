// <copyright file="Settings.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

namespace Pomodoro.DataAccess.Entities
{
    internal class Settings : BaseEntity
    {
        public int UserId { get; set; }
        public byte PomodoroDuration { get; set; }
        public byte ShortBreak { get; set; }
        public byte LongBreak { get; set; }
        public byte PomodorosBeforeLongBreak { get; set; }
        public bool AutostartEnabled { get; set; }

        //
        public User? User { get; set; }

        public Settings(
            int id, int userId,
            byte pomodoroDuration, byte shortBreak, byte longBreak,
            byte pomodorosBeforeLongBreak, bool autostartEnabled = false)
            : base(id)
        {
            UserId = userId;
            PomodoroDuration = pomodoroDuration;
            ShortBreak = shortBreak;
            LongBreak = longBreak;
            PomodorosBeforeLongBreak = pomodorosBeforeLongBreak;
            AutostartEnabled = autostartEnabled;
        }
    }
}
