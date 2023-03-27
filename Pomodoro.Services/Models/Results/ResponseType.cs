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
        /// Execution succesfull, no content for response.
        /// </summary>
        NoContent,

        /// <summary>
        /// Entity for execution not found.
        /// </summary>
        NotFound,

        /// <summary>
        /// Access for execution denied.
        /// </summary>
        Forbid,

        /// <summary>
        /// Execution throw's exeption, and this exception handled.
        /// </summary>
        Error,

        /// <summary>
        /// Unable to execute, because this cause of conflict in application state.
        /// </summary>
        Conflict,
    }
}
