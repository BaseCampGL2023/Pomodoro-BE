// <copyright file="LoginRequestModel.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;

namespace Pomodoro.Api.Models
{
    /// <summary>
    /// Represent data for login request.
    /// </summary>
    public class LoginRequestModel
    {
        /// <summary>
        /// Gets or sets email address.
        /// </summary>
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets password.
        /// </summary>
        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password length less than 8 symbols")]
        public string? Password { get; set; }
    }
}
