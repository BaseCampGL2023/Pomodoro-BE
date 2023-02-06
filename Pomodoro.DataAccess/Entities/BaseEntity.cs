// <copyright file="BaseEntity.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Pomodoro.DataAccess.Entities.Interfaces;

namespace Pomodoro.DataAccess.Entities
{
    public class BaseEntity : IBaseEntity
    {
        public Guid Id { get; set; }
    }
}
