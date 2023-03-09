// <copyright file="IEntity.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

namespace Pomodoro.Dal.Entities.Base
{
    /// <summary>
    /// Define type: entities with Guid primary key.
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// Gets or sets Id property.
        /// </summary>
        public Guid Id { get; set; }
    }
}
