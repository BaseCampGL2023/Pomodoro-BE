// <copyright file="Completed.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

namespace Pomodoro.DataAccess.Entities
{
    public class Completed : BaseEntity
    {
        public Guid TaskId { get; set; }
        public DateTime ActualDate { get; set; }
        public int TimeSpent { get; set; }
        public float PomodorosCount { get; set; }
        public bool IsDone { get; set; }

        //
        public TaskEntity? Task { get; set; }
    }
}
