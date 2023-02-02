// <copyright file="SecurityContextService.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Pomodoro.Core.Interfaces.IServices;

namespace Pomodoro.Api.SecurityContext
{
    /// <summary>
    /// Represents a service for retrieving information about the current user.
    /// </summary>
    public class SecurityContextService : ISecurityContextService
    {
        /// <summary>
        /// Returns the id of the current user.
        /// </summary>
        /// <returns>A <see cref="Guid"/> object which represents the user id.</returns>
        public Guid GetCurrentUserId()
        {
            return Guid.NewGuid();
        }
    }
}
