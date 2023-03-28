// <copyright file="PomodoroEntity.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

namespace Pomodoro.DataAccess.Entities
{
    public class PomodoroEntity : BaseEntity
    {
        public Guid TaskId { get; set; }
        public DateTime ActualDate { get; set; }
        public int TimeSpent { get; set; }
        public bool TaskIsDone { get; set; }

        //
        public TaskEntity? Task { get; set; }
    }
}
