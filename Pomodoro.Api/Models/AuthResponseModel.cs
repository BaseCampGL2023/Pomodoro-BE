// <copyright file="AuthResponseModel.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

namespace Pomodoro.Api.Models
{
    /// <summary>
    /// Represent auth response.
    /// </summary>
    public class AuthResponseModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether result of performing operation successfull.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets message from service.
        /// </summary>
        public string Message { get; set; } = string.Empty;
    }
}
