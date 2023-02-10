// <copyright file="BaseController.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Pomodoro.Api.Controllers.Base
{
    /// <summary>
    /// Extended ControllerBase by adding UserId and UserName properties.
    /// </summary>
    public class BaseController : ControllerBase
    {
        /// <summary>
        /// Gets authenticated user id Guid.Empty
        /// if user not authenticated.
        /// </summary>
        protected Guid UserId
        {
            get
            {
                var id = this.User.FindFirst("userId");
                if (id is null)
                {
                    return Guid.Empty;
                }

                return new Guid(id.Value);
            }
        }

        /// <summary>
        /// Gets autheticated user name or "User Unknown"
        /// if user not authenticated.
        /// </summary>
        protected string UserName
        {
            get => this.User.FindFirst(ClaimTypes.Name)?.Value
                ?? "User Unknown";
        }
    }
}
