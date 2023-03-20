// <copyright file="ResponseType.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

namespace Pomodoro.Services.Models.Results
{
    /// <summary>
    /// Defines the result type.
    /// </summary>
    public enum ResponseType
    {
        /// <summary>
        /// Execution succesfull.
        /// </summary>
        Ok,

        /// <summary>
        /// Entity for execution not found.
        /// </summary>
        NotFound,

        /// <summary>
        /// Access for execution denied.
        /// </summary>
        Forbid,

        /// <summary>
        /// Execution throw's exeption.
        /// </summary>
        Error,
    }
}
