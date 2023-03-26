// <copyright file="BaseController.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Pomodoro.Api.Controllers.Base
{
    /// <summary>
    /// Extended ControllerBase <see cref="ControllerBase"/> by adding UserName,
    /// UserId and UserCreatedAt properties.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public abstract class BaseController : ControllerBase
    {
        /// <summary>
        /// Gets authenticated user id or Guid.Empty
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
        /// Gets authenticated user signup date or
        /// DateTime.UtcNow if user not authenticated.
        /// </summary>
        protected DateTime UserCreatedAt
        {
            get
            {
                var date = this.User.FindFirst("signUpAt");
                if (date is null)
                {
                    return DateTime.UtcNow;
                }

                return DateTime.Parse(date.Value);
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
