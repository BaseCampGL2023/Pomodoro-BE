// <copyright file="LoginResponseViewModel.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

namespace Pomodoro.Api.ViewModels.Auth
{
    /// <summary>
    /// Represent data for login response.
    /// </summary>
    public class LoginResponseViewModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether login request successful,
        /// TRUE if operation successful, FALSE otherwise.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets message, that describe login request result.
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Gets or sets JWT Token, value sets if login request successful.
        /// </summary>
        public string? Token { get; set; }
    }
}
