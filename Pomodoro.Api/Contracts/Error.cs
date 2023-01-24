// <copyright file="Error.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using System.Text.Json;

namespace Pomodoro.Api.Contracts
{
    /// <summary>
    /// Represents a class that will be responsed in case of error.
    /// </summary>
    public class Error
    {
        /// <summary>
        /// Gets or sets status code of error.
        /// </summary>
        public string? StatusCode { get; set; }

        /// <summary>
        /// Gets or sets error message.
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Serializes the <see cref="Error"/> in json.
        /// </summary>
        /// <returns>Json serialized <see cref="Error"/>.</returns>
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
