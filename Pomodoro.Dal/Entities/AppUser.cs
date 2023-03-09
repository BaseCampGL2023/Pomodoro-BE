// <copyright file="AppUser.cs" company="PomodoroGroup_GL_BaseCamp">
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
        public Guid IdentityUserId { get; set; }

        /// <summary>
        /// Gets or sets navigation property represents user in authentication system.
        /// </summary>
        public AppIdentityUser? AppIdentityUser { get; set; }
    }
}
