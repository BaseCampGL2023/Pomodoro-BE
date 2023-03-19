// <copyright file="Category.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Pomodoro.Dal.Entities.Base;

namespace Pomodoro.Dal.Entities
{
    /// <summary>
    /// Describe category for user tasks.
    /// </summary>
    public class Category : BaseEntity, IBelongEntity
    {
        /// <summary>
        /// Gets or sets category name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets category description.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets foreign key to AppUser Entity.
        /// </summary>
        public Guid AppUserId { get; set; }

        /// <summary>
        /// Gets or sets navigation property represents application user.
        /// </summary>
        public AppUser? AppUser { get; set; }

        /// <summary>
        /// Gets or sets collection of schedules.
        /// </summary>
        public ICollection<Schedule>? Schedules { get; set; } = new List<Schedule>();

        /// <summary>
        /// Gets or sets collection of tasks.
        /// </summary>
        public ICollection<AppTask> Tasks { get; set; } = new List<AppTask>();
    }
}
