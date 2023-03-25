﻿// <copyright file="TaskEntity.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;

namespace Pomodoro.DataAccess.Entities
{
    public class TaskEntity : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid FrequencyId { get; set; }

        [MaxLength(50)]
        public string Title { get; set; } = string.Empty;
        public DateTime InitialDate { get; set; }
        public short AllocatedTime { get; set; }

        //
        public AppUser? User { get; set; }
        public Frequency? Frequency { get; set; }
        public ICollection<PomodoroEntity>? Pomodoros { get; set; }
    }
}