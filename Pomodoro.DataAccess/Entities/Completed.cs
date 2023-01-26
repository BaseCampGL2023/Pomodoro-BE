// <copyright file="Completed.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

namespace Pomodoro.DataAccess.Entities
{
    public class Completed : BaseEntity
    {
        public int TaskId { get; set; }
        public DateTime ActualDate { get; set; }
        public float TimeSpent { get; set; }
        public float PomodorosCount { get; set; }

        //
        public TaskEntity? Task { get; set; }
    }
}
