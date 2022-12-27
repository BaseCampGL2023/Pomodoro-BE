// <copyright file="Completed.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

namespace Pomodoro.DataAccess.Entities
{
    internal class Completed : BaseEntity
    {
        public int? TaskId { get; set; }
        public DateTime ActualDate { get; set; }
        public float TimeSpent { get; set; }
        public float PomodorosCount { get; set; }

        //
        public Task? Task { get; set; }

        public Completed(int id, DateTime actualDate, float timeSpent, float pomodorosCount, int? taskId = null)
            : base(id)
        {
            ActualDate = actualDate;
            TimeSpent = timeSpent;
            PomodorosCount = pomodorosCount;
            TaskId = taskId;
        }
    }
}
