// <copyright file="ResetPassword.cshtml.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Pomodoro.Api.Pages
{
    /// <summary>
    /// Page for creating new password.
    /// </summary>
#pragma warning disable SA1649 // File name should match first type name
    public class ResetPasswordModel : PageModel
#pragma warning restore SA1649 // File name should match first type name
    {
        /// <summary>
        /// Gets or sets user Id.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets user token.
        /// </summary>
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// Create form for reseting password.
        /// </summary>
        /// <param name="email">User email.</param>
        /// <param name="token">Reset token.</param>
        public void OnGet(string email, string token)
        {
            this.Email = email;
            this.Token = token;
        }
    }
}
