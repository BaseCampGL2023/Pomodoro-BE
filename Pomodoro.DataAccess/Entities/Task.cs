// <copyright file="Task.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;

namespace Pomodoro.DataAccess.Entities
{
    internal class Task : BaseEntity
    {
        public int UserId { get; set; }
        public int FrequencyId { get; set; }

        [MaxLength(50)]
        public string Title { get; set; }
        public DateTime InitialDate { get; set; }
        public short AllocatedTime { get; set; }

        //
        public User? User { get; set; }
        public Frequency? Frequency { get; set; }
        public ICollection<Completed>? CompletedTasks { get; set; }

        public Task(
            int id, int userId, int frequencyId,
            string title, DateTime initialDate, short allocatedTime)
            : base(id)
        {
            UserId = userId;
            FrequencyId = frequencyId;
            Title = title;
            InitialDate = initialDate;
            AllocatedTime = allocatedTime;
        }
    }
}
