// <copyright file="ServiceResponse.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

namespace Pomodoro.Services.Models.Results
{
    /// <summary>
    /// The object returned by the service method.
    /// </summary>
    /// <typeparam name="T">Represent result Data type.</typeparam>
    public class ServiceResponse<T>
    {
        /// <summary>
        /// Gets a value indicating whether execution was successful.
        /// </summary>
        public bool Success => this.Result == ResponseType.Ok;

        /// <summary>
        /// Gets or sets result of request execution.
        /// </summary>
        public ResponseType Result { get; set; } = ResponseType.Error;

        /// <summary>
        /// Gets or sets returned data.
        /// </summary>
        public T? Data { get; set; } = default;

        /// <summary>
        /// Gets or sets error message.
        /// </summary>
        public string Message { get; set; } = "Something went wrong";
    }
}
