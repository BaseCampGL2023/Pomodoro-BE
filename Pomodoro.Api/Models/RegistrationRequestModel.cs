// <copyright file="RegistrationRequestModel.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;
using Pomodoro.Dal.Configs;

namespace Pomodoro.Api.Models
{
    /// <summary>
    /// Represent data for registration request.
    /// </summary>
    public class RegistrationRequestModel
    {
        /// <summary>
        /// Gets or sets user name.
        /// </summary>
        [Required(ErrorMessage = "Name is requireed.")]
        [StringLength(
            PomoConstants.AppUserNameMaxLength,
            ErrorMessage = "The {0} should be less than {1} characters.")]
        public string UserName { get; set; } = string.Empty;

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
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets confirmed user password.
        /// </summary>
        [Required(ErrorMessage = "Confirmed password is required.")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Confirmation must match password.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}