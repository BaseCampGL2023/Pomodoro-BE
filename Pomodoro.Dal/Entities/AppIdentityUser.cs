// <copyright file="AppIdentityUser.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Identity;
using Pomodoro.Dal.Entities.Base;

namespace Pomodoro.Dal.Entities
{
    /// <summary>
    /// Represent user in authentication system.
    /// </summary>
    public class AppIdentityUser : IdentityUser<Guid>, IEntity
    {
        /// <summary>
        /// Gets or sets navigation property represents application user.
        /// </summary>
        public AppUser? AppUser { get; set; }
    }
}
