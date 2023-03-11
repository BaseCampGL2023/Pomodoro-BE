// <copyright file="IOwnedByEntity.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

namespace Pomodoro.Dal.Entities.Base
{
    /// <summary>
    /// Define type: entities with AppUserId property.
    /// </summary>
    public interface IOwnedByEntity : IEntity
    {
        /// <summary>
        /// Gets or sets entity owner id.
        /// </summary>
        public Guid AppUserId { get; set; }
    }
}
