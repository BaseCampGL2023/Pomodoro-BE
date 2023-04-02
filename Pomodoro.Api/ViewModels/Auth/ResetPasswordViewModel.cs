// <copyright file="ResetPasswordViewModel.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;

namespace Pomodoro.Api.ViewModels.Auth
{
    /// <summary>
    /// Represent data for reset password request.
    /// </summary>
    public class ResetPasswordViewModel
    {
        /// <summary>
        /// Gets or sets password reset token.
        /// </summary>
        [Required(ErrorMessage = "Token is required")]
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets user email.
        /// </summary>
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets user password.
        /// </summary>
        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password length less than 8 symbols")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets confirmed user password.
        /// </summary>
        [Required(ErrorMessage = "Confirmed password is required.")]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "Confirmation must match password.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
