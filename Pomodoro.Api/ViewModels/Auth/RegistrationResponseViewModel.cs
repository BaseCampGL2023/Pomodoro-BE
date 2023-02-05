// <copyright file="RegistrationResponseViewModel.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

namespace Pomodoro.Api.ViewModels.Auth
{
    /// <summary>
    /// Represent registration request result.
    /// </summary>
    public class RegistrationResponseViewModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether registration request successful,
        /// TRUE if successful, FALSE otherwise.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets list of registration request model validation errors.
        /// </summary>
        public List<string>? Errors { get; set; }
    }
}
