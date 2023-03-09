﻿// <copyright file="AppUser.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pomodoro.Dal.Entities.Base;

namespace Pomodoro.Dal.Entities
{
    /// <summary>
    /// Represent application user.
    /// </summary>
    public class AppUser : BaseEntity
    {
        /// <summary>
        /// Gets or sets username property.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets IdentityUser foreign key.
        /// </summary>
        public Guid AppIdentityUserId { get; set; }

        /// <summary>
        /// Gets or sets navigation property represents user in authentication system.
        /// </summary>
        public AppIdentityUser? AppIdentityUser { get; set; }

        /// <summary>
        /// Gets or sets collection of user timer settings.
        /// </summary>
        public ICollection<TimerSettings>? TimerSettings { get; set; }

        /// <summary>
        /// Gets or sets collection of user scheduled tasks.
        /// </summary>
        public ICollection<Routine>? Routins { get; set; }

        /// <summary>
        /// Gets or sets collection of user tasks.
        /// </summary>
        public ICollection<AppTask>? Tasks { get; set; }
    }
}
