// <copyright file="BaseEntity.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pomodoro.Dal.Entities.Base
{
    /// <summary>
    /// Abstract class, define type: entities with Guid primary key.
    /// </summary>
    public abstract class BaseEntity : IEntity
    {
        /// <inheritdoc/>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
    }
}
